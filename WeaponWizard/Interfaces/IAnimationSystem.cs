using System;
using WeaponWizard.Elements;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Interfaces
{
	public interface IAnimationSystem : ISystem
	{
		void Update (IList<Entity> entities, GameTime gametime);
	}
}

