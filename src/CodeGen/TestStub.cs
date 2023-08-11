using System.Reflection;

namespace CodeGen
{
    internal static class TestStub
    {
        public static void GenerateTestStub(string assemblyFilePath, string classUnderTestName, string testBaseMemberName)
        {
            var assembly = Assembly.LoadFile(assemblyFilePath);
            var classUnderTest = assembly.GetType(classUnderTestName) ?? throw new ArgumentException($"{classUnderTestName} not found");

            var testClass = new TestClass(classUnderTest.Namespace!, classUnderTest.Name);

            var methods = classUnderTest.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_"));
            foreach (var method in methods)
            {
                testClass.AddMethod(new TestMethod(method, testBaseMemberName));
            }

            Console.WriteLine(testClass.ToString());
        }
    }
}
