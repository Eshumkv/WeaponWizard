using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WeaponWizard.Elements.Animation;

namespace WeaponWizard.Elements.Components
{
	public class MovieAnimationComponent : Component
	{
		public List<FrameInfo> Frames { get; set; }

		public GameTime LastGameTime { get; set; }

		public int CurrentFrame { get; set; }

		public MovieAnimationComponent (IList<FrameInfo> frameInfo)
		{
			Frames = new List<FrameInfo> (frameInfo);
			CurrentFrame = 0;
			LastGameTime = null;
		}
	}
}

