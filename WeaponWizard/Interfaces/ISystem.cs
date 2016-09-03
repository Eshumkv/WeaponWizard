using System;
using WeaponWizard.Elements.Enums;

namespace WeaponWizard.Interfaces
{
	public interface ISystem
	{
		void HandleMsg (Message msg, object data);
	}
}

