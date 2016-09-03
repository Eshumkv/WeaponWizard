using System;
using WeaponWizard.Elements;
using System.Collections.Generic;
using WeaponWizard.Elements.Enums;
using WeaponWizard.Interfaces;
using WeaponWizard.Elements.Systems;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Elements.Systems
{
	public class SystemManager
	{

		public Dictionary<Message, Dictionary<string, Action<object>>> _messageQueue;

		#region Systems

		// All the systems needed
		// REMINDER: Add them to the SendMsg function
		public IRenderSystem RenderSystem { get; set; }

		public IInputSystem InputSystem { get; set; }

		private IAnimationSystem AnimationSystem { get; set; }

		private ITimerSystem TimerSystem { get; set; }

		private IMovementSystem MoveSystem { get; set; }

		#endregion

		public SystemManager ()
		{
			_messageQueue = new Dictionary<Message, Dictionary<string, Action<object>>> ();
		}

		public void LoadSystems (GameEngine engine)
		{

			RenderSystem = new BasicRenderSystem ();
			InputSystem = new InputSystem ();
			AnimationSystem = new AnimationSystem (engine);
			TimerSystem = new TimerSystem (engine);
			MoveSystem = new MovementSystem (engine);
		}

		public void Draw (SpriteBatch spritebatch, IDictionary<string, Entity> entities)
		{
			RenderSystem.Draw (entities.Values.ToList (), spritebatch);
		}

		public void UpdateBefore (GameTime gameTime, IDictionary<string, Entity> entities)
		{
			InputSystem.Update (gameTime);
			MoveSystem.Update (entities.Values.ToList (), gameTime);
			RenderSystem.Camera.Update ();
		}

		public void UpdateAfter (GameTime gameTime, IDictionary<string, Entity> entities)
		{
			TimerSystem.Update (gameTime);
			AnimationSystem.Update (entities.Values.ToList (), gameTime);
		}

		/// <summary>
		/// Sends the message.
		/// </summary>
		/// <param name="msg">Message.</param>
		/// <param name="data">Data.</param>
		public void SendMsg (Message msg, object data)
		{
			if (!_messageQueue.ContainsKey (msg))
				return;

			try {
				foreach (var msgDict in _messageQueue.Values) {
					foreach (var func in msgDict.Values) {
						func (data);
					}
				}
			} catch (KeyNotFoundException) {
				; // NOTHING
			}
		}

		/// <summary>
		/// Register the specified function to a message, with the given id (if any).
		/// </summary>
		/// <param name="msg">Message.</param>
		/// <param name="function">Function.</param>
		/// <param name="id">Identifier.</param>
		public string Register (Message msg, Action<object> function, string id = null)
		{
			if (!_messageQueue.ContainsKey (msg) || _messageQueue [msg] == null)
				_messageQueue [msg] = new Dictionary<string, Action<object>> ();

			if (string.IsNullOrWhiteSpace (id)) {
				id = Guid.NewGuid ().ToString ();
			}

			_messageQueue [msg].Add (id, function);

			return id;
		}

		/// <summary>
		/// Unregister the specified msg and id.
		/// </summary>
		/// <param name="msg">Message.</param>
		/// <param name="id">Identifier.</param>
		public void Unregister (Message msg, string id)
		{
			try {
				_messageQueue [msg].Remove (id);
			} catch (KeyNotFoundException) {
				; // NOTHING
			}
		}
	}
}

