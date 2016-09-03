using System;
using Microsoft.Xna.Framework;
using WeaponWizard.Elements.Enums;

namespace WeaponWizard.Elements.Components
{
	public class SlideInAnimationComponent : Component
	{
		public bool HasAnimationStarted { get; set; }

		public Vector2 StartPosition { get; set; }

		public Vector2 Destination { get; set; }

		public float Delta { get; set; }

		public float TimeToReach { get; set; }

		public Side SlideInSide { get; set; }

		public SlideInAnimationComponent (float ms, Side s = Side.Down)
		{
			TimeToReach = ms;
			Delta = 0;
			HasAnimationStarted = false;
			StartPosition = Vector2.Zero;
			Destination = Vector2.Zero;
			SlideInSide = s;
		}
	}
}

