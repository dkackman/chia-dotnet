using System.Text.Json;
using Xunit;

namespace chia.dotnet.tests
{
    public class MessageTests
    {
        [Fact]
        public void CanSerializeToJson()
        {
            var m = new Message();

            var json = JsonSerializer.Serialize(m);

            Assert.False(string.IsNullOrEmpty(json));
            Assert.StartsWith("{", json);
            Assert.EndsWith("}", json);
        }
    }
}
