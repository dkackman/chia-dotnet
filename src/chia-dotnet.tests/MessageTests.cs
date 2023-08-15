using System.Text.Json;
using System.Dynamic;
using System.Collections.Generic;
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

        [Fact]
        public void NullDataFieldsAreRemoved()
        {
            dynamic data = new ExpandoObject();
            data.name = "test";
            data.is_null = null;

            var m = Message.Create("command", data, "destination", "orgin");

            var dict = m.Data as IDictionary<string, object>;
            Assert.False(dict!.ContainsKey("is_null"));
        }
    }
}
