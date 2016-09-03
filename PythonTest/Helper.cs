using System;
using System.Text;
using System.IO;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;

namespace WeaponWizard
{
	public static class Extension
	{
		public static string Path (this string path)
		{
			return Helper.GetRelativePath (path);
		}
	}

	public static class Helper
	{
		public static string GetRelativePath (string path)
		{
			var split = path.Split ('/');

			if (split.Length == 1)
				return split [0];

			StringBuilder sb = new StringBuilder (split [0]);

			for (var i = 1; i < split.Length; i++) {
				if (string.IsNullOrWhiteSpace (split [i]))
					continue;

				sb.Append (Path.DirectorySeparatorChar).Append (split [i]);
			}

			return sb.ToString ();
		}

		//public static IScreen GetPythonScreen (string path, string name)
		public static object GetPythonScreen (string path, string name)
		{
			var engine = GetPythonEngine (path);
			var scope = engine.CreateScope ();

			// Load the necessary .NET modules
			var source = engine.CreateScriptSourceFromFile ("scripts" + Path.DirectorySeparatorChar + "load.py", Encoding.UTF8, Microsoft.Scripting.SourceCodeKind.File);
			source.Execute (scope);

			// Actually load the script
			source = engine.CreateScriptSourceFromFile (path.Path (), Encoding.UTF8, Microsoft.Scripting.SourceCodeKind.File);
			source.Execute (scope);

			//dynamic screen = scope.GetVariable (name) ();
			object screen = null;

			return screen;
		}

		public static dynamic RunPythonMain (string path)
		{
			var engine = GetPythonEngine (path);
			var scope = engine.CreateScope ();
			
			var source = engine.CreateScriptSourceFromFile ("scripts" + Path.DirectorySeparatorChar + "load.py", Encoding.UTF8, Microsoft.Scripting.SourceCodeKind.File);
			source.Execute (scope);
			source = engine.CreateScriptSourceFromFile (path.Path (), Encoding.UTF8, Microsoft.Scripting.SourceCodeKind.File);
			source.Execute (scope);

			return scope.GetVariable ("main");
		}

		public static ScriptEngine GetPythonEngine (string scriptPath)
		{
			var engine = Python.CreateEngine ();
			var paths = engine.GetSearchPaths ();
			var dir = Path.GetDirectoryName (scriptPath);

			if (string.IsNullOrWhiteSpace (dir)) {
				paths.Add (Environment.CurrentDirectory);
			} else {
				paths.Add (dir);
			}

			engine.SetSearchPaths (paths);

			return engine;
		}
	}
}

