using System;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace PythonTest
{
	public class Duck
	{
		public string Color { get; set; }

		public int Size { get; set; }

		public Duck ()
		{
			Color = "Yellow";
			Size = 20;
		}

		public string Quack ()
		{
			return "QUACK!!";
		}

		private string Quack2 ()
		{
			return "No Quack :(";
		}
	}

	class MainClass
	{
		private static ScriptEngine _engine = Python.CreateEngine ();
		private static ScriptScope _scope = null;

		public static void Main (string[] args)
		{
			PythonFromString ();
			PythonFromFile ();
		}

		public static void PythonFromString ()
		{
			var duck = new Duck ();
			_scope = _engine.CreateScope ();
			_scope.SetVariable ("duck", duck);

			var source = _engine.CreateScriptSourceFromString ("duck.Color = duck.Quack()", SourceCodeKind.SingleStatement);
			source.Execute (_scope);

			Console.WriteLine (duck.Color);
		}

		public static void PythonFromFile ()
		{
			var duck = new Duck ();
			_scope = _engine.CreateScope ();
			_scope.SetVariable ("duck", duck);

			var source = _engine.CreateScriptSourceFromFile ("test.py");
			source.Execute (_scope);
			_engine.Runtime.Shutdown ();

			Console.WriteLine (duck.Size);
		}
	}
}
