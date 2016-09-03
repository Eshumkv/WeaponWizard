using System;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using WeaponWizard.Elements;
using System.Collections.Generic;
using System.Linq;
using WeaponWizard.Elements.Components;
using WeaponWizard.Elements.Enums;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Elements.Systems
{
	public class BasicRenderSystem : IRenderSystem
	{
		public Camera2D Camera { get; set; }

		public BasicRenderSystem ()
		{
			Camera = null;
		}

		public void Draw (IList<Entity> drawEntities, SpriteBatch spritebatch)
		{
			var entities = drawEntities.Where (x => x.HasComponent<RenderableComponent> ()).OrderBy (x => x.Get<RenderableComponent> ().Z);
			var textEntities = drawEntities.Where (x => x.HasComponent<TextComponent> ());

			if (Camera == null)
				spritebatch.Begin ();
			else
				spritebatch.Begin (transformMatrix: Camera.Transform);

			foreach (var entity in entities) {
				var comp = entity.Get<RenderableComponent> ();
				var transform = entity.Get<TransformComponent> ();
				spritebatch.Draw (comp.Texture, 
					position: transform.Position.ToVector2 (),
					sourceRectangle: comp.SourceRect,
					origin: transform.Origin,
					rotation: transform.Rotation,
					scale: transform.Scale,
					color: comp.Color);
			}

			foreach (var entity in textEntities) {
				var comp = entity.Get<TextComponent> ();
				var loc = comp.Location;

				if (entity.HasComponent<TransformComponent> ()) {
					var transform = entity.Get<TransformComponent> ();

					loc += transform.AccuratePosition;
				}

				loc = new Vector2 ((int)loc.X, (int)loc.Y);

				spritebatch.DrawString (comp.Font, comp.Text, loc, comp.Color);
			}

			spritebatch.End ();
		}

		public void HandleMsg (Message msg, object data)
		{
			switch (msg) {
			default:
				break;
			}
		}
	}
}

