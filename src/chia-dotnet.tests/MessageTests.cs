using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Text.Json;

namespace chia.dotnet.tests
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void NewMessageHasId()
        {
            var m = new Message();
            Assert.IsFalse(string.IsNullOrEmpty(m.Request_Id));
        }
            
        [TestMethod]
        public void GeneratedIdsAreUnique()
        {
            var m1 = new Message();
            var m2 = new Message();
            Assert.IsFalse(m1.Request_Id == m2.Request_Id);
        }

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
