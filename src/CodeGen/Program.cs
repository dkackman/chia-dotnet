namespace CodeGen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var command = args.Length > 0 ? args[0] : null;
            if (command == "impl")
            {
                var swaggerFilePath = args[1];
                var endpointName = args[2];
                var returnType = args.Length > 3 ? args[3] : null;

                StubImplementation.GenerateImplementationStub(swaggerFilePath, endpointName, returnType);
            }
            else if (command == "test")
            {
                var assemblyFilePath = args[1];
                var classUnderTestName = args[2];
                var testBaseMemberName = args[3];

                StubTestClass.GenerateTestStub(assemblyFilePath, classUnderTestName, testBaseMemberName);
            }
            else
            {
                Console.WriteLine("Usage: CodeGen command [impl | test]");
                Console.WriteLine("impl - <path_to_yaml> <endpoint_name> [return_type_name]");
                Console.WriteLine("test - <path_to_assembly> <class_under_test>");
            }
        }
    }
}
