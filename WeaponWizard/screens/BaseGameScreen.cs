using System;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WeaponWizard.Elements;

namespace WeaponWizard.Screens
{
	/// <summary>
	/// Base game screen.
	/// </summary>
	public abstract class BaseGameScreen : IScreen
	{
		/// <summary>
		/// Gets or sets the engine.
		/// </summary>
		/// <value>The engine.</value>
		public static GameEngine StaticEngine { get; set; }

		/// <summary>
		/// Gets or sets the engine.
		/// </summary>
		/// <value>The engine.</value>
		public GameEngine Engine { 
			get { 
				return StaticEngine;
			}set {
				StaticEngine = value;
			}
		}

		/// <summary>
		/// Gets or sets the color of the background.
		/// </summary>
		/// <value>The color of the background.</value>
		public Color BackgroundColor { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="WeaponWizard.Screens.BaseGameScreen"/> class.
		/// </summary>
		public BaseGameScreen ()
		{
			BackgroundColor = Color.CornflowerBlue;
		}

		/// <summary>
		/// Loads the content.
		/// </summary>
		/// <param name="engine">Engine.</param>
		public virtual void LoadContent ()
		{
		}

		/// <summary>
		/// Code to unload assets.
		/// </summary>
		public virtual void OnExit ()
		{
			Engine.ClearEntities ();
			Engine.Camera.Position = Vector2.Zero;
		}

		/// <summary>
		/// Code to load assets.
		/// </summary>
		/// <param name="engine">Engine.</param>
		public abstract void OnEnter ();

		/// <summary>
		/// Update this screen instance.
		/// </summary>
		/// <param name="gametime">Gametime.</param>
		public abstract Transition Update (GameTime gametime);

		/// <summary>
		/// Draw the specified screen.
		/// </summary>
		/// <param name="spritebatch">Spritebatch.</param>
		public virtual void Draw (SpriteBatch spritebatch)
		{
		}

		public virtual void DrawAfterEntities (SpriteBatch spritebatch)
		{
		}
	}
}

