using System;
using Microsoft.Xna.Framework;

namespace WeaponWizard.Elements.Gui
{
	/// <summary>
	/// User interface item.
	/// </summary>
	public abstract class UIItem
	{
		/// <summary>
		/// The width.
		/// </summary>
		private int _width;

		/// <summary>
		/// The height.
		/// </summary>
		private int _height;

		/// <summary>
		/// On width changed handler.
		/// </summary>
		public delegate void OnWidthChangedHandler (UIItem item);

		/// <summary>
		/// On height changed handler.
		/// </summary>
		public delegate void OnHeightChangedHandler (UIItem item);

		/// <summary>
		/// On rect changed handler.
		/// </summary>
		public delegate void OnRectChangedHandler (UIItem item);

		/// <summary>
		/// Occurs when on width changed.
		/// </summary>
		public event OnWidthChangedHandler OnWidthChanged;

		/// <summary>
		/// Occurs when on height changed.
		/// </summary>
		public event OnHeightChangedHandler OnHeightChanged;

		/// <summary>
		/// Occurs when on rect changed.
		/// </summary>
		public event OnRectChangedHandler OnRectChanged;

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		/// <value>The width.</value>
		public int Width {
			get {
				return _width;
			}
			set {
				_width = value;

				if (OnWidthChanged != null)
					OnWidthChanged (this);
			}
		}

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		/// <value>The height.</value>
		public int Height {
			get {
				return _height;
			}
			set {
				_height = value;

				if (OnHeightChanged != null)
					OnHeightChanged (this);
			}
		}

		/// <summary>
		/// Gets or sets the x.
		/// </summary>
		/// <value>The x.</value>
		public int X { get; set; }

		/// <summary>
		/// Gets or sets the y.
		/// </summary>
		/// <value>The y.</value>
		public int Y { get; set; }

		/// <summary>
		/// Gets or sets the rect.
		/// </summary>
		/// <value>The rect.</value>
		public Rectangle Rect {
			get {
				return new Rectangle (X, Y, Width, Height);
			}
			set {
				X = value.X;
				Y = value.Y;
				_width = value.Width;
				_height = value.Height;

				if (OnRectChanged != null)
					OnRectChanged (this);
			}
		}

		/// <summary>
		/// Gets the position as a <see cref="Vector2"/>.
		/// </summary>
		/// <value>The position.</value>
		public Vector2 Position {
			get {
				return new Vector2 (X, Y);
			}
		}

		/// <summary>
		/// Gets or sets the middle.
		/// </summary>
		/// <value>The middle.</value>
		public Point Middle {
			get {
				return Rect.Center;
			}
			set {
				Rect = new Rectangle (value.X - (Width / 2), value.Y - (Height / 2), Width, Height);
			}
		}
	}
}

