using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PairUp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PairUp.Screens
{
	public class TitleScreen(GraphicsDevice graphicsDevice, InputManager inputManager, ScreenManager screenManager, DisplayManager displayManager, ContentManager content) : Screen
	{
		BitmapFont titleFont, startFont, optionsFont;
		InputManager inputManager = inputManager;
		ScreenManager screenManager = screenManager;
		DisplayManager displayManager = displayManager;
		private Button startButton;
		private ContentManager Content = content;
		private GraphicsDevice graphicsDevice = graphicsDevice;

		public override void Initialize()
		{
			titleFont = new BitmapFont("Pair Up", new Vector2(displayManager.InternalResolution.X/2, displayManager.InternalResolution.Y/2 - 25), Color.Orange, Alignment.Center, 3);

			startFont = new BitmapFont("Start Game", Color.Orange, Alignment.Center);

			startButton = new Button(
				startFont,
				graphicsDevice,
				new Vector2(displayManager.InternalResolution.X / 2, 100),
				margin: 4
				);


			
			
		}	

		public override void Update(GameTime gameTime)
		{

				startButton.ButtonPress(inputManager, () =>
				{
					screenManager.SwitchScreen(Game1.PLAYING_SCREEN);
				});
		

			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			titleFont.Draw(Content.Load<Texture2D>, spriteBatch);
			startButton.Draw(spriteBatch, Content.Load<Texture2D>);
		}
	}
}
