using System.Text;
using System.Reflection;

namespace CodeGen
{
    internal class TestMethod
    {
        private readonly MethodInfo _method;
        private readonly string _testBaseMemberName;

        public TestMethod(MethodInfo method, string testBaseMemberName)
        {
            _method = method;
            _testBaseMemberName = testBaseMemberName;
        }

        private const string OpenBracket = "{";
        private const string CloseBracket = "}";

        private bool HasReturn()
        {
            return _method.ReturnType is not null && _method.ReturnType.IsGenericType;
        }

        private void Method(StringBuilder builder)
        {
            _ = builder.Append($"""
                    [Fact(Skip = "Requires review")]
                    public async Task {_method.Name}()
                    {OpenBracket}
                """
                );
        }

        private void Arrange(StringBuilder builder)
        {
            var paramInitializationClause = GetParamInitialization();
            _ = builder.Append($"""

                        // Arrange
                        using var cts = new CancellationTokenSource(15000);
                        {paramInitializationClause}
                """
                );
        }

        private void Act(StringBuilder builder)
        {
            var returnClause = HasReturn() ? "var returnValue = await " : "await ";
            var paramClause = getParamsAsArguments();

            _ = builder.Append($"""

                        // Act
                        {returnClause}{_testBaseMemberName}.{_method.Name}({paramClause});
                        
                """
                );
        }

        private void Assert(StringBuilder builder)
        {
            var assertClause = HasReturn() ? "Assert.NotNull(returnValue);" : "";
            _ = builder.Append($"""

                        // Assert
                        {assertClause}
                """
                );
        }

        private IEnumerable<ParameterInfo> GetParams()
        {
            return _method.GetParameters()
                .Where(p => p.ParameterType.Name != "CancellationToken" && !p.IsOptional);
        }

        private string GetParamInitialization()
        {
            var builder = new StringBuilder();
            var leadingSpaces = "";
            foreach (var param in GetParams())
            {
                _ = builder.Append(leadingSpaces)
                    .Append(param.ParameterType.Name)
                    .Append(" ")
                    .Append(param.Name)
                    .Append(" = ")
                    .Append(GetDefaultValue(param.ParameterType))
                    .Append(";\n");

                leadingSpaces = "        ";
            }

            return builder.ToString();
        }

        public static string GetDefaultValue(Type type)
        {
            if (type == null)
            {
                return "null";
            }

            if (type == typeof(DateTime))
            {
                return "DateTime.Now";
            }

#pragma warning disable IDE0046 // Convert to conditional expression
            if (type == typeof(string))
            {
                return "string.Empty";
            }
#pragma warning restore IDE0046 // Convert to conditional expression


            return type.IsValueType ? Activator.CreateInstance(type)?.ToString() ?? "null" : "null";
        }

        private string getParamsAsArguments()
        {
            var builder = new StringBuilder();

            var paramList = GetParams();
            if (paramList.Any())
            {
                foreach (var param in paramList)
                {
                    _ = builder.Append(param.Name)
                        .Append(": ")
                        .Append(param.Name)
                        .Append(", ");
                }
            }

            _ = builder.Append("cancellationToken: cts.Token");
            return builder.ToString();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            Method(builder);
            Arrange(builder);
            Act(builder);
            Assert(builder);

            return builder.Append($"\n    {CloseBracket}").ToString();
        }
    }
}
