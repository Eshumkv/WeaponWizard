using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WeaponWizard.GameElements;
using System.Linq;

namespace WeaponWizard.VoronoiDiagram
{
	public class Voronoi
	{
		private Random rand;
		private List<VPoint> data;

		public List<VPoint> Points {
			get {
				return data;
			}
		}

		public int Seed { get; private set; }

		public Voronoi (int? seed = null)
		{
			if (seed == null) {
				Seed = new Random ().Next (0, Int32.MaxValue);
			} else {
				Seed = (int)seed;
			}


			rand = new Random (Seed);
		}

		public void Calculate (int amount, int width, int height)
		{
			data = GenerateData (amount, width, height);
			data = GetPoints (data, width, height);
		}

		public void Draw (SpriteBatch batch)
		{			
			foreach (var point in data) {
				if (point.OriginalPoint) {
					DrawPoint (batch, point, Color.Black, 6);
				} else {

					var color = Color.ForestGreen;

					switch (point.Type) {
					case Tile.TileType.Ocean:
						color = Color.DeepSkyBlue;
						break;
					case Tile.TileType.Sand:
						color = Color.YellowGreen;
						break;
					case Tile.TileType.Stone:
						color = Color.Gray;
						break;
					default: 
						break;
					}
					DrawPoint (batch, point, color);
				}
			}
		}

		private List<VPoint> GenerateData (int totalamount, int width, int height)
		{
			var list = new List<VPoint> ();

			int edge_amount = (int)(totalamount * 0.15);
			// Either 7 part or 9
			int height_limit = height / (rand.Next (2) == 0 ? 7 : 9);
			int width_limit = width / (rand.Next (2) == 0 ? 9 : 11);
			int width_part = width / edge_amount;
			int height_part = height / edge_amount;

			// Top part
			for (var i = 0; i < edge_amount; i++) {
				var x = (i * width_part) + rand.Next (width_part);
				var y = rand.Next (height_limit);

				list.Add (new VPoint (x, y, Tile.TileType.Ocean, true));
			}

			// Bottom part
			for (var i = 0; i < edge_amount; i++) {
				var x = (i * width_part) + rand.Next (width_part);
				var y = (height - height_limit) + rand.Next (height_limit);

				list.Add (new VPoint (x, y, Tile.TileType.Ocean, true));
			}

			// Left part
			for (var i = 0; i < edge_amount; i++) {
				var x = rand.Next (width_limit);
				var y = (i * height_part) + rand.Next (height_part);

				list.Add (new VPoint (x, y, Tile.TileType.Ocean, true));
			}

			// Right part
			for (var i = 0; i < edge_amount; i++) {
				var x = (width - width_limit) + rand.Next (width_limit);
				var y = (i * height_part) + rand.Next (height_part);

				list.Add (new VPoint (x, y, Tile.TileType.Ocean, true));
			}

			var startWidth = width_limit;
			var endWidth = width - (width_limit * 2);
			var startHeight = height_limit;
			var endHeight = height - (height_limit * 2);
			var amount = totalamount - (edge_amount * 4);

			for (var i = 0; i < amount; i++) {
				var x = rand.Next (startWidth, endWidth + 1);
				var y = rand.Next (startHeight, endHeight + 1);

				list.Add (new VPoint (x, y, Tile.TileType.Grass, true));
			}

			// Now go through the list again to set the different types of land
			list = SetLandTypes (list);

			return list;
		}

		private List<VPoint> SetLandTypes (IEnumerable<VPoint> points)
		{
			//var amountOfTypes = Enum.GetNames (typeof(Tile.TileType)).Length - 2; 

			foreach (var point in points) {
				var closest = GetClosestPoint (points, point);

				if (closest.Type == Tile.TileType.Ocean && point.Type != Tile.TileType.Ocean) {
					point.Type = Tile.TileType.Sand;
				}
			}

			return points.ToList ();
		}

		private VPoint GetClosestPoint (IEnumerable<VPoint> points, VPoint givenPoint)
		{
			float closest_distance = float.MaxValue;
			var given_point = new Vector2 (givenPoint.X, givenPoint.Y);
			VPoint closest_point = null;

			points = points.Where (x => x != givenPoint);

			foreach (var point in points) {
				var distance = Vector2.Distance (point.ToVector2 (), given_point);

				if (distance < closest_distance) {
					closest_distance = distance;
					closest_point = point;
				}
			}

			return closest_point;
		}

		private List<VPoint> GetPoints (IEnumerable<VPoint> sites, int width, int height)
		{
			var list = new List<VPoint> ();
			
			for (var x = 0; x < width; x++) {
				for (var y = 0; y < height; y++) {
					// max distance
					float closest_distance = width * height;
					var given_point = new Vector2 (x, y);
					VPoint closest_point = null;

					foreach (var point in sites) {
						var distance = Vector2.Distance (point.ToVector2 (), given_point);

						if (distance < closest_distance) {
							closest_distance = distance;
							closest_point = point;
						}
					}

					var original = false;

					if (closest_point.X == x && closest_point.Y == y && closest_point.OriginalPoint) {
						original = true;
					}

					list.Add (new VPoint (x, y) {
						Type = closest_point.Type,
						ClosestPoint = closest_point,
						OriginalPoint = original
					});
				}
			}

			return list;
		}

		#region DRAWING FUNCTIONS

		private void DrawPoint (SpriteBatch batch, VPoint point, Color color, int size = 1)
		{
			var rect = new Rectangle (
				           Convert.ToInt32 (point.X - (size / 2)), 
				           Convert.ToInt32 (point.Y - (size / 2)), 
				           size, 
				           size);

			batch.Draw (GameEngine.DummyTexture, 
				destinationRectangle: rect,
				color: color);
		}

		private void DrawLine (SpriteBatch batch, Vector2 start, Vector2 end)
		{
			var edge = end - start;
			// calculate angle to rotate line
			var angle = (float)Math.Atan2 (edge.Y, edge.X);
			var color = Color.Black;

			batch.Draw (GameEngine.DummyTexture,
				new Rectangle (// rectangle defines shape of line and position of start of line
					(int)start.X,
					(int)start.Y,
					(int)edge.Length (), //sb will strech the texture to fill this rectangle
					1), //width of line, change this to make thicker line
				null,
				color, //colour of line
				angle,     //angle of line (calulated above)
				new Vector2 (0, 0), // point in line about which to rotate
				SpriteEffects.None,
				0);

		}

		#endregion
	}

	public class VPoint
	{
		public int X { get; set; }

		public int Y { get; set; }

		public VPoint ClosestPoint { get; set; }

		public Tile.TileType Type { get; set; }

		public bool OriginalPoint { get; set; }

		public VPoint (int x, int y, Tile.TileType type = Tile.TileType.Grass, bool isOriginal = false)
		{
			X = x;
			Y = y;
			ClosestPoint = null;
			Type = type;
			OriginalPoint = isOriginal;
		}

		public Vector2 ToVector2 ()
		{
			return new Vector2 (X, Y);
		}
	}
}

