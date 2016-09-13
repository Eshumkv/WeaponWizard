using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using WeaponWizard.Elements.Animation;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Graphics;

namespace WeaponWizard
{
	public class AnimationJsonFile
	{
		public string Id { get; set; }

		public string ImageName { get; set; }

		public string Image { get; set; }

		public Point Size { get; set; }

		public int Time { get; set; }

		public Dictionary<string, List<int>> Animations { get; set; }

		[JsonIgnore]
		private Texture2D _texture = null;
		[JsonIgnore]
		private int _part_width;
		[JsonIgnore]
		private int _part_height;

		public AnimationJsonFile ()
		{
		}

		public List<FrameInfo> Load (GameEngine engine, string key)
		{
			var list = new List<FrameInfo> ();

			if (_texture == null) {
				_texture = engine.Textures [ImageName];

				_part_width = _texture.Width / Size.X;
				_part_height = _texture.Height / Size.Y;
			}

			foreach (var frame in Animations[key]) {
				var counter = frame;
				var y = 0;

				while (counter >= Size.X) {
					y += _part_height;
					counter -= Size.X;
				}

				list.Add (new FrameInfo (
					new Rectangle (counter * _part_width, y, _part_width, _part_height), 
					Time
				));
			}

			return list;
		}
	}
}

