using Common;
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
			Calculator.Calc("min(-100;1)", null).Should().Be(-100);
			Calculator.Calc("max(-100;1)", null).Should().Be(1);

			var s = "{ \"a\": 1, \"b\": 2, \"c_c\": 3, \"pi\": 4 }";
			Calculator.Calc("-(b+a)*c_c", s).Should().Be(-9);

			s = null;
			Calculator.Calc("2.0 + 7.1", s).Should().Be(9.1);

			Calculator.Calc("sqrt(9) + min(1;2) - max(3;5)", null).Should().Be(-1);
		}
	}
}