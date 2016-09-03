using System;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Elements.Components.Animation
{
	public struct FrameInfo
	{
		public Rectangle SourceRect;
		public TimeSpan Time;

		public FrameInfo (Rectangle srect, double ms)
		{
			Time = TimeSpan.FromMilliseconds (ms);
			SourceRect = srect;
		}
	};
}

