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
	public class TitleScreen(GraphicsDevice graphicsDevice, InputManager inputManager, ScreenManager screenManager, DisplayManager displayManager, ContentManager content, Action exit) : Screen
	{
		BitmapFont titleFont, startFont, optionsFont;
		InputManager inputManager = inputManager;
		ScreenManager screenManager = screenManager;
		DisplayManager displayManager = displayManager;
		private TextButton startButton, exitButton;
		private ContentManager Content = content;
		private GraphicsDevice graphicsDevice = graphicsDevice;

		private Action exit = exit;

		public override void Initialize()
		{
			titleFont = new BitmapFont("Pair Up", new Vector2(displayManager.InternalResolution.X/2, displayManager.InternalResolution.Y/2 - 25), Color.Orange, Alignment.Center, 3);

			startFont = new BitmapFont("Start Game", Color.Orange, Alignment.Center);

			var xPos = displayManager.InternalResolution.X / 2;

			startButton = new TextButton(
				startFont,
				graphicsDevice,
				new Vector2(xPos, 100),
				margin: 4
				);

			exitButton = new TextButton(
				new BitmapFont("Exit", Color.Orange, Alignment.Center),
				graphicsDevice,
				new Vector2(xPos, 124),
				margin: 4
				);
			
		
		}	

		public override void Update(GameTime gameTime)
		{

			startButton.ButtonPress(inputManager, () =>
			{
				screenManager.SwitchScreen(Game1.PLAYING_SCREEN);
			});

			exitButton.ButtonPress(inputManager, () => exit.Invoke());
			
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			titleFont.Draw(Content.Load<Texture2D>, spriteBatch);
			startButton.Draw(spriteBatch, Content.Load<Texture2D>);
			exitButton.Draw(spriteBatch, Content.Load<Texture2D>);
		}
	}
}
