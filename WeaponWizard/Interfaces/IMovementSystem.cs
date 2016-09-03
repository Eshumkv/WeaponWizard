using System;
using Microsoft.Xna.Framework;
using WeaponWizard.Elements;
using System.Collections.Generic;

namespace WeaponWizard.Interfaces
{
	public interface IMovementSystem : ISystem
	{
		void Update (IList<Entity> entities, GameTime gametime);
	}
}

