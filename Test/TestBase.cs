using System;
using System.IO;

namespace Test
{
	public class TestBase
	{
		public TestBase()
		{
			Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory); // чтобы решарпер не продолбал тестовые файлы
		}
	}
}