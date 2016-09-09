using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WeaponWizard.Interfaces;
using System.Collections.Generic;
using WeaponWizard.Elements;
using System.Linq;
using WeaponWizard.Elements.Enums;
using WeaponWizard.Screens;
using WeaponWizard.Elements.Systems;
using WeaponWizard.Elements.Animation;
using WeaponWizard.GameElements;

namespace WeaponWizard
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class GameEngine : Game
	{
		public Resources<Texture2D> Textures = new Resources<Texture2D> (new Dictionary<string, string> () {
			{ "default", "default" },
			{ "loading", "loading" },
			{ "mainmenu_bg", "mainmenu/background" },
			{ "tile/default", "tiles/default" },
			{ "tile/grass", "tiles/grass" },
			{ "tile/ocean", "tiles/ocean" },
			{ "player", "player" }
		});
		public Dictionary<Tile.TileType, Rectangle> TileSourceRects = new Dictionary<Tile.TileType, Rectangle> () {
			{ Tile.TileType.Spawn, new Rectangle (0, 0, Tile.TileSize.X, Tile.TileSize.Y) },
			{ Tile.TileType.Grass, new Rectangle (0, 0, Tile.TileSize.X, Tile.TileSize.Y) },
			{ Tile.TileType.Ocean, new Rectangle (16, 0, Tile.TileSize.X, Tile.TileSize.Y) },
		};

		public Dictionary<string, IScreen> Screens;

		#region Private Variables

		private static Texture2D _dummyTexture;
		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		private FrameCounter _frameCounter;
		private IScreen _screen;
		private Dictionary<string, Entity> _entities;

		#endregion

		#region Public Properties

		public SystemManager Systems { get; private set; }

		public AnimationDataStore AnimationStore { get; private set; }

		public static Texture2D DummyTexture {
			get {
				if (_dummyTexture == null) {
					throw new InvalidOperationException ("GameEngine not started yet.");
				}

				return _dummyTexture;
			}
		}

		public Camera2D Camera {
			get {
				return Systems.RenderSystem.Camera;
			}
			set {
				Systems.RenderSystem.Camera = value;
			}
		}

		public Entity this [string name] {
			get {
				return GetEntity (name);
			}
		}

		#endregion

		public GameEngine ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
			Systems = new SystemManager ();
			_frameCounter = new FrameCounter ();
			_entities = new Dictionary<string, Entity> ();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{			
			IsMouseVisible = true;

			this.Exiting += (sender, e) => {
				ClearEntities ();
			};

			// Get supported resolutions
			var s = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;

			SetResolution (1024, 768);

			base.Initialize ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			_dummyTexture = new Texture2D (graphics.GraphicsDevice, 1, 1);
			_dummyTexture.SetData (new Color[] { Color.White });
			Fonts.SetContentManager (Content);

			Systems.LoadSystems (this);

			Camera = new Camera2D ();

			//_screen = Helper.GetPythonScreen ("scripts/screens/main.py", "MainScreen");
			//_screen = Helper.RunPythonMain ("scripts/screens/main.py") ();

			Textures.Load (Content);
			AnimationStore = new AnimationDataStore (this, "Content\\data\\animations\\".ToPath (true));
			AnimationStore.LoadAll ();

			BaseGameScreen.StaticEngine = this;
			LoadScreens ();

			_screen = Screens ["mainmenu"];
			_screen.OnEnter ();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			Systems.UpdateBefore (gameTime, _entities);

			var t = _screen.Update (gameTime);

			if (t != null) {
				_screen.OnExit ();
				_screen = Screens [t.NextScreen];
				_screen.OnEnter ();
			}

			Systems.UpdateAfter (gameTime, _entities);

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			_frameCounter.Update ((float)gameTime.ElapsedGameTime.TotalSeconds);
			graphics.GraphicsDevice.Clear (_screen.BackgroundColor);

			_screen.Draw (spriteBatch);
			Systems.Draw (spriteBatch, _entities);
			_screen.DrawAfterEntities (spriteBatch);

			base.Draw (gameTime);
		}

		private void MaybeChangeScreen (Transition t)
		{
		}

		/// <summary>
		/// Adds an entity.
		/// </summary>
		/// <returns>The entity.</returns>
		/// <param name="ent">Entity.</param>
		/// <param name="easyName">Easy name.</param>
		public Entity AddEntity (Entity ent, string easyName = null)
		{
			if (string.IsNullOrEmpty (easyName)) {
				easyName = Guid.NewGuid ().ToString ();
			}

			_entities.Add (easyName, ent);
			return ent;
		}

		/// <summary>
		/// Removes the entity.
		/// </summary>
		/// <returns>The entity.</returns>
		/// <param name="ent">Entity.</param>
		public Entity RemoveEntity (Entity ent)
		{
			foreach (var item in _entities.Where(x => x.Value == ent)) {
				_entities.Remove (item.Key);
			}

			return ent;
		}

		/// <summary>
		/// Removes the entity.
		/// </summary>
		/// <returns>The entity.</returns>
		/// <param name="easyName">Easy name.</param>
		public Entity RemoveEntity (string easyName)
		{
			var ent = GetEntity (easyName);
			_entities.Remove (easyName);

			return ent;
		}

		/// <summary>
		/// Clears the entities.
		/// </summary>
		public void ClearEntities ()
		{
			_entities.Clear ();
		}

		/// <summary>
		/// Gets the entity.
		/// </summary>
		/// <returns>The entity.</returns>
		/// <param name="easyName">Easy name.</param>
		public Entity GetEntity (string easyName)
		{
			return _entities [easyName];
		}

		public void LoadScreens ()
		{
			Screens = new Dictionary<string, IScreen> () {
				{ "test", new TestScreen () },

				{ "mainmenu", new MainMenuScreen () },
				{ "overworld", new OverworldScreen () }
			};
		}

		/// <summary>
		/// Gets the FPS.
		/// </summary>
		/// <returns>The FPS.</returns>
		public float GetFPS ()
		{
			return _frameCounter.AverageFramesPerSecond;
		}

		/// <summary>
		/// Gets the size of the screen.
		/// </summary>
		/// <returns>The screen size.</returns>
		public Point GetScreenSize ()
		{
			return new Point (graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
		}

		private void SetResolution (int w, int h)
		{			
			graphics.PreferredBackBufferWidth = w;
			graphics.PreferredBackBufferHeight = h;
			graphics.ApplyChanges ();
		}

		public void CenterCameraOnEntity (Entity ent)
		{
			var t = ent.Get<WeaponWizard.Elements.Components.TransformComponent> ();
			var middle = new Rectangle ((int)t.X, (int)t.Y, t.Width, t.Height).Center;
			Camera.CenterCameraOnPoint (this, middle.X, middle.Y);
		}
	}
}

