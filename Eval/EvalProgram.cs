using System;
using System.Globalization;
using System.Text;
using Common;

namespace EvalTask
{
	public class EvalProgram
	{
		static void Main(string[] args)
		{
			string input = Console.In.ReadToEnd();
			var sb = new StringBuilder();
			string exp = input, json = null;
			for (int i = 0; i < input.Length; i++)
			{
				if ((input[i] == '\n'))
				{
					exp = input.Substring(0, i).Trim();
					json = input.Substring(i).Trim();
					break;
				}
			}

			string s;
			try
			{
				s = Calculator.Calc(exp, json).ToString(CultureInfo.InvariantCulture);

			}
			catch (Exception e)
			{
				s = "error";
			}

			string output = s;
			Console.WriteLine(output);
		}
	}
}
