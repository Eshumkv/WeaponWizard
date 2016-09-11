using System;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WeaponWizard.Noise;
using System.Linq;
using WeaponWizard.VoronoiDiagram;
using System.Collections;

namespace WeaponWizard.GameElements
{
	// SHOULD HELP: http://www.redblobgames.com/maps/terrain-from-noise/

	public class Overworld : IRenderable
	{
		private Voronoi Vor;

		public List<Tile> Tiles { get; set; }

		public int Seed { get { return Vor.Seed; } }

		public Tile Spawn { get; private set; }

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
					sourceRectangle: tile.SourceRect,
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

			Vor = new Voronoi (seed);
			Vor.Calculate (150, 300, 300);

			SetSpawn ();

			var tileset = engine.TileSetManager.Current;
			var rand = new Random ();

			foreach (var point in Vor.Points) {
				var tilesettile = tileset.GetRandomTile (point.Type, rand);
				var tile = new Tile (point, tileset.Texture, tilesettile.Rect, tilesettile.IsPassable);
				Tiles.Add (tile);

				if (point.Type == Tile.TileType.Spawn) {
					Spawn = tile;
				}
			}
		}

		private void SetSpawn ()
		{
			var grassTiles = Vor.Points.Where (x => x.Type == Tile.TileType.Grass);
			var count = grassTiles.Count ();
			var point = grassTiles.ElementAt (new Random ().Next (count));
			var surrounding = GetSurroundingTiles (point.X, point.Y);

			var valid = false;

			while (!valid) {
				if (surrounding.All ().Any (p => p.Type == Tile.TileType.Grass)) {
					valid = true;
				} else {
					var x = point.X;
					var y = point.Y;

					if (surrounding.Point1.Type == Tile.TileType.Ocean) {
						x++;
						y++;
					} else if (surrounding.Point2.Type == Tile.TileType.Ocean) {
						y++;
					} else if (surrounding.Point3.Type == Tile.TileType.Ocean) {
						x--;
						y++;
					} else if (surrounding.Point4.Type == Tile.TileType.Ocean) {
						x++;
					} else if (surrounding.Point5.Type == Tile.TileType.Ocean) {
						x--;
					} else if (surrounding.Point6.Type == Tile.TileType.Ocean) {
						x++;
						y--;
					} else if (surrounding.Point7.Type == Tile.TileType.Ocean) {
						y--;
					} else if (surrounding.Point8.Type == Tile.TileType.Ocean) {
						x--;
						y--;
					}
					point = GetPoint (x, y);
					surrounding = GetSurroundingTiles (point.X, point.Y);
				}
			}

			point.Type = Tile.TileType.Spawn;
		}

		private VPoint GetPoint (int x, int y)
		{
			return Vor.Points.Where (p => p.X == x && p.Y == y).First ();
		}

		private SurroundingTiles GetSurroundingTiles (int x, int y)
		{
			var sp = new SurroundingTiles ();

			sp.Point1 = GetPoint (x - 1, y - 1);
			sp.Point2 = GetPoint (x, y - 1);
			sp.Point3 = GetPoint (x + 1, y - 1);
			sp.Point4 = GetPoint (x - 1, y);
			sp.Point5 = GetPoint (x + 1, y);
			sp.Point6 = GetPoint (x - 1, y + 1);
			sp.Point7 = GetPoint (x, y + 1);
			sp.Point8 = GetPoint (x + 1, y + 1);

			return sp;
		}

		private class SurroundingTiles
		{
			/*
			 * 1 2 3
			 * 4 X 5
			 * 6 7 8
			 */

			public VPoint Point1 { get; set; }

			public VPoint Point2 { get; set; }

			public VPoint Point3 { get; set; }

			public VPoint Point4 { get; set; }

			public VPoint Point5 { get; set; }

			public VPoint Point6 { get; set; }

			public VPoint Point7 { get; set; }

			public VPoint Point8 { get; set; }

			public IEnumerable<VPoint> All ()
			{
				return new List<VPoint> () { Point1, Point2, Point3, Point4, Point5, Point6, Point7, Point8 };
			}
		}
	}
}

