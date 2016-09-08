using System;
using WeaponWizard.Elements.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WeaponWizard.Interfaces;
using WeaponWizard.Elements;
using WeaponWizard.Elements.Components;
using WeaponWizard.Elements.Animation;

namespace WeaponWizard.Screens
{
	public class MainMenuScreen : BaseGameScreen
	{
		private Transition _nextScreen;
		private ImageButton button;

		public MainMenuScreen ()
		{
			BackgroundColor = Color.Red;
		}

		public override void OnEnter ()
		{
			var screen = Engine.GetScreenSize ();
			_nextScreen = null;

			button = new ImageButton (Engine.Textures ["default"], "test", 200, 200, 200, 48);
			button.Middle = new Point (screen.X / 2, screen.Y / 2);
			button.ButtonClicked += (sender) => {
				_nextScreen = new Transition () {
					NextScreen = "overworld"
				};
			};

			var movie = new MovieAnimationComponent (new[] {
				new FrameInfo (new Rectangle (0, 0, 1024, 768), 500),
				new FrameInfo (new Rectangle (1024, 0, 1024, 768), 1000)
			});

			Engine.AddEntity (new Entity ()
				.AddComponent (new TransformComponent (0, 0, screen.X, screen.Y))
				.AddComponent (new RenderableComponent (Engine.Textures ["mainmenu_bg"]) { SourceRect = new Rectangle (0, 0, 1024, 768) })
				.AddComponent (movie), "bg");
		}

		public override Transition Update (GameTime gametime)
		{
			button.Update (Engine, gametime);

			return _nextScreen;
		}

		public override void DrawAfterEntities (SpriteBatch spritebatch)
		{
			spritebatch.Begin ();

			button.Draw (spritebatch);

			spritebatch.End ();
		}
	}
}

