using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void CanSerializeToJson()
        {
            var m = new Message();

            var json = JsonSerializer.Serialize(m);

            Assert.IsFalse(string.IsNullOrEmpty(json));
            Assert.IsTrue(json.StartsWith("{"));
            Assert.IsTrue(json.EndsWith("}"));
        }
    }
}
