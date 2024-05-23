using System;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        string jsonString = @"{
            ""name"": ""Иван"",
            ""age"": 30,
            ""city"": ""Казань"",
            ""children"": [
                { ""name"": ""Саша"", ""age"": 10 },
                { ""name"": ""Маша"", ""age"": 8 }
            ]
        }";

        using (JsonDocument doc = JsonDocument.Parse(jsonString))
        {
            XElement xml = JsonToXml(doc.RootElement, "root");
            Console.WriteLine(xml);
        }
    }

    static XElement JsonToXml(JsonElement jsonElement, string elementName)
    {
        switch (jsonElement.ValueKind)
        {
            case JsonValueKind.Object:
                var objElement = new XElement(elementName);
                foreach (JsonProperty property in jsonElement.EnumerateObject())
                {
                    objElement.Add(JsonToXml(property.Value, property.Name));
                }
                return objElement;

            case JsonValueKind.Array:
                var arrayElement = new XElement(elementName);
                foreach (JsonElement item in jsonElement.EnumerateArray())
                {
                    arrayElement.Add(JsonToXml(item, "item"));
                }
                return arrayElement;

            case JsonValueKind.String:
                return new XElement(elementName, jsonElement.GetString());

            case JsonValueKind.Number:
                return new XElement(elementName, jsonElement.GetDecimal());

            case JsonValueKind.True:
                return new XElement(elementName, true);

            case JsonValueKind.False:
                return new XElement(elementName, false);

            case JsonValueKind.Null:
                return new XElement(elementName, null);

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
