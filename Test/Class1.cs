using System;
using System.IO;
using System.Text.RegularExpressions;
using Common;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Test
{
	[TestFixture]
	public class Class1
	{
		[Test]
		public void Test1()
		{
			Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory); // чтобы решарпер не продолбал тестовые файлы
			var oldJson = File.ReadAllText(@"TestData\JsonConverter\JsonSamples1\1.v2.json");
			var newJsonExpected = File.ReadAllText(@"TestData\JsonConverter\JsonSamples1\1.v3.json");
			var newJson = JsonV3Converter.Convert(oldJson);
			Compare(newJson, newJsonExpected);
		}

		private void Compare(string stringOne, string expected)
		{
			string fixedStringOne = Regex.Replace(stringOne, @"\s+", String.Empty);
			string fixedStringTwo = Regex.Replace(expected, @"\s+", String.Empty);
			Assert.That(fixedStringOne, Is.EqualTo(fixedStringTwo));
		}
	}
}
