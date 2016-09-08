using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WeaponWizard.VoronoiDiagram
{
	public class Voronoi
	{
		private List<VPoint> data;

		public Voronoi ()
		{
		}

		public void Calculate (int amount)
		{
			var width = 400;
			var height = 400;

			data = GenerateData (amount, width, height);
			data = GetPoints (data, width, height);
		}

		public void Draw (SpriteBatch batch)
		{
			batch.Draw (GameEngine.DummyTexture, 
				destinationRectangle: new Rectangle (0, 0, 400, 400),
				color: Color.White);
			
			foreach (var point in data) {
				DrawPoint (batch, point);
			}
		}

		private List<VPoint> GenerateData (int amount, int width, int height)
		{
			var list = new List<VPoint> ();
			var rand = new Random ();

			int edge_amount = (int)(amount * 0.15);
			// Either 7 part or 9
			int height_limit = height / (rand.Next (2) == 0 ? 7 : 9);
			int width_limit = width / (rand.Next (2) == 0 ? 9 : 11);
			int width_part = width / edge_amount;
			int height_part = height / edge_amount;

			// Top part
			for (var i = 0; i < edge_amount; i++) {
				var x = (i * width_part) + rand.Next (width_part);
				var y = rand.Next (height_limit);

				list.Add (new VPoint (x, y) { Color = Color.DeepSkyBlue });
			}

			// Bottom part
			for (var i = 0; i < edge_amount; i++) {
				var x = (i * width_part) + rand.Next (width_part);
				var y = (height - height_limit) + rand.Next (height_limit);

				list.Add (new VPoint (x, y) { Color = Color.DeepSkyBlue });
			}

			// Left part
			for (var i = 0; i < edge_amount; i++) {
				var x = rand.Next (width_limit);
				var y = (i * height_part) + rand.Next (height_part);

				list.Add (new VPoint (x, y) { Color = Color.DeepSkyBlue });
			}

			// Right part
			for (var i = 0; i < edge_amount; i++) {
				var x = (width - width_limit) + rand.Next (width_limit);
				var y = (i * height_part) + rand.Next (height_part);

				list.Add (new VPoint (x, y) { Color = Color.DeepSkyBlue });
			}

			for (var i = 0; i < (amount - (edge_amount * 4)); i++) {
				var x = width_limit + rand.Next (width - (width_limit * 2));
				var y = height_limit + rand.Next (height - (height_limit * 2));

				list.Add (new VPoint (x, y) { Color = Color.ForestGreen });
			}

			return list;
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

					list.Add (new VPoint (x, y) {
						Color = closest_point.Color,
						ClosestPoint = closest_point
					});
				}
			}

			return list;
		}

		#region DRAWING FUNCTIONS

		private void DrawPoint (SpriteBatch batch, VPoint point)
		{
			var size = 1;

			var rect = new Rectangle (
				           Convert.ToInt32 (point.X - (size / 2)), 
				           Convert.ToInt32 (point.Y - (size / 2)), 
				           size, 
				           size);

			batch.Draw (GameEngine.DummyTexture, 
				destinationRectangle: rect,
				color: point.Color);
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

		public Color Color { get; set; }

		public VPoint (int x, int y)
		{
			X = x;
			Y = y;
			Color = Color.Blue;
			ClosestPoint = null;
		}

		public Vector2 ToVector2 ()
		{
			return new Vector2 (X, Y);
		}
	}
}

