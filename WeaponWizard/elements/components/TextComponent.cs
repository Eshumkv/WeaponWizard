using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Elements.Components
{
	public class TextComponent : Component
	{
		public string Text { get; set; }

		public SpriteFont Font { get; set; }

		public Vector2 Location { get; set; }

		public Color Color { get; set; }

		public TextComponent (string text, Color color, Vector2 offset = default(Vector2), SpriteFont font = null)
		{
			Text = text;
			Color = color;
			Location = offset;

			if (font == null)
				Font = Fonts.Default;
		}
	}
}

