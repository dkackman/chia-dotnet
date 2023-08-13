using System.Text;

namespace CodeGen
{
    internal class TestClass
    {
        private readonly string _namespace = string.Empty;
        private readonly string _classUnderTestName = string.Empty;
        private readonly List<TestMethod> _methods = new();

        public TestClass(string ns, string classUnderTestName)
        {
            _namespace = $"{ns}.tests";
            _classUnderTestName = classUnderTestName;
        }

        private const string OpenBracket = "{";
        private const string CloseBracket = "}";

        private void GetHeader(StringBuilder builder)
        {
            _ = builder.Append($"""
                using System;
                using System.Threading.Tasks;
                using System.Collections.Generic;            
                using chia.dotnet.tests.Core;
                using Xunit;

                namespace {_namespace};

                public class {_classUnderTestName}Tests : TestBase
                {OpenBracket}
                    public {_classUnderTestName}Tests(ChiaDotNetFixture fixture) : base(fixture)
                    {OpenBracket}
                    {CloseBracket}

                """
                );
        }

        public void AddMethod(TestMethod testMethod)
        {
            _methods.Add(testMethod);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            GetHeader(builder);
            foreach (var method in _methods)
            {
                _ = builder.Append($"\n{method}\n");
            }
            return builder.Append("\n}").ToString();
        }
    }
}
