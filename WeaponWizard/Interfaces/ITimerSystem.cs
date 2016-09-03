using System;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Interfaces
{
	public interface ITimerSystem : ISystem
	{
		void Update (GameTime gametime);

		Timer Register (Timer timer);

		Timer Deregister (Guid guid);
	}
}

