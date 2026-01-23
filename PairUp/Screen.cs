using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PairUp
{
	public class Screen
	{

		public virtual void Initialize() { }	

		public virtual void OnEnter()
		{

		}

		public virtual void OnExit() { }

		public virtual void Update(GameTime gameTime)
		{

		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{

		}
	}
}
