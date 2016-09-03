using System;
using WeaponWizard.Elements.Enums;

namespace WeaponWizard.Elements.Components
{
	public class MovementComponent : Component
	{
		public float Speed { get; set; }

		public MovementState State { get; set; }

		public bool CenterCameraOnAction { get; set; }

		public MovementComponent (float speed = 1.0f, MovementState state = MovementState.Idle)
		{
			Speed = speed;
			State = state;
			CenterCameraOnAction = false;
		}
	}
}

