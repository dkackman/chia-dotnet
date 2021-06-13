using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text.Json;

namespace chia.dotnet.tests
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void CanSerializeToJson()
        {
            var m = new Message();
            
            string json = JsonSerializer.Serialize(m);

            Assert.IsFalse(string.IsNullOrEmpty(json));
            Assert.IsTrue(json.StartsWith("{"));
            Assert.IsTrue(json.EndsWith("}"));
        }
    }
}
