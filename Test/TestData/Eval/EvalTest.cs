using EvalTask;
using FluentAssertions;
using NUnit.Framework;

namespace Test.TestData.Eval
{
	public class EvalTest
	{
		[Test]
		public void Eval()
		{
			var s = "{ \"a\": 1, \"b\": 2, \"c_c\": 3, \"pi\": 4 }";
			EvalProgram.Calc("-(b+a)*c_c", s).Should().Be(-9);
		}
	}
}