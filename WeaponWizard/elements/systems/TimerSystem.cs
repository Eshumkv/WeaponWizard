using System;
using Microsoft.Xna.Framework;
using WeaponWizard.Elements.Enums;
using System.Collections.Generic;
using System.Linq;
using WeaponWizard.Interfaces;

namespace WeaponWizard.Elements.Systems
{
	public class TimerSystem : ITimerSystem
	{
		private GameEngine _engine;
		private List<Timer> _timers;
		private GameTime _lastGameTime;

		public TimerSystem (GameEngine ge)
		{
			_lastGameTime = new GameTime ();
			_engine = ge;
			_timers = new List<Timer> ();
		}

		public void Update (GameTime gametime)
		{
			foreach (var timer in _timers.Reverse<Timer>()) {
				if (timer.StartTime.TotalGameTime.Add (timer.Time) <= gametime.TotalGameTime) {
					timer.Callback ();

					if (timer.DestroyAtEnd) {
						_timers.Remove (timer);
					}
				}
			}

			_lastGameTime = gametime;
		}

		public Timer Register (Timer timer)
		{
			timer.ID = Guid.NewGuid ();
			timer.StartTime = new GameTime (_lastGameTime.TotalGameTime, _lastGameTime.ElapsedGameTime);
			_engine.Systems.SendMsg (Message.RESPONSE_Start_Timer, timer);

			_timers.Add (timer);
			
			return timer;
		}

		public Timer Deregister (Guid guid)
		{
			var timer = _timers.Where (x => x.ID == guid).Single ();

			_timers.Remove (timer);

			return timer;
		}

		public void HandleMsg (Message msg, object data)
		{			
			switch (msg) {
			case Message.Start_Timer:
				Register (data as Timer);
				break;

			case Message.End_Timer:
				Deregister ((Guid)data);
				break;

			default:
				break;
			}
		}
	}
}

