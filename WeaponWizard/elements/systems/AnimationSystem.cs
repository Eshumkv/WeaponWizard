using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using WeaponWizard.Elements;
using WeaponWizard.Elements.Components;
using WeaponWizard.Elements.Enums;
using WeaponWizard.Interfaces;
using System.Linq;

namespace WeaponWizard.Elements.Systems
{
	public class AnimationSystem : IAnimationSystem
	{
		private GameEngine _engine;
		private Dictionary<Type, Action<Entity, GameTime>> _registeredComponentTypes;

		public bool Paused { get; set; }

		public AnimationSystem (GameEngine ge)
		{
			Paused = false;
			
			_engine = ge;
			_registeredComponentTypes = new Dictionary<Type, Action<Entity, GameTime>> ();

			_registeredComponentTypes.Add (typeof(SlideInAnimationComponent), SlideInAnimationHandler);
			_registeredComponentTypes.Add (typeof(RotateAnimationComponent), RotateAnimationHandler);
			_registeredComponentTypes.Add (typeof(MovieAnimationComponent), MovieAnimationHandler);
			_registeredComponentTypes.Add (typeof(SpriteAnimationComponent), SpriteAnimationHandler);
		}

		public void Update (IList<Entity> entities, GameTime gametime)
		{
			if (Paused)
				return;
			
			foreach (var entity in entities) {
				foreach (var kv in _registeredComponentTypes) {
					if (entity.HasComponent (kv.Key)) {
						kv.Value (entity, gametime);
					}
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

		private void SlideInAnimationHandler (Entity entity, GameTime gametime)
		{
			var animation = entity.Get<SlideInAnimationComponent> ();
			var transform = entity.Get<TransformComponent> ();

			if (!animation.HasAnimationStarted) {
				var oldX = transform.X;
				var oldY = transform.Y;

				if (animation.SlideInSide == Side.Left) {
					transform.X = 0 - transform.Width - 10;
				} else if (animation.SlideInSide == Side.Right) {
					transform.X = _engine.GetScreenSize ().X + 10;
				} else if (animation.SlideInSide == Side.Up) {
					transform.Y = 0 - transform.Height - 10;
				} else {
					transform.Y = _engine.GetScreenSize ().Y + 10;
				}

				animation.StartPosition = new Vector2 (transform.X, transform.Y);
				animation.Destination = new Vector2 (oldX, oldY);
				animation.Delta = 0;

				animation.HasAnimationStarted = true;
				_engine.Systems.SendMsg (Message.Start_Animation, entity);
			} else {
				animation.Delta += (float)gametime.ElapsedGameTime.TotalSeconds / animation.TimeToReach;
				transform.AccuratePosition = Vector2.SmoothStep (animation.StartPosition, animation.Destination, animation.Delta);

				if ((animation.SlideInSide == Side.Left && transform.AccuratePosition.X >= animation.Destination.X) ||
				    (animation.SlideInSide == Side.Right && transform.AccuratePosition.X <= animation.Destination.X) ||
				    (animation.SlideInSide == Side.Up && transform.AccuratePosition.Y >= animation.Destination.Y) ||
				    (animation.SlideInSide == Side.Down && transform.AccuratePosition.Y <= animation.Destination.Y)) {
					transform.AccuratePosition = animation.Destination;

					entity.RemoveComponent<SlideInAnimationComponent> ();
					_engine.Systems.SendMsg (Message.End_Animation, entity);
				}
			}
		}

		private void RotateAnimationHandler (Entity entity, GameTime gametime)
		{
			var rotatecomp = entity.Get<RotateAnimationComponent> ();
			var transformcomp = entity.Get<TransformComponent> ();

			transformcomp.Rotation += rotatecomp.Speed / 100.0f;

			if (transformcomp.Rotation >= MathHelper.TwoPi) {
				transformcomp.Rotation -= MathHelper.TwoPi;
			}
		}

		private void SpriteAnimationHandler (Entity entity, GameTime time)
		{
			var sprite = entity.Get<SpriteAnimationComponent> ();
			var renderable = entity.Get<RenderableComponent> ();

			if (!sprite.Frames.ContainsKey (sprite.CurrentState))
				return;

			if (sprite.LastGameTime == null)
				sprite.LastGameTime = new GameTime (time.TotalGameTime, time.ElapsedGameTime);

			var currentFrame = sprite.Get ();

			if (time.TotalGameTime >= sprite.LastGameTime.TotalGameTime + currentFrame.Time) {
				sprite.CurrentFrame++;

				if (sprite.CurrentFrame >= sprite.Frames [sprite.CurrentState].Count)
					sprite.CurrentFrame = 0;

				renderable.SourceRect = sprite.Frames [sprite.CurrentState] [sprite.CurrentFrame].SourceRect;

				sprite.LastGameTime = new GameTime (time.TotalGameTime, time.ElapsedGameTime);
			}
		}

		private void MovieAnimationHandler (Entity entity, GameTime time)
		{
			var sprite = entity.Get<MovieAnimationComponent> ();
			var renderable = entity.Get<RenderableComponent> ();

			if (sprite.LastGameTime == null)
				sprite.LastGameTime = new GameTime (time.TotalGameTime, time.ElapsedGameTime);

			if (time.TotalGameTime >= sprite.LastGameTime.TotalGameTime + sprite.Frames [sprite.CurrentFrame].Time) {
				sprite.CurrentFrame++;

				if (sprite.CurrentFrame >= sprite.Frames.Count)
					sprite.CurrentFrame = 0;

				renderable.SourceRect = sprite.Frames [sprite.CurrentFrame].SourceRect;

				sprite.LastGameTime = new GameTime (time.TotalGameTime, time.ElapsedGameTime);
			}
		}
	}
}

