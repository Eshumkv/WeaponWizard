using System;
using Microsoft.Xna.Framework;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace WeaponWizard.Elements.Gui
{
	/// <summary>
	/// Part of the imagebutton.
	/// </summary>
	public class ImageButtonPart : IRenderable
	{
		/// <summary>
		/// The original source rect.
		/// </summary>
		private readonly Rectangle _originalSourceRect;

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		/// <value>The texture.</value>
		public Texture2D Texture { get; set; }

		/// <summary>
		/// Gets or sets the source rect.
		/// </summary>
		/// <value>The source rect.</value>
		public Rectangle SourceRect { get; set; }

		/// <summary>
		/// Gets or sets the destination rect.
		/// </summary>
		/// <value>The destination rect.</value>
		public Rectangle DestinationRect { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="WeaponWizard.Elements.Gui.ImageButtonPart"/> is wrapped.
		/// </summary>
		/// <value><c>true</c> if wrap; otherwise, <c>false</c>.</value>
		public bool Wrap { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="WeaponWizard.Elements.Gui.ImageButtonPart"/> class.
		/// </summary>
		/// <param name="texture">Texture.</param>
		/// <param name="destRect">Destination rect.</param>
		/// <param name="sourceRect">Source rect.</param>
		/// <param name="wrap">If set to <c>true</c> wrap.</param>
		public ImageButtonPart (Texture2D texture, Rectangle destRect, Rectangle sourceRect, bool wrap = false)
		{
			Texture = texture;
			SourceRect = sourceRect;
			DestinationRect = destRect;
			Wrap = wrap;
			_originalSourceRect = SourceRect;
		}

		/// <summary>
		/// Draw the specified part.
		/// </summary>
		/// <param name="spritebatch">Spritebatch.</param>
		public void Draw (SpriteBatch spritebatch)
		{
			if (Wrap) {
				spritebatch.End ();
				spritebatch.Begin (SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
			}

			spritebatch.Draw (Texture, 
				destinationRectangle: DestinationRect, 
				sourceRectangle: SourceRect);
		}

		/// <summary>
		/// Determines whether this instance is highlighted.
		/// </summary>
		/// <returns><c>true</c> if this instance is highlighted the specified value; otherwise, <c>false</c>.</returns>
		/// <param name="value">If set to <c>true</c> value.</param>
		public void IsHighlighted (bool value)
		{
			var offset = (int)Texture.Width / 2;
			SourceRect = new Rectangle (_originalSourceRect.X + (value ? offset : 0), _originalSourceRect.Y, _originalSourceRect.Width, _originalSourceRect.Height);
		}
	}
}

