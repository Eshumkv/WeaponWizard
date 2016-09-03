using System;
using Microsoft.Xna.Framework;

namespace WeaponWizard
{
	public static class Helper
	{
		public static double ConvertRange (double value, double oldMin, double oldMax, double newMin, double newMax)
		{
			return (((value - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
		}
	}
}

