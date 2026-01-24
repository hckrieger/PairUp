using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PairUp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairUp.Screens
{
	public class TitleScreen : Screen
	{
		BitmapFont titleFont, startFont, optionsFont;
		InputManager inputManager;
		ScreenManager screenManager;
		DisplayManager displayManager;
		private ContentManager Content;

		public TitleScreen(InputManager inputManager, ScreenManager screenManager, DisplayManager displayManager, ContentManager content)
		{

			this.inputManager = inputManager;	
			this.screenManager = screenManager;
			this.displayManager = displayManager;
			Content = content;
		}
		public override void Initialize()
		{
			titleFont = new BitmapFont("Pair Up", new Vector2(displayManager.InternalResolution.X/2, displayManager.InternalResolution.Y/2 - 25), Color.Orange, Alignment.Center, 3);

			startFont = new BitmapFont("Start Game", new Vector2(displayManager.InternalResolution.X/2, 100), Color.Orange, Alignment.Center);
			
			


			
			
		}	

		public override void Update(GameTime gameTime)
		{
			Rectangle startButton = new Rectangle((int)startFont.Position.X, (int)startFont.Position.Y, (int)startFont.MeasureString.X, (int)startFont.MeasureString.Y);
			
			if (inputManager.IsMouseOver(startButton) && inputManager.IsMouseButtonJustReleased())
			{
				screenManager.SwitchScreen(Game1.PLAYING_SCREEN);
			}

			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			titleFont.Draw(Content.Load<Texture2D>, spriteBatch);
			startFont.Draw(Content.Load<Texture2D>, spriteBatch);
			//fontManager.DrawText("options", textureCache.GetAsset, spriteBatch);
		}
	}
}
