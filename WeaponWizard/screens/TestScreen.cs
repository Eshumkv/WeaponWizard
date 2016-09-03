using System;
using WeaponWizard.Elements.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WeaponWizard.Interfaces;
using WeaponWizard.Elements.Enums;
using WeaponWizard.Elements.Components;
using WeaponWizard.Elements;

namespace WeaponWizard.Screens
{
	public class TestScreen : BaseGameScreen
	{
		public TestScreen ()
		{
			BackgroundColor = Color.Green;
		}

		public override void OnEnter ()
		{
			var enter_game_id = Engine.Systems.Register (Message.Enter_Game, OnEnterGame);

			var renderable = new RenderableComponent (Engine.Textures ["default"]);

			Engine.AddEntity (new Entity ()
				.AddComponent (renderable)
				.AddComponent (new TransformComponent (200, 20, renderable.Texture.Width, renderable.Texture.Height))
				.AddComponent (new SlideInAnimationComponent (0.5f, Side.Left)), "player");

			Engine.AddEntity (new Entity ()
				.AddComponent (renderable)
				.AddComponent (new TransformComponent (200, 70, renderable.Texture.Width, renderable.Texture.Height))
				.AddComponent (new SlideInAnimationComponent (0.5f, Side.Right)), "player2");

			Engine.Systems.SendMsg (Message.Enter_Game, "This is data");
			Engine.Systems.Unregister (Message.Enter_Game, enter_game_id);
			Engine.Systems.SendMsg (Message.Enter_Game, "This will not be seen");
		}

		public override Transition Update (GameTime gametime)
		{			
			return null;
		}

		public override void Draw (SpriteBatch spritebatch)
		{
		}

		private void OnEnterGame (object data)
		{
			Console.WriteLine (data.ToString ());
		}
	}
}

