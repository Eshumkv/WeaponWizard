using System;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Interfaces
{
	/// <summary>
	/// Interface that declares something needs to be updated
	/// </summary>
	public interface IUpdatable
	{
		/// <summary>
		/// Update the specified engine and gametime.
		/// </summary>
		/// <param name="engine">Engine.</param>
		/// <param name="gametime">Gametime.</param>
		void Update (GameEngine engine, GameTime gametime);
	}
}

