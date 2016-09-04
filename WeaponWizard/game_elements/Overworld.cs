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

		public Overworld (GameEngine engine)
		{
			Tiles = new List<Tile> ();
			var width = 10;
			var height = 10;

			for (var x = 0; x < width; x++) {
				for (var y = 0; y < height; y++) {
					Tiles.Add (new Tile (x, y, Tile.TileType.Grass, engine.Textures ["tile/grass"]));
				}
			}

			Vor = new WeaponWizard.VoronoiDiagram.Voronoi ();
			Vor.Calculate (75);
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
	}
}

