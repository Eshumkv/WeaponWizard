using System;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using WeaponWizard.Elements.Enums;

namespace WeaponWizard.Elements.Systems
{
	public class InputSystem : IInputSystem
	{
		public KeyboardState KeyboardState { get; private set; }

		public KeyboardState PreviousKeyBoardState { get; private set; }

		public MouseState MouseState { get; private set; }

		public MouseState PreviousMouseState { get; private set; }

		public InputSystem ()
		{
		}

		public void Update (GameTime gametime)
		{
			PreviousKeyBoardState = KeyboardState;
			PreviousMouseState = MouseState;

			KeyboardState = Keyboard.GetState ();
			MouseState = Mouse.GetState ();
		}

		public bool IsPressed (Keys key)
		{
			return KeyboardState.IsKeyDown (key) && PreviousKeyBoardState.IsKeyUp (key);
		}

		public bool IsReleased (Keys key)
		{
			return KeyboardState.IsKeyUp (key) && PreviousKeyBoardState.IsKeyDown (key);
		}

		public bool IsKey (Keys key)
		{
			return KeyboardState.IsKeyDown (key);
		}

		public bool IsMouse (MouseButton button)
		{
			return GetButtonState (button, MouseState) == ButtonState.Pressed;
		}

		public bool IsMousePressed (MouseButton button)
		{
			return GetButtonState (button, MouseState) == ButtonState.Pressed && GetButtonState (button, PreviousMouseState) == ButtonState.Released;
		}

		public bool IsMouseReleased (MouseButton button)
		{
			return GetButtonState (button, MouseState) == ButtonState.Released && GetButtonState (button, PreviousMouseState) == ButtonState.Pressed;
		}

		public Point MousePosition ()
		{
			return MouseState.Position;
		}

		public void HandleMsg (Message msg, object data)
		{
			switch (msg) {
			default:
				break;
			}
		}

		private static ButtonState GetButtonState (MouseButton button, MouseState mstate)
		{
			ButtonState bstate;

			switch (button) {
			case MouseButton.Left:
				bstate = mstate.LeftButton;
				break;
			case MouseButton.Right:
				bstate = mstate.RightButton;
				break;
			case MouseButton.Middle:
				bstate = mstate.RightButton;
				break;
			default:
				throw new Exception ("No buttonstate. (for some reason ...)");
			}

			return bstate;
		}
	}
}

