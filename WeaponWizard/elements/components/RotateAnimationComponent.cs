using System;
using Microsoft.Xna.Framework;
using WeaponWizard.Elements.Enums;

namespace WeaponWizard.Elements.Components
{
	public class RotateAnimationComponent : Component
	{
		public float Speed { get; set; }

		public RotateAnimationComponent (float speed)
		{
			Speed = speed;
		}
	}
}

