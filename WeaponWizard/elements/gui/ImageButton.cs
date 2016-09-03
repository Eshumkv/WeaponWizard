using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using WeaponWizard.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace WeaponWizard.Elements.Gui
{
	/// <summary>
	/// Image button.
	/// </summary>
	public class ImageButton : UIItem, IRenderable, IUpdatable
	{
		/// <summary>
		/// Value to check whether the button was highlighted the previous frame.
		/// </summary>
		private bool _wasHighlighted;
		
		/// <summary>
		/// The top parts (left, middle, right)
		/// </summary>
		private ImageButtonPart part_tl;
		private ImageButtonPart part_tm;
		private ImageButtonPart part_tr;

		/// <summary>
		/// The middle parts (left, middle, right)
		/// </summary>
		private ImageButtonPart part_ml;
		private ImageButtonPart part_mm;
		private ImageButtonPart part_mr;

		/// <summary>
		/// The bottom parts (left, middle, right)
		/// </summary>
		private ImageButtonPart part_bl;
		private ImageButtonPart part_bm;
		private ImageButtonPart part_br;

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		/// <value>The texture.</value>
		public Texture2D Texture { get; set; }

		/// <summary>
		/// Gets or sets the text (<see cref="Label"/>).
		/// </summary>
		/// <value>The text.</value>
		public Label Text { get; set; }

		/// <summary>
		/// Button clicked handler.
		/// </summary>
		public delegate void ButtonClickedHandler (ImageButton button);

		/// <summary>
		/// Occurs when button clicked.
		/// </summary>
		public event ButtonClickedHandler ButtonClicked;

		/// <summary>
		/// Initializes a new instance of the <see cref="WeaponWizard.Elements.Gui.ImageButton"/> class.
		/// </summary>
		/// <param name="texture">Texture.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		public ImageButton (Texture2D texture, string text, int x, int y, int w, int h)
		{
			OnRectChanged += (item) => Calculate (item);
			OnHeightChanged += (item) => Calculate (item);
			OnWidthChanged += (item) => Calculate (item);

			Texture = texture;
			Text = new Label (text, 0, 0, Color.Black, Fonts.Default);
			Rect = new Rectangle (x, y, w, h);
			_wasHighlighted = false;

			Text.HighlightColor = Color.Black;
		}

		/// <summary>
		/// Draw the specified button.
		/// </summary>
		/// <param name="spritebatch">Spritebatch.</param>
		public void Draw (SpriteBatch spritebatch)
		{
			part_tl.Draw (spritebatch);
			part_tm.Draw (spritebatch);
			part_tr.Draw (spritebatch);

			part_ml.Draw (spritebatch);
			part_mm.Draw (spritebatch);
			part_mr.Draw (spritebatch);

			part_bl.Draw (spritebatch);
			part_bm.Draw (spritebatch);
			part_br.Draw (spritebatch);

			Text.Draw (spritebatch);
		}

		/// <summary>
		/// Update the specified button.
		/// </summary>
		/// <param name="gametime">Gametime.</param>
		public void Update (GameEngine engine, GameTime gametime)
		{
			var isMouseIn = Rect.Contains (engine.Systems.InputSystem.MousePosition ());

			if (isMouseIn) {
				if (engine.Systems.InputSystem.IsMousePressed (MouseButton.Left)) {
					if (ButtonClicked != null) {
						ButtonClicked (this);
					}
				}
			}

			IsHighlighted (isMouseIn);
			Text.IsHighlighted = isMouseIn;
		}

		/// <summary>
		/// Calculate the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		private void Calculate (UIItem item)
		{			
			var t_partw = (int)((Texture.Width / 2) / 3);
			var t_parth = (int)(Texture.Height / 3);

			var t_pw2 = t_partw * 2;
			var t_ph2 = t_parth * 2;

			var p_m = X + t_partw;
			var p_r = X + Width - t_partw;

			var p_mw = Width - t_pw2;

			part_tl = new ImageButtonPart (Texture, new Rectangle (X, Y, t_partw, t_parth), new Rectangle (0, 0, t_partw, t_parth), false);
			part_tm = new ImageButtonPart (Texture, new Rectangle (p_m, Y, p_mw, t_parth), new Rectangle (t_partw, 0, t_partw, t_parth), true);
			part_tr = new ImageButtonPart (Texture, new Rectangle (p_r, Y, t_partw, t_parth), new Rectangle (t_pw2, 0, t_partw, t_parth), false);

			var middleHeight = (Y + Height - t_parth) - (Y + t_parth);

			part_ml = new ImageButtonPart (Texture, new Rectangle (X, Y + t_parth, t_partw, middleHeight), new Rectangle (0, t_parth, t_partw, t_parth), true);
			part_mm = new ImageButtonPart (Texture, new Rectangle (p_m, Y + t_parth, p_mw, middleHeight), new Rectangle (t_partw, t_parth, t_partw, t_parth), true);
			part_mr = new ImageButtonPart (Texture, new Rectangle (p_r, Y + t_parth, t_partw, middleHeight), new Rectangle (t_pw2, t_parth, t_partw, t_parth), true);

			part_bl = new ImageButtonPart (Texture, new Rectangle (X, Y + Height - t_parth, t_partw, t_parth), new Rectangle (0, t_ph2, t_partw, t_parth), false);
			part_bm = new ImageButtonPart (Texture, new Rectangle (p_m, Y + Height - t_parth, p_mw, t_parth), new Rectangle (t_partw, t_ph2, t_partw, t_parth), true);
			part_br = new ImageButtonPart (Texture, new Rectangle (p_r, Y + Height - t_parth, t_partw, t_parth), new Rectangle (t_pw2, t_ph2, t_partw, t_parth), false);

			Text.Middle = Rect.Center;
		}

		/// <summary>
		/// Determines whether this instance is highlighted.
		/// </summary>
		/// <returns><c>true</c> if this instance is highlighted the specified value; otherwise, <c>false</c>.</returns>
		/// <param name="value">If set to <c>true</c> value.</param>
		private bool IsHighlighted (bool value = true)
		{
			if (value == _wasHighlighted)
				return false;

			part_tl.IsHighlighted (value);
			part_tm.IsHighlighted (value);
			part_tr.IsHighlighted (value);

			part_ml.IsHighlighted (value);
			part_mm.IsHighlighted (value);
			part_mr.IsHighlighted (value);

			part_bl.IsHighlighted (value);
			part_bm.IsHighlighted (value);
			part_br.IsHighlighted (value);

			_wasHighlighted = value;

			return value;
		}
	}
}

