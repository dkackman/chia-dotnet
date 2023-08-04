using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Models;
using System.Globalization;

namespace CodeGen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: CodeGen <path_to_yaml> <endpoint_name>");
                return;
            }

            var filePath = args[0];
            var endpointName = args[1];
            var returnType = args.Length > 2 ? args[2] : null;

            var yamlContent = File.ReadAllText(filePath);
            var openApiDocument = new OpenApiStringReader().Read(yamlContent, out var diagnostic);

            if (diagnostic.Errors.Count > 0)
            {
                Console.WriteLine("Error reading OpenAPI document:");
                foreach (var error in diagnostic.Errors)
                {
                    Console.WriteLine(error.Message);
                }
                return;
            }

            var operation = openApiDocument.Paths[$"/{endpointName}"]?.Operations[OperationType.Post];
            if (operation == null)
            {
                Console.WriteLine($"Endpoint {endpointName} not found.");
                return;
            }

            var csharpCode = GenerateCSharpMethod(operation, returnType);
            Console.WriteLine(csharpCode);
        }

        private static string GenerateCSharpMethod(OpenApiOperation operation, string? returnType)
        {
            // Convert the operation ID from snake_case to PascalCase
            var methodName = ToPascalCase(operation.OperationId);
            var summary = operation.Summary;

            // Determine the return type
            var responseSchema = operation.Responses["200"].Content["application/json"].Schema;
            if (IsPlural(operation.OperationId) && returnType is not null)
            {
                returnType = "IEnumerable<" + returnType + ">";
            }

            // Generate method parameters, parameter comments, and data initialization
            var methodParameters = "CancellationToken cancellationToken = default";
            var parameterComments = "/// <param name=\"cancellationToken\">A token to allow the call to be cancelled</param>";
            var dataInitialization = "";
            if (operation.RequestBody != null && operation.RequestBody.Content.ContainsKey("application/json"))
            {
                var requestBodySchema = operation.RequestBody.Content["application/json"].Schema;
                foreach (var property in requestBodySchema.Properties)
                {
                    var parameterType = ConvertOpenApiTypeToCSharpType(property.Value.Type);
                    var parameterNameCamelCase = ToCamelCase(property.Key); // camelCased C# argument
                    var parameterDescription = property.Value.Description; // Description from schema
                    methodParameters = parameterType + " " + parameterNameCamelCase + ", " + methodParameters;
                    parameterComments = "/// <param name=\"" + parameterNameCamelCase + "\">" + parameterDescription + "</param>\n" + parameterComments;
                    dataInitialization += "    data." + property.Key + " = " + parameterNameCamelCase + ";\n"; // snake_cased data object field
                }
                dataInitialization = "    dynamic data = new ExpandoObject();\n" + dataInitialization;
            }

            // Generate method signature
            var methodSignature = returnType is not null ? "public async Task<" + returnType + "> " + methodName + "(" + methodParameters + ")" : "public async Task " + methodName + "(" + methodParameters + ")";

            // Determine the result key from the response schema
            var resultKey = GetResultKeyFromSchema(responseSchema);

            var methodBody = GetMethodBody(dataInitialization, returnType, operation, resultKey);

            // Generate summary comment
            var comment =
                "/// <summary>\n" +
                "/// " + summary + "\n" +
                "/// </summary>\n" +
                parameterComments +
                "\n/// <returns>A list of <see cref=\"" + returnType + "\"/></returns>";

            return comment + "\n" + methodSignature + "\n" + methodBody;
        }

        private static string GetMethodBody(string dataInitialization, string? returnType, OpenApiOperation operation, string? resultKey)
        {
            if (returnType is not null)
            {
                return "{\n" +
                       dataInitialization +
                       "    return await SendMessage<" + returnType + ">(\"" + operation.OperationId + "\", " + (string.IsNullOrEmpty(dataInitialization) ? "null" : "data") + ", \"" + resultKey + "\", cancellationToken).ConfigureAwait(false);\n" +
                       "}";
            }

            return "{\n" +
                   dataInitialization +
                   "    var resonse = await SendMessage(\"" + operation.OperationId + "\", " + (string.IsNullOrEmpty(dataInitialization) ? "null" : "data") + ", \"" + resultKey + "\", cancellationToken).ConfigureAwait(false);\n" +
                   "    return response;\n" +
                   "}";
        }
        private static string GetResponseTypeFromSchema(OpenApiSchema schema)
        {
            // If the schema uses "allOf", iterate over the combined schemas
            if (schema.AllOf.Count > 0)
            {
                foreach (var combinedSchema in schema.AllOf)
                {
                    // You can tailor this logic based on how the desired property is structured in your actual OpenAPI document
                    if (combinedSchema.Properties != null && combinedSchema.Properties.Count > 0)
                    {
                        var property = combinedSchema.Properties.Values.First();
                        return ConvertOpenApiTypeToCSharpType(property.Type);
                    }
                }
            }
            else if (schema.Properties != null && schema.Properties.Count > 0)
            {
                // If "allOf" is not used, retrieve the type of the first property
                var property = schema.Properties.Values.First();
                return ConvertOpenApiTypeToCSharpType(property.Type);
            }

            throw new InvalidOperationException("Could not determine response type from schema");
        }


        private static string? GetResultKeyFromSchema(OpenApiSchema schema)
        {
            // If the schema uses "allOf", iterate over the combined schemas
            if (schema.AllOf.Count > 0)
            {
                foreach (var combinedSchema in schema.AllOf)
                {
                    if (combinedSchema.Properties != null && combinedSchema.Properties.Count > 0)
                    {
                        return combinedSchema.Properties.Keys.FirstOrDefault();
                    }
                }
            }
            else if (schema.Properties != null && schema.Properties.Count > 0)
            {
                // If "allOf" is not used, retrieve the key of the first property
                return schema.Properties.Keys.FirstOrDefault();
            }

            throw new InvalidOperationException("Could not determine result key from schema");
        }


        private static string ToPascalCase(string snakeCase)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(snakeCase.Replace("_", " ")).Replace(" ", string.Empty);
        }

        private static string ToCamelCase(string snakeCase)
        {
            var pascalCase = ToPascalCase(snakeCase);
            return char.ToLower(pascalCase[0]) + pascalCase.Substring(1);
        }

        // Other helper methods remain the same

        private static string ConvertOpenApiTypeToCSharpType(string openApiType)
        {
            // Simple mapping between OpenAPI data types and C# data types
            return openApiType switch
            {
                "string" => "string",
                "integer" => "int",
                // Add other type mappings as needed
                _ => "object"
            };
        }
        private static bool IsPlural(string word)
        {
            // Simple check for pluralization; may need to be adapted based on your naming conventions
            return word.EndsWith("s");
        }
    }
}
