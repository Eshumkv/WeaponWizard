using System;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Elements
{
	public class Camera2D
	{
		public float ZoomValue { get; set; }

		public Matrix Transform { get; private set; }

		public Matrix InverseTransform { get; private set; }

		public Vector2 Position { get; set; }

		public float Rotation { get; set; }

		public Camera2D ()
		{
			ZoomValue = 1.0f;
			Rotation = 0.0f;
			Position = Vector2.Zero;
			Update ();
		}

		public void Update ()
		{
			ZoomValue = MathHelper.Clamp (ZoomValue, 0.0f, 10.0f);
			Rotation = ClampAngle (Rotation);
			Transform = Matrix.CreateRotationZ (Rotation) *
			Matrix.CreateScale (new Vector3 (ZoomValue, ZoomValue, 1)) *
			Matrix.CreateTranslation (Position.X, Position.Y, 0);
			InverseTransform = Matrix.Invert (Transform);
		}

		public void Move (Vector2 moveAmount)
		{
			Position += moveAmount;
		}

		public void Rotate (float radians)
		{
			Rotation += radians;
		}

		public void Zoom (float amount)
		{
			ZoomValue += amount;
		}

		protected float ClampAngle (float radians)
		{
			while (radians < -MathHelper.Pi) {
				radians += MathHelper.TwoPi;
			}

			while (radians > MathHelper.Pi) {
				radians -= MathHelper.TwoPi;
			}

			return radians;
		}

		public void CenterCameraOnPoint (GameEngine engine, int x, int y)
		{
			var screen = engine.GetScreenSize ();
			this.Position = new Vector2 (screen.X / 2 - x, screen.Y / 2 - y);
		}
	}
}

