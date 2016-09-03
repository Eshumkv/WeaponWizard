using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using WeaponWizard.Elements;

namespace WeaponWizard.Interfaces
{
	public interface IInputSystem : ISystem
	{
		KeyboardState KeyboardState { get; }

		KeyboardState PreviousKeyBoardState { get; }

		MouseState MouseState { get; }

		MouseState PreviousMouseState { get; }

		void Update (GameTime gametime);

		bool IsPressed (Keys key);

		bool IsReleased (Keys key);

		bool IsKey (Keys key);

		bool IsMouse (MouseButton button);

		bool IsMousePressed (MouseButton button);

		bool IsMouseReleased (MouseButton button);

		Point MousePosition ();
	}
}

