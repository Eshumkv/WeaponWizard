using System;
using Microsoft.Xna.Framework;

namespace WeaponWizard
{
	public class Timer
	{
		public Guid ID { get; set; }

		public GameTime StartTime { get; set; }

		public TimeSpan Time { get; set; }

		public bool DestroyAtEnd { get; set; }

		public Action Callback { get; set; }

		public Timer ()
		{
		}
	}
}

