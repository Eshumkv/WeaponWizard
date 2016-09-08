using System;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WeaponWizard.Elements;
using WeaponWizard.Elements.Components;
using Microsoft.Xna.Framework.Input;
using WeaponWizard.GameElements;
using WeaponWizard.Elements.Animation;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace WeaponWizard.Screens
{
	public class OverworldScreen : BaseGameScreen
	{
		Overworld _world;

		public OverworldScreen ()
		{
			BackgroundColor = Color.LightYellow;

			_world = new Overworld (Engine);
		}

		public override void OnEnter ()
		{
			Engine.AddEntity (new Entity ()
				.AddComponent (new TransformComponent (0, 0, 32, 39) {
				Origin = new Vector2 (32 / 2, 39 / 2),
				AccuratePosition = _world.GetTileMiddlePoint (0, 0).ToVector2 ()
			})
				.AddComponent (new RenderableComponent (Engine.Textures ["player"]) { SourceRect = new Rectangle (0, 0, 32, 39) })
				.AddComponent (new MovementComponent (){ CenterCameraOnAction = true })
				.AddComponent (new SpriteAnimationComponent (Engine.AnimationStore.Get ("player", "Idle", "Move"))), "player");

			Engine.CenterCameraOnEntity (Engine ["player"]);
		}

		public override Transition Update (GameTime gametime)
		{
			if (Engine.Systems.InputSystem.IsPressed (Keys.Escape)) {
				return new Transition () { NextScreen = "mainmenu" };
			}

			return null;
		}

		public override void Draw (SpriteBatch spritebatch)
		{
			spritebatch.Begin (transformMatrix: Engine.Camera.Transform);

			_world.Draw (spritebatch);

			spritebatch.End ();
		}
	}
}

