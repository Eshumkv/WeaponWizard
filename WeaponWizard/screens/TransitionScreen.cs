using System;
using WeaponWizard.Elements.Gui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WeaponWizard.Interfaces;
using WeaponWizard.Elements.Enums;
using WeaponWizard.Elements.Components;
using WeaponWizard.Elements;
using System.ComponentModel;
using System.Threading;

namespace WeaponWizard.Screens
{
	public class TransitionScreen : BaseGameScreen
	{
		private IScreen _transitionFromScreen;
		private IScreen _transitionToScreen;
		private bool _isGoingOpaque;
		private bool _isTransitioning;
		private bool _isLoading;
		private float _speed;
		private float _alpha;
		private Rectangle _rect;

		private static Texture2D _loadingTexture;

		public readonly Color ColorFrom;
		public readonly Color ColorTo;

		public Color CurrentColor { get; private set; }

		public TransitionScreen (IScreen screenToTransitionTo, IScreen screenToTransitionFrom, Color colorFrom, Color colorTo, float speed = 0.25f)
		{
			BackgroundColor = screenToTransitionFrom.BackgroundColor;

			_transitionFromScreen = screenToTransitionFrom;
			_transitionToScreen = screenToTransitionTo;
			_isGoingOpaque = true;
			_isTransitioning = true;
			_isLoading = false;
			ColorFrom = colorFrom;
			ColorTo = colorTo;
			_speed = speed;
			_alpha = 0f;
		}

		public override void LoadContent ()
		{
			_rect = new Rectangle (0, 0, Engine.GetScreenSize ().X, Engine.GetScreenSize ().Y);

			if (_loadingTexture == null) {
				_loadingTexture = Engine.Textures ["loading"];
			}
		}

		public override void OnEnter ()
		{
//			var loading = new Entity ();
//
//			var transform = loading.AddComponent (new TransformComponent (0, 0, _loadingTexture.Width, _loadingTexture.Height));
//			loading.AddComponent (new RenderableComponent (_loadingTexture));
//			loading.AddComponent (new RotateAnimationComponent (1f));
//			loading.AddComponent (new TextComponent ("Loading", Color.White, new Vector2 (_loadingTexture.Width / 2, 0)));
//
//			transform.Origin = new Vector2 (transform.Width / 2, transform.Height / 2);
//			transform.Scale = new Vector2 (0.5f, 0.5f);
//			transform.X += (transform.Width / 2) * transform.Scale.X;
//			transform.Y += (transform.Height / 2) * transform.Scale.Y;
//
//			Engine.AddEntity (loading, "loading");
		}

		public override Transition Update (GameTime gametime)
		{
//			if (!_isTransitioning && _isLoading) {
//				return null;
//			}
//			
//			Color color = ColorTo;
//			
//			if (_isGoingOpaque) {
//				color = ColorFrom;
//
//				_alpha += (float)gametime.ElapsedGameTime.TotalSeconds / _speed;
//
//				if (_alpha >= 1.0f) {
//					_isGoingOpaque = false;
//					_isTransitioning = false;
//					_alpha = 1f;
//				}
//			} else {
//				_alpha -= (float)gametime.ElapsedGameTime.TotalSeconds / _speed;
//
//				if (_alpha <= 0.0f) {
//					_alpha = 0f;
//					SetColor (color);
//					return _transitionToScreen;
//				}
//			}
//
//			SetColor (color);

			return null;
		}

		public override void Draw (SpriteBatch spritebatch)
		{
			if (_isGoingOpaque) {
				_transitionFromScreen.Draw (spritebatch);
			} else {
				_transitionToScreen.Draw (spritebatch);
				BackgroundColor = _transitionToScreen.BackgroundColor;
			}
			
			spritebatch.Begin ();

			spritebatch.Draw (GameEngine.DummyTexture, destinationRectangle: _rect, color: CurrentColor);

			spritebatch.End ();
		}

		private void SetColor (Color colorTo)
		{
			CurrentColor = new Color (colorTo, _alpha);

			var loadingRenderable = Engine.GetEntity ("loading").Get<RenderableComponent> ();
			loadingRenderable.Color = new Color (loadingRenderable.Color, _alpha);
		}
	}
}

