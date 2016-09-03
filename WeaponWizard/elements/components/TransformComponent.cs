using System;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Elements.Components
{
	public class TransformComponent : Component
	{
		public float X { get; set; }

		public float Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public float Rotation { get; set; }

		public Vector2 Scale { get; set; }

		public Vector2 Origin { get; set; }

		public Point Position {
			get {
				return AccuratePosition.ToPoint ();
			} 
		}

		public Vector2 AccuratePosition {
			get {
				return new Vector2 (X, Y);
			}
			set {
				X = value.X;
				Y = value.Y;
			}
		}

		public TransformComponent (int x, int y, int w, int h)
		{
			X = x;
			Y = y;
			Width = w;
			Height = h;
			Rotation = 0;
			Scale = Vector2.One;
			Origin = Vector2.Zero;
		}
	}
}

