using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PairUp.Screens;
using PairUp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace PairUp
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;



		private DisplayManager displayManager;
		private InputManager inputManager;

		private ScreenManager screenManager;

		public const string PLAYING_SCREEN = "PlayingScene";
		public const string TITLE_SCREEN = "TitleScreen";


		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = false;


		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			displayManager = new DisplayManager(_graphics);
			inputManager = new InputManager(displayManager);
			screenManager = new ScreenManager();

			_graphics.HardwareModeSwitch = false;
			displayManager.SetWindowSize(new Point(320, 180), 3);



			screenManager.AddScreen(TITLE_SCREEN, new TitleScreen(inputManager, screenManager, displayManager, Content));
			screenManager.SwitchScreen(TITLE_SCREEN);

			screenManager.AddScreen(PLAYING_SCREEN, new PlayingScreen(GraphicsDevice, inputManager, Difficulty.Easy, Content));

			base.Initialize();
		}








		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here

			inputManager.Update();

			if (inputManager.IsKeyPressed(Keys.F))
			{
				displayManager.ToggleFullScreen();

			}
			

			//Debug.WriteLine(fontManager.dataCache.Count);
			screenManager.Update(gameTime);
			//cardSystem.Update(gameTime);

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			displayManager.DrawRenderTarget(_spriteBatch, () =>
			{
				//cardSystem.Draw(_spriteBatch);

				//fontManager.Draw(Content.Load<Texture2D>, _spriteBatch);
				screenManager.Draw(_spriteBatch);
				_spriteBatch.Draw(Content.Load<Texture2D>("cursor"), inputManager.MousePosition, Color.White);
			});


			base.Draw(gameTime);
		}


	}
}
