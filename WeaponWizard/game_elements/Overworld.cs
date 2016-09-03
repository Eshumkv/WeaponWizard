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
		private OpenSimplexNoise _noise { get; set; }

		public List<Tile> Tiles { get; set; }

		public Overworld (GameEngine engine)
		{
			Tiles = new List<Tile> ();
			_noise = new OpenSimplexNoise (4564131235);
			// (Octave, (frequency_x, frequency_y))
			var octaves = new List<Tuple<double, Tuple<int, int>>> () { 
				new Tuple<double, Tuple<int, int>> (0.34, new Tuple<int, int> (1, 1)),
				new Tuple<double, Tuple<int, int>> (0.5, new Tuple<int, int> (2, 2)),
				new Tuple<double, Tuple<int, int>> (0.25, new Tuple<int, int> (4, 4)),
				new Tuple<double, Tuple<int, int>> (0.13, new Tuple<int, int> (8, 8)),
				new Tuple<double, Tuple<int, int>> (0.06, new Tuple<int, int> (16, 16)),
				new Tuple<double, Tuple<int, int>> (0.03, new Tuple<int, int> (32, 32))
			};
			var redistribution = 1.25;

			var width = 800;
			var height = 800;

			for (var x = 0; x < width; x++) {
				for (var y = 0; y < height; y++) {
					// Standard is a range from -1 to 1;
					// Convert it to 0 to 1
					// var nx = Helper.ConvertRange (x, 0, width, 0.0, 1.0);
					// var ny = Helper.ConvertRange (y, 0, height, 0.0, 1.0);
					var nx = x / Convert.ToDouble (width) - 0.5;
					var ny = y / Convert.ToDouble (height) - 0.5;
					double no = 0;

					double t_o = 0;

					foreach (var tuple in octaves) {
						var octave = tuple.Item1;
						var frequency_x = tuple.Item2.Item1;
						var frequency_y = tuple.Item2.Item2;
						no += octave * Noise (frequency_x * nx, frequency_y * ny);
						t_o += octave;
					}

					no /= t_o;

					no = Math.Pow (no, redistribution);
					no = MakeIsland (no, nx, ny, true);

					var n = (float)no;
					//Tiles.Add (new Tile (x, y, Tile.TileType.Grass, engine.Textures ["tile/grass"]));
					Tiles.Add (new Tile (x, y, Tile.TileType.Grass, GameEngine.DummyTexture) { Color = new Color (n, n, n) });
				}
			}
		}

		private double Noise (double nx, double ny)
		{
			// rescale form -1.0:+1.0 to 0.0:+1.0
			return _noise.Evaluate (nx, ny) / 2.0 + 0.5;
		}

		private double MakeIsland (double elevation, double nx, double ny, bool euclidean = false, bool Add = false)
		{
			var a = 0.02;
			var b = 0.65;
			var c = 10;
			double d = 0;

			if (euclidean) {
				d = 2 * Math.Sqrt (nx * nx + ny * ny);
			} else {
				// Manhatten distance
				d = 2 * Math.Max (Math.Abs (nx), Math.Abs (ny));
			}

			if (Add) {
				return elevation + a - b * Math.Pow (d, c);
			} else {
				// Multiply
				return (elevation + a) * (1 - b * Math.Pow (d, c));
			}
		}

		public void Draw (SpriteBatch batch)
		{
			foreach (var tile in Tiles) {
				batch.Draw (tile.Texture, 
					position: tile.Position,
					color: tile.Color);
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
	}
}

