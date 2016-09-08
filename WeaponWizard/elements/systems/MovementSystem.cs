using System;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework;
using WeaponWizard.Elements.Enums;
using System.Collections.Generic;
using System.Linq;
using WeaponWizard.Elements.Components;
using Microsoft.Xna.Framework.Input;

namespace WeaponWizard.Elements.Systems
{
	public class MovementSystem : IMovementSystem
	{
		private GameEngine Engine { get; set; }

		public MovementSystem (GameEngine engine)
		{
			Engine = engine;
		}

		public void Update (IList<Entity> allEntities, GameTime gametime)
		{
			var entities = allEntities.Where (x => x.HasComponent<MovementComponent> ());

			foreach (var entity in entities) {
				var mov = entity.Get<MovementComponent> ();
				var transform = entity.Get<TransformComponent> ();
				var sprite = entity.HasComponent<SpriteAnimationComponent> () ? entity.Get<SpriteAnimationComponent> () : null;

				var spriteState = "idle";

				if (Engine.Systems.InputSystem.IsKey (Keys.Right)) {
					MoveEntity (entity, mov.Speed, 0);
					transform.Rotation = 0;
					spriteState = "Move";
				}
				if (Engine.Systems.InputSystem.IsKey (Keys.Left)) {
					MoveEntity (entity, -mov.Speed, 0);
					transform.Rotation = MathHelper.Pi;
					spriteState = "Move";
				}
				if (Engine.Systems.InputSystem.IsKey (Keys.Up)) {
					MoveEntity (entity, 0, -mov.Speed);
					transform.Rotation = MathHelper.PiOver2;
					spriteState = "Move";
				}
				if (Engine.Systems.InputSystem.IsKey (Keys.Down)) {
					MoveEntity (entity, 0, mov.Speed);
					transform.Rotation = 3 * MathHelper.Pi / 2;
					spriteState = "Move";
				}

				if (sprite != null) {
					sprite.ChangeState (spriteState);
				}
			}
		}

		public void HandleMsg (Message msg, object data)
		{
			switch (msg) {
			default:
				break;
			}
		}

		private void MoveEntity (Entity ent, float x, float y)
		{
			var transform = ent.Get<TransformComponent> ();

			transform.X += x;
			transform.Y += y;

			Engine.CenterCameraOnEntity (ent);
		}
	}
}

