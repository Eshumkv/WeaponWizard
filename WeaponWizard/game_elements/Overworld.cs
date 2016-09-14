using System;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WeaponWizard.Noise;
using System.Linq;
using WeaponWizard.VoronoiDiagram;
using System.Collections;
using WeaponWizard.Elements.Noise;

namespace WeaponWizard.GameElements
{
	// SHOULD HELP: http://www.redblobgames.com/maps/terrain-from-noise/

	public class Overworld : IRenderable
	{
		private Voronoi Vor;
		private List<PPoint> _perlinList;
		private GameEngine _engine;

		public List<Tile> Tiles { get; set; }

		public int Seed { get { return Vor.Seed; } }

		public Tile Spawn { get; private set; }

		public Overworld (GameEngine engine)
		{
			_perlinList = new List<PPoint> ();
			Tiles = new List<Tile> ();

			_engine = engine;

			//GenerateWorldVoronoi ();
			GenerateWorldPerlin ();
		}

		public void Draw (SpriteBatch batch)
		{
			foreach (var tile in Tiles) {
				batch.Draw (tile.Texture, 
					position: tile.Position,
					sourceRectangle: tile.SourceRect,
					color: tile.Color);
			}

			//Vor.Draw (batch);

			foreach (var point in _perlinList) {
				batch.Draw (GameEngine.DummyTexture, 
					position: point.Position,
					color: point.Color);
			}
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

		public void GenerateWorldPerlin (int? seed = null)
		{
			PerlinNoise perlin;

			_perlinList.Clear ();

			if (seed == null)
				perlin = new PerlinNoise (new Random ().Next (Int32.MaxValue));
			else {
				perlin = new PerlinNoise ((int)seed);
			}

			var w = 500;
			var h = 500;

			var widthDivisor = 1 / (double)w;
			var heightDivisor = 1 / (double)h;

			for (var x = 0; x < w; x++) {
				for (var y = 0; y < h; y++) {
					// Note that the result from the noise function is in the range -1 to 1, but I want it in the range of 0 to 1
					// that's the reason of the strange code
					double v =
						// First octave
						(perlin.Noise (2 * x * widthDivisor, 2 * y * heightDivisor, -0.5) + 1) / 2 * 0.7 +
						// Second octave
						(perlin.Noise (4 * x * widthDivisor, 4 * y * heightDivisor, 0) + 1) / 2 * 0.2 +
						// Third octave
						(perlin.Noise (8 * x * widthDivisor, 8 * y * heightDivisor, +0.5) + 1) / 2 * 0.1;

					v = Math.Min (1, Math.Max (0, v));
					_perlinList.Add (new PPoint (x, y, v));
				}
			}
		}


		public void GenerateWorldVoronoi (int? seed = null)
		{
			Tiles.Clear ();

			Vor = new Voronoi (seed);
			Vor.Calculate (150, 300, 300);

			SetSpawnVoronoi ();

			var tileset = _engine.TileSetManager.Current;
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

		private void SetSpawnVoronoi ()
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

		private class PPoint
		{
			public int X { get; set; }

			public int Y { get; set; }

			public Vector2 Position { get; set; }

			public double Value { get; set; }

			public Color Color { get; private set; }

			public PPoint (int x, int y, double v)
			{
				X = x;
				Y = y;
				Value = v;

				var c = (int)(Value * 255);

				Color = new Color (c, c, c);
				Position = new Vector2 (X, Y);
			}
		}
	}
}

