using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimQLTask
{
	public class SimQLProgram
	{
		static void Main(string[] args)
		{
			var json = Console.In.ReadToEnd();
			foreach (var result in ExecuteQueries(json))
				Console.WriteLine(result);
		}

		public static IEnumerable<string> ExecuteQueries(string json)
		{
			var jObject = JObject.Parse(json);
			var data = (JObject)jObject["data"];
			var queries = jObject["queries"].ToObject<string[]>();
			return queries.Select(q => ProcessQuery(data, q));
		}

		private static string ProcessQuery(JObject data, string query)
		{
			var z = data.SelectToken(query);
			var s = "";
			try
			{
				s = (string) z;
			}
			catch (Exception e)
			{
				s = null;
			}

			return $"{query} = {s}";
		}
	}
}
