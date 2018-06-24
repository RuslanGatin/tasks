using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

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
				s = Calc(exp, json).ToString(CultureInfo.InvariantCulture);

			}
			catch (Exception e)
			{
				s = "error";
			}

			string output = s;
			Console.WriteLine(output);
		}

		public static double Calc(string expr, string map)
		{
			Dictionary<string, double> dict = new Dictionary<string, double>();
			if (!string.IsNullOrEmpty(map))
				dict = ParseJson(map);
			return calc(expr, dict);
		}

		public static double GetNum(Dictionary<string, double> dict, string token)
		{
			return dict.ContainsKey(token)
				? dict[token]
				: double.Parse(token.Replace(',', '.').Replace("'", ""), CultureInfo.InvariantCulture);
		}

		public static Dictionary<string, double> ParseJson(string json)
		{
			var jObject = JObject.Parse(json);
			var ret = new Dictionary<string, double>();
			foreach (var obj in jObject)
			{
				ret[obj.Key] = double.Parse((string)obj.Value);
			}

			return ret;
		}

		public static List<string> GetTokens(string s)
		{
			List<string> res = new List<string>();
			StringBuilder sb = new StringBuilder();
			foreach (var ch in s)
			{
				var item = sb.ToString();
				if (Char.IsWhiteSpace(ch) || ch == ';')
				{
					if (!string.IsNullOrEmpty(item))
						res.Add(item);
					sb.Clear();
					continue;
				}

				if (IsOp(ch))
				{
					if (!string.IsNullOrEmpty(item))
						res.Add(item);
					sb.Clear();
					res.Add(ch.ToString());
					continue;
				}

				sb.Append(ch);
			}

			var q = sb.ToString();
			if (!string.IsNullOrEmpty(q))
				res.Add(q);
			return res;
		}

		public static bool IsOp(char c)
		{
			return c == '(' || c == ')' || c == '/' || c == '*' || c == '+' || c == '-' || is_op(c);
		}

		static bool is_op(char c)
		{
			return c == '+' || c == '-' || c == '*' || c == '/' || c == '%';
		}

		static int priority(int op)
		{
			if (op < 0)
				return 4;
			return
				op == '+' || op == '-' ? 1 :
				op == '*' || op == '/' || op == '%' ? 2 :
				-1;
		}

		static void process_op(List<double> st, int op)
		{
			if (op < 0)
			{
				double l = st.Last();
				st.RemoveAt(st.Count - 1);
				switch (-op)
				{
					case '+':
						st.Add(l);
						break;
					case '-':
						st.Add(-l);
						break;
				}
			}
			else
			{

				double r = st.Last();
				st.RemoveAt(st.Count - 1);
				double l = st.Last();
				st.RemoveAt(st.Count - 1);
				switch (op)
				{
					case '+':
						st.Add(l + r);
						break;
					case '-':
						st.Add(l - r);
						break;
					case '*':
						st.Add(l * r);
						break;
					case '/':
						if (r == 0)
							throw new Exception();
						st.Add(l / r);
						break;
					case '%':
						st.Add(l % r);
						break;
				}
			}

		}

		static List<string> preprocess(List<string> tokens)
		{
			List<string> ret = new List<string>();
			for (int i = 0; i < tokens.Count; i++)
			{
				var token = tokens[i];
				if (token == "sqrt")
				{
					double op = double.Parse(tokens[i + 2]);
					op = Math.Sqrt(op);
					ret.Add(op.ToString(CultureInfo.InvariantCulture));
					i += 3;
				}
				else if (token == "max")
				{
					double op1 = double.Parse(tokens[i + 2].Trim(','));
					double op2 = double.Parse(tokens[i + 3].Trim(','));
					ret.Add(Math.Max(op1, op2).ToString(CultureInfo.InvariantCulture));
					i += 4;
				}
				else if (token == "min")
				{
					double op1 = double.Parse(tokens[i + 2].Trim(','));
					double op2 = double.Parse(tokens[i + 3].Trim(','));
					ret.Add(Math.Min(op1, op2).ToString(CultureInfo.InvariantCulture));
					i += 4;
				}
				else
				{
					ret.Add(token);
				}
			}

			return ret;
		}


		static double calc(string inp, Dictionary<string, double> dict)
		{
			bool may_unary = true;
			List<double> st = new List<double>();
			List<int> op = new List<int>();
			var tokens = GetTokens(inp);
			tokens = preprocess(tokens);
			foreach (var token in tokens)
			{
				if (token == "(")
				{
					op.Add('(');
					may_unary = true;
				}
				else if (token == ")")
				{
					while (op.Last() != '(')
					{
						process_op(st, op.Last());
						op.RemoveAt(op.Count - 1);
					}

					op.RemoveAt(op.Count - 1);
					may_unary = false;
				}
				else if (IsOp(token[0]))
				{
					int curop = token[0];
					while (may_unary && curop == '-')
						curop = (-curop);
					while (op.Any() && 
						   (curop >= 0 && priority(op.Last()) >= priority(curop)
						    || curop < 0 && priority(op.Last()) > priority(curop)))
					{
						process_op(st, op.Last());
						op.RemoveAt(op.Count - 1);
					}

					op.Add(curop);
					may_unary = true;
				}
				else
				{
					st.Add(GetNum(dict, token));
					may_unary = false;
				}
			}

			while (op.Any())
			{
				process_op(st, op.Last());
				op.RemoveAt(op.Count - 1);
			}

			if (st.Count != 1)
				throw new Exception();
			return st.Last();
		}
	}
}
