using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PairUp.Services
{
	public class DisplayManager
	{
		private GraphicsDeviceManager _graphics;

		public Point WindowSize { get; set; }

		private Point internalResolution;

		private RenderTarget2D renderTarget;
		public RenderTarget2D RenderTarget => renderTarget;

		public Point InternalResolution
		{
			get => internalResolution;
			set
			{
				internalResolution = value;
				ApplyDisplaySettings();
			}
		}

		public void ToggleFullScreen()
		{
			_graphics.ToggleFullScreen();
			ApplyDisplaySettings();
		}

		public Rectangle AdjustedViewport { get; private set; }

		public DisplayManager(GraphicsDeviceManager graphics)
		{
			_graphics = graphics;
		}

		public void SetWindowSize(Point internalResolution, int scale = 1)
		{
			if (renderTarget != null)
				renderTarget.Dispose();



			renderTarget = new RenderTarget2D(_graphics.GraphicsDevice, internalResolution.X, internalResolution.Y);
			WindowSize = new Point(internalResolution.X * scale, internalResolution.Y * scale);




			InternalResolution = new Point(internalResolution.X, internalResolution.Y);

		}

		public void ApplyDisplaySettings()
		{

			Point screenSize;
			if (_graphics.IsFullScreen)
			{
				screenSize = new Point(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
									   GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
			}
			else
			{
				screenSize = new Point(WindowSize.X, WindowSize.Y);
			}

			_graphics.PreferredBackBufferWidth = screenSize.X;
			_graphics.PreferredBackBufferHeight = screenSize.Y;
			_graphics.ApplyChanges();

			AdjustViewportForAspectRatio();
		}

		private void AdjustViewportForAspectRatio()
		{
			float internalAspectRatio = (float)internalResolution.X / internalResolution.Y;
			float screenAspectRatio = (float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight;
			int width, height, x, y;

			if (internalAspectRatio < screenAspectRatio)
			{
				height = _graphics.PreferredBackBufferHeight;
				width = (int)(height * internalAspectRatio);
				y = 0;
				x = (_graphics.PreferredBackBufferWidth - width) / 2;
			}
			else
			{
				width = _graphics.PreferredBackBufferWidth;
				height = (int)(width / internalAspectRatio);
				x = 0;
				y = (_graphics.PreferredBackBufferHeight - height) / 2;
			}

			AdjustedViewport = new Rectangle(x, y, width, height);


		}

		public Vector2 ScreenToViewport(Vector2 rawCoordinate)
		{
			var scaleX = _graphics.PreferredBackBufferWidth / internalResolution.X;
			var scaleY = _graphics.PreferredBackBufferHeight / internalResolution.Y;

			int xPos = (int)((rawCoordinate.X - AdjustedViewport.X) / scaleX);
			int yPos = (int)((rawCoordinate.Y - AdjustedViewport.Y) / scaleY);


			return new Vector2(xPos, yPos);
		}

		public void DrawRenderTarget(SpriteBatch _spriteBatch, Action drawRenderTarget, Matrix? matrix = null)
		{

			_graphics.GraphicsDevice.SetRenderTarget(renderTarget);

			_spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: matrix ?? Matrix.Identity);
			drawRenderTarget?.Invoke();
			_spriteBatch.End();

			_graphics.GraphicsDevice.SetRenderTarget(null);


			_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
			_spriteBatch.Draw(renderTarget, AdjustedViewport, Color.White);
			_spriteBatch.End();
		}
	}
}
