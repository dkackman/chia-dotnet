using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void DeserializeTransacation()
        {
            var file = new FileInfo("transaction.json");
            using var reader = file.OpenText();
            var json = reader.ReadToEnd();

            var transaction = Converters.ToObject<TransactionRecord>(json);

            Assert.IsNotNull(transaction);
        }

        [TestMethod]
        public void SerializeTransacation()
        {
            var file = new FileInfo("transaction.json");
            using var reader = file.OpenText();
            var json = reader.ReadToEnd();

            // if we can go from json -> object -> json -> object 
            // the derialization and desrialziation is doing the correct things in aggregate
            var transaction = Converters.ToObject<TransactionRecord>(json);
            Assert.IsNotNull(transaction);

            string t = transaction.ToJson();
            Assert.IsFalse(string.IsNullOrEmpty(t));

            var transaction2 = Converters.ToObject<TransactionRecord>(t);
            Assert.IsNotNull(transaction2);
        }

        [TestMethod]
        public void DeserializeFullBlock()
        {
            var file = new FileInfo("block.json");
            using var reader = file.OpenText();
            var json = reader.ReadToEnd();

            var block = Converters.ToObject<FullBlock>(json);

            Assert.IsNotNull(block);
        }

        [TestMethod]
        public void DeserializeMempoolItem()
        {
            var file = new FileInfo("mempoolItem.json");
            using var reader = file.OpenText();
            var json = reader.ReadToEnd();

            var item = Converters.ToObject<MempoolItem>(json);

            Assert.IsNotNull(item);
        }

        [TestMethod]
        public void SerializeMempoolItem()
        {
            var file = new FileInfo("mempoolItem.json");
            using var reader = file.OpenText();
            var json = reader.ReadToEnd();

            // if we can go from json -> object -> json -> object 
            // the derialization and desrialziation is doing the correct things in aggregate
            var item = Converters.ToObject<MempoolItem>(json);
            Assert.IsNotNull(item);

            string s = item.ToJson();
            Assert.IsFalse(string.IsNullOrEmpty(s));

            var item2 = Converters.ToObject<MempoolItem>(s);
            Assert.IsNotNull(item2);
        }
    }
}
