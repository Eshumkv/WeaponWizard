using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Elements.Components
{
	public class RenderableComponent : Component
	{
		public Texture2D Texture { get; set; }

		public int Z { get; set; }

		public Color Color { get; set; }

		public Rectangle? SourceRect { get; set; }

		public RenderableComponent (Texture2D texture, Rectangle? srect = null, int z = 1)
		{
			Texture = texture;
			Z = z;
			Color = Color.White;
			SourceRect = srect;
		}
	}
}

