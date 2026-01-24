using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

		public bool Visible { get; set; }

		public Rectangle Rectangle
		{
			get => new Rectangle((int)(position.X), (int)(position.Y), texture.Width, texture.Height);
		}
		public Button(BitmapFont font, GraphicsDevice graphicsDevice, Vector2 position, Color color = default, int margin=0)
		{

			texture = Utils.RectangleTexture((int)font.MeasureString.X + (margin), (int)font.MeasureString.Y + (margin), Color.White, graphicsDevice);
			this.position = position;
			font.Position = position + new Vector2(margin/2, margin/2);
			font.TextAlignment = Alignment.Left;
			this.color = (color == default) ? Color.Transparent : color;
			this.font = font;
			Visible = true;

		}

		



		public void Draw(SpriteBatch spriteBatch, Func<string, Texture2D> getAsset)
		{
			if (!Visible)
				return;

			spriteBatch.Draw(texture, position, null, color, 0, Vector2.Zero, 1, SpriteEffects.None, .5f);
			font.Draw(getAsset, spriteBatch);
		}
	}
}
