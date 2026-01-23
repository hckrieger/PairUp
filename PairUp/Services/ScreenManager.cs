using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairUp.Services
{


	public class ScreenManager
	{

		private Dictionary<string, Screen> screens = new Dictionary<string, Screen>();

		public Screen? CurrentScreen { get; private set; }


		public void AddScreen(string name, Screen screen)
		{
			if (!screens.ContainsKey(name))
				screens[name] = screen;

			screens[name].Initialize();
		}

		public void SwitchScreen(string name)
		{
			if (screens.ContainsKey(name))
			{
				CurrentScreen?.OnExit();
				CurrentScreen = screens[name];
				CurrentScreen.OnEnter();
			}
				

		}

		public void RemoveScreen(string name)
		{
			if (screens.ContainsKey(name))
				screens.Remove(name);
		}

		public Screen GetScreen(string name)
		{
			if (screens.TryGetValue(name, out Screen screen))
			{
				return screen;
			}

			return null;
		}


		public void Update(GameTime gameTime)
		{
			CurrentScreen?.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			CurrentScreen?.Draw(spriteBatch);
		}
	}
}
