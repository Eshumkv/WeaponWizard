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
			data = new List<VPoint> ();
			var rand = new Random ();
			var width = 1000;
			var height = 500;

			for (var i = 0; i < amount; i++) {
				var c = new Color (rand.Next (255), rand.Next (255), rand.Next (255));
				data.Add (new VPoint (rand.Next (width), rand.Next (height)) { Color = c });
			}

			data = GetPoints (data, width, height);
		}

		public void Draw (SpriteBatch batch)
		{
			foreach (var point in data) {
				DrawPoint (batch, point);
			}
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
			var size = 6;

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

