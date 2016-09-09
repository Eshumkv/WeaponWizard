using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WeaponWizard
{
	public class Sprite
	{
		public Texture2D Texture { get; set; }

		public Rectangle SourceRect { get; set; }

		public Sprite (Texture2D texture, Rectangle source)
		{
			Texture = texture;
			SourceRect = source;
		}

		public Sprite (Texture2D texture, int x, int y, int w, int h) : this (texture, new Rectangle (x, y, w, h))
		{
			// No extra steps
		}
	}
}

