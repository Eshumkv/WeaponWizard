using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WeaponWizard.Elements.Components.Animation;
using System.Linq;

namespace WeaponWizard.Elements.Components
{
	public class SpriteAnimationComponent : Component
	{
		public Dictionary<string, List<FrameInfo>> Frames { get; set; }

		public string CurrentState { get; private set; }

		public GameTime LastGameTime { get; set; }

		public int CurrentFrame { get; set; }

		public SpriteAnimationComponent (Dictionary<string, List<FrameInfo>> frames)
		{
			Frames = frames;
			CurrentState = Frames.Keys.First ();
		}

		public void ChangeState (string state)
		{
			if (CurrentState == state)
				return;

			CurrentState = state;
			CurrentFrame = 0;
		}

		public FrameInfo Get ()
		{
			return Frames [CurrentState] [CurrentFrame];
		}
	}
}

