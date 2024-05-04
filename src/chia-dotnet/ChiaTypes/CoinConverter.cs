using System;
using System.Numerics;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace chia.dotnet
{
    internal class CoinConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Coin);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);
            var list = new List<Coin>();
            foreach (var item in array)
            {
                list.Add(new Coin
                {
                    ParentCoinInfo = item[0].ToString(),
                    Amount = BigInteger.Parse(item[1].ToString()),
                    PuzzleHash = item[2].ToString()
                });
            }
            return list;

        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (var coin in value as IEnumerable<Coin>)
            {
                writer.WriteStartArray();
                writer.WriteValue(coin.ParentCoinInfo);
                writer.WriteValue(coin.Amount);
                writer.WriteValue(coin.PuzzleHash);
                writer.WriteEndArray();

            }
            writer.WriteEndArray();
        }
    }

    public record NameValuePair
    {
        public string Name { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
    }

    internal class NameValuePairConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(NameValuePair);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);
            var list = new List<NameValuePair>();
            foreach (var item in array)
            {
                list.Add(new NameValuePair
                {
                    Name = item[0].ToString(),
                    Value = item[1].ToString()
                });
            }
            return list;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (var nvp in value as IEnumerable<NameValuePair>)
            {
                writer.WriteStartArray();
                writer.WriteValue(nvp.Name);
                writer.WriteValue(nvp.Value);
                writer.WriteEndArray();

            }
            writer.WriteEndArray();
        }
    }
}
