using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WeaponWizard.Elements;

namespace WeaponWizard.Interfaces
{
	/// <summary>
	/// The interface that describes what a game screen should look like. 
	/// </summary>
	public interface IScreen
	{
		/// <summary>
		/// Gets or sets the color of the background.
		/// </summary>
		/// <value>The color of the background.</value>
		Color BackgroundColor { get; set; }

		/// <summary>
		/// Gets or sets the engine.
		/// </summary>
		/// <value>The engine.</value>
		GameEngine Engine { get; set; }

		/// <summary>
		/// Update the specified screen.
		/// </summary>
		/// <param name="gametime">Gametime.</param>
		Transition Update (GameTime gametime);

		/// <summary>
		/// Code to run when entering the screen.
		/// </summary>
		void OnEnter ();

		/// <summary>
		/// Code to run when exiting the screen.
		/// </summary>
		void OnExit ();

		/// <summary>
		/// Draw the specified screen.
		/// </summary>
		/// <param name="spritebatch">Spritebatch.</param>
		void Draw (SpriteBatch spritebatch);

		/// <summary>
		/// Draws the after entities.
		/// </summary>
		/// <param name="spritebatch">Spritebatch.</param>
		void DrawAfterEntities (SpriteBatch spritebatch);
	}
}

