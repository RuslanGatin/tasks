using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common
{
	public class OldJson
	{
		public string version;
		public Dictionary<string, OldProd> products;
	}

	public class OldProd
	{
		public string name;
		public string proce;
		public string count;
	}

	public class NewJson
	{
		public string version;
		public NewProd[] products;
	}

	public class NewProd
	{
		public long id;
		public string name;
		public Decimal price;
		public long count;
	}

	public static class JsonV3Converter
	{
		public static string Convert(string json)
		{
			JObject v2 = JObject.Parse(json);
			var products = v2["products"];
			var newJson = new NewJson();
			var res = new List<NewProd>();
			foreach (var product in products)
			{
				var p = new NewProd();
				p.id = long.Parse(product.Path.Substring("products.".Length));
				foreach (var x in product)
				{
					p.count = (long) x["count"];
					p.name = (string) x["name"];
					p.price = Decimal.Parse((string) x["price"], CultureInfo.InvariantCulture);
				}

				res.Add(p);
			}

			newJson.version = "3";
			newJson.products = res.ToArray();
			var serializer = new JsonSerializer();
			serializer.Converters.Add(new DecimalJsonConverter());

			var stringBuilder = new StringBuilder();
			serializer.Serialize(new StringWriter(stringBuilder), newJson);
			return stringBuilder.ToString();
		}
	}

	class DecimalJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(decimal);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteRawValue(((decimal)value).ToString(CultureInfo.InvariantCulture));
		}
	}
}