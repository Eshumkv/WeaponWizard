using System;
using Microsoft.Xna.Framework;
using System.Text;
using System.IO;

namespace WeaponWizard
{
	public static class Helper
	{
		public static double ConvertRange (double value, double oldMin, double oldMax, double newMin, double newMax)
		{
			return (((value - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
		}

		public static string ToPath (this string path, bool dir = false)
		{
			var sb = new StringBuilder ();

			foreach (var part in path.Replace ('\\', '/').Split ('/')) {
				sb.Append (part).Append (Path.DirectorySeparatorChar);
			}

			if (!dir) {
				sb.Remove (sb.Length - 1, 1);
			}
			
			return sb.ToString ();
		}
	}
}

