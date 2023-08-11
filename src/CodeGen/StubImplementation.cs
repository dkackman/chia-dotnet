using Microsoft.OpenApi.Readers;
using Microsoft.OpenApi.Models;
using System.Globalization;

namespace CodeGen
{
    internal static class StubImplementation
    {
        public static void GenerateImplementationStub(string swaggerFilePath, string endpointName, string? returnType)
        {
            var yamlContent = File.ReadAllText(swaggerFilePath);
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
            OpenApiSchema? responseSchema = null;
            if (operation.Responses.TryGetValue("200", out var response) &&
                response.Content.TryGetValue("application/json", out var content))
            {
                responseSchema = content.Schema;
            }

            var returnsArray = GetResponseArrayType(responseSchema) != null;
            var fullReturnType = returnsArray ? "IEnumerable<" + returnType + ">" : returnType;

            // Generate method parameters, parameter comments, and data initialization
            var methodParameters = "CancellationToken cancellationToken = default";
            var parameterComments = "/// <param name=\"cancellationToken\">A token to allow the call to be cancelled</param>";
            var dataInitialization = "";
            if (operation.RequestBody != null && operation.RequestBody.Content.ContainsKey("application/json"))
            {
                var requestBodySchema = operation.RequestBody.Content["application/json"].Schema;
                foreach (var property in requestBodySchema.Properties)
                {
                    var parameterType = property.Value.Type == "array" ? "IEnumerable<object>" : ConvertOpenApiTypeToCSharpType(property.Value.Type, property.Value.Format);

                    var defaultValue = property.Value.Default != null ? " = 0" : string.Empty;
                    var parameterNameCamelCase = ToCamelCase(property.Key); // camelCased C# argument

                    // Use paramName, paramType, and defaultValue as needed
                    var parameterDefinition = $"{parameterType} {parameterNameCamelCase}{defaultValue}";
                    // Add parameterDefinition to the list of parameters

                    var parameterDescription = property.Value.Description; // Description from schema
                    methodParameters = parameterDefinition + ", " + methodParameters;
                    parameterComments = "/// <param name=\"" + parameterNameCamelCase + "\">" + parameterDescription + "</param>\n" + parameterComments;
                    dataInitialization += "    data." + property.Key + " = " + parameterNameCamelCase + ";\n"; // snake_cased data object field
                }
                dataInitialization = "    dynamic data = new ExpandoObject();\n" + dataInitialization;
            }

            // Generate method signature
            var methodSignature = fullReturnType is not null ? "public async Task<" + fullReturnType + "> " + methodName + "(" + methodParameters + ")" : "public async Task " + methodName + "(" + methodParameters + ")";

            // Determine the result key from the response schema
            var resultKey = fullReturnType is not null ? GetResultKeyFromSchema(responseSchema) : null;

            var methodBody = GetMethodBody(dataInitialization, fullReturnType, operation, resultKey);

            var returnsComment = returnsArray ? "/// <returns>A list of <see cref=\"" + returnType + "\"/></returns>" : "/// <returns><see cref=\"" + returnType + "\"/></returns>";
            // Generate summary comment
            var comment =
                "/// <summary>\n" +
                "/// " + summary + "\n" +
                "/// </summary>\n" +
                parameterComments + "\n" +
                returnsComment;

            return comment + "\n" + methodSignature + "\n" + methodBody;
        }

        private static string GetMethodBody(string dataInitialization, string? fullReturnType, OpenApiOperation operation, string? resultKey)
        {
            if (fullReturnType is not null)
            {
                return "{\n" +
                       dataInitialization +
                       "    return await SendMessage<" + fullReturnType + ">(\"" + operation.OperationId + "\", " + (string.IsNullOrEmpty(dataInitialization) ? "null" : "data") + ", \"" + resultKey + "\", cancellationToken).ConfigureAwait(false);\n" +
                       "}";
            }
            else if (resultKey is not null)
            {
                return "{\n" +
                   dataInitialization +
                   "    var response = await SendMessage(\"" + operation.OperationId + "\", " + (string.IsNullOrEmpty(dataInitialization) ? "null" : "data") + ", \"" + resultKey + "\", cancellationToken).ConfigureAwait(false);\n" +
                   "    return response;\n" +
                   "}";
            }

            return "{\n" +
               dataInitialization +
               "    await SendMessage(\"" + operation.OperationId + "\", " + (string.IsNullOrEmpty(dataInitialization) ? "null" : "data") + ", cancellationToken).ConfigureAwait(false);\n" +
               "}";
        }


        private static string? GetResultKeyFromSchema(OpenApiSchema? schema)
        {
            if (schema is null)
            {
                return null;
            }
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

            return null;
        }


        private static string ToPascalCase(string snakeCase)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(snakeCase.Replace("_", " ")).Replace(" ", string.Empty);
        }

        private static string ToCamelCase(string snakeCase)
        {
            var pascalCase = ToPascalCase(snakeCase);
            return char.ToLower(pascalCase[0]) + pascalCase[1..];
        }

        // Other helper methods remain the same

        private static string ConvertOpenApiTypeToCSharpType(string type, string format)
        {
            return type switch
            {
                "integer" => format switch
                {
                    "int32" => "int",
                    "int64" => "long",
                    "byte" => "byte",
                    "uint16" => "ushort",
                    "uint32" => "uint",
                    "uint64" => "ulong",
                    "bigint" => "BigInteger",
                    _ => "int", // Default to int if format is not recognized
                },
                "string" => "string",
                "number" => "double",
                "boolean" => "bool",
                // Add other type mappings as needed
                _ => "object",// Default to object if type is not recognized
            };
        }


        private static string? GetResponseArrayType(OpenApiSchema? schema)
        {
            if (schema is null)
            {
                return null;
            }

            // If the schema uses "allOf", iterate over the combined schemas
            if (schema.AllOf.Count > 0)
            {
                foreach (var combinedSchema in schema.AllOf)
                {
                    if (combinedSchema.Type == "array")
                    {
                        return combinedSchema.Items.Type;
                    }
                    else if (combinedSchema.Type == "object" && combinedSchema.Properties.Count() == 1)
                    {
                        var property = combinedSchema.Properties.First();
                        if (property.Value.Type == "array")
                        {
                            return property.Value.Items.Type ?? "object";
                        }
                    }
                }
            }
            else if (schema.Type == "array")
            {
                return schema.Items.Type;
            }

            return null;
        }
    }
}
