using Newtonsoft.Json.Linq;
using System;
using Common;

namespace JsonConversion
{
	class JsonProgram
	{
		static void Main()
		{
			string json = Console.In.ReadToEnd();
			Console.Write(JsonV3Converter.Convert(json));
		}
	}
}
