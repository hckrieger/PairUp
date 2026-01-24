using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PairUp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairUp
{
	public class Button
	{
		private Texture2D texture;
		private BitmapFont font;
		private Vector2 position;
		private Color color;
		private Vector2 origin;
		public bool Visible { get; set; }

		public Rectangle Rectangle
		{
			get
			{

				return new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), texture.Width, texture.Height);
			}
		}

		public Button(BitmapFont font, GraphicsDevice graphicsDevice, Vector2 position, Color color = default, int margin = 0)
		{

			texture = Utils.RectangleTexture((int)font.MeasureString.X + (margin), (int)font.MeasureString.Y + (margin), Color.White, graphicsDevice);
			this.position = position;
			switch (font.TextAlignment)
			{

				case Alignment.Center:

					origin = new Vector2(texture.Width / 2, texture.Height / 2);
					font.Position = position;
					break;

				case Alignment.Right:
					origin = new Vector2(texture.Width, 0);
					font.Position = position + new Vector2(-margin / 2, margin / 2);
					break;

				default:
					font.Position = position + new Vector2(margin / 2, margin / 2);
					origin = Vector2.Zero;
					break;
			}

			this.color = (color == default) ? Color.Transparent : color;
			this.font = font;
			Visible = true;
		}

		
		public void ButtonPress(InputManager inputManager, Action onButtonClick)
		{
			if (!Visible) return;
			if (inputManager.IsMousePressedOver(Rectangle))
			{
				onButtonClick.Invoke();	
			}
		}


		public void Draw(SpriteBatch spriteBatch, Func<string, Texture2D> getAsset)
		{
			if (!Visible)
				return;

			spriteBatch.Draw(texture, position, null, color, 0, origin, 1, SpriteEffects.None, .5f);
			font.Draw(getAsset, spriteBatch);
		}
	}
}
