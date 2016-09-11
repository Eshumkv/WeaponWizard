using System;
using Microsoft.Xna.Framework;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace WeaponWizard
{
	public static class Helper
	{
		public static double ConvertRange (double value, double oldMin, double oldMax, double newMin, double newMax)
		{
			return (((value - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
		}

		public static string ToPath (this string path)
		{
			var sb = new StringBuilder ();

			foreach (var part in path.Replace ('\\', '/').Split ('/')) {
				sb.Append (part).Append (Path.DirectorySeparatorChar);
			}

			try {
				var attributes = File.GetAttributes (path);

				// If its not a directory, remove the last slash if it's there
				if (!attributes.HasFlag (FileAttributes.Directory)) {
					if (sb.ToString ().EndsWith ("/")) {
						sb.Remove (sb.Length - 1, 1);
					}
				}
			} catch (Exception) {
			}
			
			return sb.ToString ();
		}

		/// <summary>
		/// Gets all files including the ones in a subdirectory.
		/// </summary>
		/// <returns>The all files.</returns>
		/// <param name="path">Path.</param>
		public static List<string> GetAllFiles (string path)
		{
			var files = new List<string> ();

			// Just to be safe
			path = path.ToPath ();

			// Go through all the files in the main dir
			foreach (var file in Directory.GetFiles(path)) {
				files.Add (file);
			}

			// Go through all the sub directories
			foreach (var dir in Directory.GetDirectories(path)) {
				foreach (var file in Directory.GetFiles(dir)) {
					files.Add (file);
				}
			}

			return files;
		}

		public static void ShowMsgBox (string message)
		{
			Console.WriteLine (message);
		}
	}
}

