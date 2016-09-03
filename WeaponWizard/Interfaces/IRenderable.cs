using System;
using Microsoft.Xna.Framework.Graphics;

namespace WeaponWizard.Interfaces
{
	/// <summary>
	/// Interface that declares something can be drawn
	/// </summary>
	public interface IRenderable
	{
		/// <summary>
		/// Draw the specified screen.
		/// </summary>
		/// <param name="spritebatch">Spritebatch.</param>
		void Draw (SpriteBatch spritebatch);
	}
}

