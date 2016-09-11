using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WeaponWizard.VoronoiDiagram;

namespace WeaponWizard.GameElements
{
	public class Tile
	{
		public enum TileType
		{
			Spawn,
			Grass,
			Ocean,
			Sand,
			Stone
		}

		public static readonly Point TileSize = new Point (16, 16);

		private int _x, _y;

		public TileType Type { get; set; }

		public Texture2D Texture { get; set; }

		public Rectangle SourceRect { get; set; }

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

		public Tile (int x, int y, TileType type, Texture2D texture, Rectangle source, bool isPassable = true)
		{
			X = x;
			Y = y;
			Type = type;
			Texture = texture;
			IsPassable = isPassable;
			Color = Color.White;
			SourceRect = source;
		}

		public Tile (VPoint point, Texture2D texture, Rectangle source, bool isPassable = true) : this (point.X, point.Y, point.Type, texture, source, isPassable)
		{
		}
	}
}

