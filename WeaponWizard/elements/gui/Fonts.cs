using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WeaponWizard
{
	public static class Fonts
	{
		private static ContentManager _content;

		private static SpriteFont _default = null;

		public static SpriteFont Default {
			get {
				if (_default == null)
					_default = _content.Load<SpriteFont> ("fonts/default");

				return _default;
			}
		}

		public static void SetContentManager (ContentManager content)
		{
			_content = content;
		}
	}
}

