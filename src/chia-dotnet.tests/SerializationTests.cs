using System.IO;
using Xunit;

namespace chia.dotnet.tests
{
    public class SerializationTests
    {
        [Fact]
        public void DeserializeTransacation()
        {
            var file = new FileInfo("transaction.json");
            using var reader = file.OpenText();
            var json = reader.ReadToEnd();

            var transaction = Converters.ToObject<TransactionRecord>(json);

            Assert.NotNull(transaction);
        }

        [Fact]
        public void SerializeTransacation()
        {
            var file = new FileInfo("transaction.json");
            using var reader = file.OpenText();
            var json = reader.ReadToEnd();

            // if we can go from json -> object -> json -> object 
            // the derialization and desrialziation is doing the correct things in aggregate
            var transaction = Converters.ToObject<TransactionRecord>(json);
            Assert.NotNull(transaction);

            var t = transaction.ToJson();
            Assert.False(string.IsNullOrEmpty(t));

            var transaction2 = Converters.ToObject<TransactionRecord>(t);
            Assert.NotNull(transaction2);
        }

        [Fact]
        public void DeserializeFullBlock()
        {
            var file = new FileInfo("block.json");
            using var reader = file.OpenText();
            var json = reader.ReadToEnd();

            var block = Converters.ToObject<FullBlock>(json);

            Assert.NotNull(block);
        }

        [Fact]
        public void DeserializeMempoolItem()
        {
            var file = new FileInfo("mempoolItem.json");
            using var reader = file.OpenText();
            var json = reader.ReadToEnd();

            var item = Converters.ToObject<MempoolItem>(json);

            Assert.NotNull(item);
        }

        [Fact]
        public void SerializeMempoolItem()
        {
            var file = new FileInfo("mempoolItem.json");
            using var reader = file.OpenText();
            var json = reader.ReadToEnd();

            // if we can go from json -> object -> json -> object 
            // the derialization and desrialziation is doing the correct things in aggregate
            var item = Converters.ToObject<MempoolItem>(json);
            Assert.NotNull(item);

            var s = item.ToJson();
            Assert.False(string.IsNullOrEmpty(s));

            var item2 = Converters.ToObject<MempoolItem>(s);
            Assert.NotNull(item2);
        }
    }
}
