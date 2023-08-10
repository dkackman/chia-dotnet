using System.Reflection;

namespace CodeGen
{
    internal class TestStub
    {
        public static void GenerateTestStub(string assemblyFilePath, string classUnderTestName)
        { 
            var assembly = Assembly.LoadFile(assemblyFilePath);
            var classUnderTest = assembly.GetType(classUnderTestName);

        }
    }
}
