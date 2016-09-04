using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WeaponWizard.GameElements
{
	public class Tile
	{
		public enum TileType
		{
			Grass
		}

		public static readonly Point TileSize = new Point (32, 32);

		private int _x, _y;

		public TileType Type { get; set; }

		public Texture2D Texture { get; set; }

		public bool IsPassable { get; set; }

		public Vector2 Position { get; set; }

		public Color Color { get; set; }

		public int X {
			get {
				return _x;
			}
			set {
				_x = value;
				Position = new Vector2 (_x * TileSize.X, _y * TileSize.Y);
			}
		}

		public int Y {
			get {
				return _y;
			}
			set {
				_y = value;
				Position = new Vector2 (_x * TileSize.X, _y * TileSize.Y);
			}
		}

		public Tile (int x, int y, TileType type, Texture2D texture, bool isPassable = true)
		{
			X = x;
			Y = y;
			Type = type;
			Texture = texture;
			IsPassable = isPassable;
			Color = Color.White;
		}
	}
}

