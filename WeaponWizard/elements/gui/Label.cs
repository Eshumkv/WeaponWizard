using System;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Elements.Gui
{
	/// <summary>
	/// A GUI Label.
	/// </summary>
	public class Label : UIItem, IRenderable, IUpdatable
	{
		/// <summary>
		/// Gets or sets a value indicating whether this instance is highlighted.
		/// </summary>
		/// <value><c>true</c> if this instance is highlighted; otherwise, <c>false</c>.</value>
		public bool IsHighlighted { get; set; }

		/// <summary>
		/// Gets or sets the font.
		/// </summary>
		/// <value>The font.</value>
		public SpriteFont Font { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		/// <value>The color.</value>
		public Color Color { get; set; }

		/// <summary>
		/// Gets or sets the color of the highlight.
		/// </summary>
		/// <value>The color of the highlight.</value>
		public Color HighlightColor { get; set; }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public Point Size {
			get {
				return Font.MeasureString (Text).ToPoint ();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WeaponWizard.Elements.Gui.Label"/> class.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="color">Color.</param>
		/// <param name="font">Font.</param>
		public Label (string text, int x, int y, Color color, SpriteFont font = null)
		{
			Text = text;
			Font = font ?? Fonts.Default;
			Color = color;
			HighlightColor = Color;
			IsHighlighted = false;

			var textSize = Size;

			Rect = new Rectangle (x, y, (int)textSize.X, (int)textSize.Y);
		}

		/// <summary>
		/// Draw the specified screen.
		/// </summary>
		/// <param name="spritebatch">Spritebatch.</param>
		public void Draw (SpriteBatch spritebatch)
		{
			spritebatch.DrawString (Font, Text, Position, IsHighlighted ? HighlightColor : Color);
		}

		/// <summary>
		/// Update the specified engine and gametime.
		/// </summary>
		/// <param name="engine">Engine.</param>
		/// <param name="gametime">Gametime.</param>
		public void Update (GameEngine engine, GameTime gametime)
		{
			IsHighlighted = Rect.Contains (engine.Systems.InputSystem.MousePosition ());
		}
	}
}

