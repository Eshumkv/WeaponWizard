using System;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WeaponWizard.Noise;

namespace WeaponWizard.GameElements
{
	// SHOULD HELP: http://www.redblobgames.com/maps/terrain-from-noise/

	public class Overworld : IRenderable
	{
		private VoronoiDiagram.Voronoi Vor;

		public List<Tile> Tiles { get; set; }

		public int Seed { get { return Vor.Seed; } }

		public Overworld (GameEngine engine)
		{
			Tiles = new List<Tile> ();
			GenerateWorld (engine);
		}

		public void Draw (SpriteBatch batch)
		{
			foreach (var tile in Tiles) {
				batch.Draw (tile.Texture, 
					position: tile.Position,
					color: tile.Color);
			}

			Vor.Draw (batch);
		}

		public void CenterCameraOnTile (GameEngine engine, int tileX, int tileY)
		{
			var screen = engine.GetScreenSize ();
			engine.Camera.Position = new Vector2 (screen.X / 2 - tileX * Tile.TileSize.X - (Tile.TileSize.X / 2), screen.Y / 2 - tileY * Tile.TileSize.Y - (Tile.TileSize.Y / 2));
		}

		public Point GetTileMiddlePoint (int x, int y)
		{
			return new Rectangle (x * Tile.TileSize.X, y * Tile.TileSize.Y, Tile.TileSize.X, Tile.TileSize.Y).Center;
		}

		public void GenerateWorld (GameEngine engine, int? seed = null)
		{
			Tiles.Clear ();
			Vor = new WeaponWizard.VoronoiDiagram.Voronoi (seed);
			Vor.Calculate (150, 300, 300);

			foreach (var point in Vor.Points) {
				var texturestr = "tile/grass";
				var isPassable = true;

				switch (point.Type) {
				case Tile.TileType.Ocean:
					texturestr = "tile/ocean";
					isPassable = false;
					break;
				default:
					break;
				}

				var tile = new Tile (point.X, point.Y, point.Type, engine.Textures [texturestr], isPassable);
				Tiles.Add (tile);
			}
		}
	}
}

