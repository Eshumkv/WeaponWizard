using System;
using System.Collections.Generic;
using WeaponWizard.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace WeaponWizard.Interfaces
{
	public interface IRenderSystem : ISystem
	{
		Camera2D Camera { get; set; }

		void Draw (IList<Entity> entities, SpriteBatch spritebatch);
	}
}

