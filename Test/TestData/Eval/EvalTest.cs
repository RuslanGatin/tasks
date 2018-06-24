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

			s = null;
			EvalProgram.Calc("2.0 + 7.1", s).Should().Be(9.1);

			EvalProgram.Calc("sqrt(9) + min(1, 2) - max(3, 5)", null).Should().Be(-1);
		}
	}
}