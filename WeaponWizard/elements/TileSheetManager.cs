using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using WeaponWizard.GameElements;
using Microsoft.Xna.Framework.Graphics;

namespace WeaponWizard
{
	public class TileSheetManager
	{
		private GameEngine _engine;
		private string _path;

		private Dictionary<string, TileSheetJsonFile> _tilesets;

		public string CurrentTileSet { get; set; }

		public string Path {
			get {
				return _path;
			}
		}

		public TileSheetJsonFile Current {
			get {
				return _tilesets [CurrentTileSet];
			}
		}

		public TileSheetManager (GameEngine engine, string path)
		{
			_path = path;
			_tilesets = new Dictionary<string, TileSheetJsonFile> ();
			_engine = engine;
		}

		public void Load (string defaultTileset = "default")
		{
			var files = Helper.GetAllFiles (_path);

			foreach (var file in files) {
				using (var sr = new StreamReader (file)) {
					try {
						var tilesheetfile = JsonConvert.DeserializeObject<TileSheetJsonFile> (sr.ReadToEnd ());
						tilesheetfile.SetCorrectSizes ();
						_tilesets.Add (tilesheetfile.Id, tilesheetfile);

					} catch (Exception e) {
						Helper.ShowMsgBox ("ERROR: <file: " + file + ">" + e.Message);
					}
				}
			}

			// Simple error checking
			foreach (var tileset in _tilesets) {
				foreach (var item in tileset.Value.Tiles) {
					if (item.Value.Count <= 0) {
						throw new Exception ("Tileset has no valid tiles in it. (" + tileset.Key + " : " + item.Key + ")");
					}
				}
			}

			CurrentTileSet = defaultTileset;
		}

		public void SetTextures ()
		{
			foreach (var tileset in _tilesets.Values) {
				tileset.Texture = _engine.Textures [tileset.ImageName];
			}
		}

		public IDictionary<string, string> GetImagePaths ()
		{
			var dict = new Dictionary<string, string> ();

			foreach (var tileset in _tilesets.Values) {
				dict.Add (tileset.ImageName, tileset.Image);
			}

			return dict;
		}
	}

	public class TileSheetJsonFile
	{
		public string Id { get; set; }

		public string ImageName { get; set; }

		public string Image { get; set; }

		public Point TileSize { get; set; }

		[JsonIgnore]
		public Dictionary<Tile.TileType, List<TileSheetTile>> Tiles { get; set; }

		[JsonIgnore]
		public Texture2D Texture { get; set; }

		public Dictionary<Tile.TileType, List<TileSheetJsonFileTile>> SetTiles { get; set; }

		public TileSheetJsonFile ()
		{
			Tiles = new Dictionary<Tile.TileType, List<TileSheetTile>> ();
		}

		public void SetCorrectSizes ()
		{
			var tiles = new Dictionary<Tile.TileType, List<TileSheetTile>> (Tiles.Count);
			
			foreach (var tile in SetTiles) {
				var list = new List<TileSheetTile> ();

				foreach (var rect in tile.Value) {
					list.Add (new TileSheetTile (new Rectangle (rect.X * TileSize.X, rect.Y * TileSize.Y, TileSize.X, TileSize.Y), rect.Pass));
				}

				tiles.Add (tile.Key, list);
			}

			Tiles = tiles;
		}

		public TileSheetTile GetRandomTile (Tile.TileType type, Random rand = null)
		{
			if (rand == null) {
				rand = new Random ();
			}

			return Tiles [type] [rand.Next (Tiles [type].Count)];
		}
	}

	public class TileSheetTile
	{
		public Rectangle Rect { get; set; }

		public bool IsPassable { get; set; }

		public TileSheetTile (Rectangle rect, bool ispassable)
		{
			Rect = rect;
			IsPassable = ispassable;
		}
	}

	public class TileSheetJsonFileTile
	{
		public int X { get; set; }

		public int Y { get; set; }

		public bool Pass { get; set; }
	}
}

