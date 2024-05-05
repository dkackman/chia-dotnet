using System;
using System.Numerics;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Linq;

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
                Debug.Assert(item.Count() >= 3);
                list.Add(new Coin
                {
                    ParentCoinInfo = item[0]!.ToString(),
                    Amount = BigInteger.Parse(item[1]!.ToString()),
                    PuzzleHash = item[2]!.ToString()
                });
            }
            return list;

        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not null)
            {
                writer.WriteStartArray();
                foreach (var coin in (IEnumerable<Coin>)value)
                {
                    writer.WriteStartArray();
                    writer.WriteValue(coin.ParentCoinInfo);
                    writer.WriteValue(coin.Amount);
                    writer.WriteValue(coin.PuzzleHash);
                    writer.WriteEndArray();

                }
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteNull();
            }
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
                Debug.Assert(item.Count() >= 2);

                list.Add(new NameValuePair
                {
                    Name = item[0]!.ToString(),
                    Value = item[1]!.ToString()
                });
            }
            return list;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not null)
            {
                writer.WriteStartArray();
                foreach (var nvp in (IEnumerable<NameValuePair>)value)
                {
                    writer.WriteStartArray();
                    writer.WriteValue(nvp.Name);
                    writer.WriteValue(nvp.Value);
                    writer.WriteEndArray();

                }
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteNull();
            }
        }
    }
}
