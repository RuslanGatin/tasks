using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SimQLTask;

namespace Test
{
	public class SimQlTests : TestBase
	{
		[Test]
		public void GetQueryTest()
		{
			var JSON = File.ReadAllText(@"TestData\SimQl\Data.txt");
			var executeQueries = SimQLProgram.ExecuteQueries(JSON).Should().Equal("a.b.c = 15", "z = 42", "a.x = 3.14");
		}
	}
}