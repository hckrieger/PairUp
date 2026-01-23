using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairUp
{
	public enum Alignment { Left, Center, Right };
	public class BitmapFont
	{
		public BitmapFont(string text, string filePath, Vector2 rawPosition, Color color, Alignment alignment = Alignment.Left, int scale = 1, bool visible = true)
		{
			
			Position = rawPosition;
			Color = color;
			this.text = text;
			FilePath = filePath;
			Scale = scale;
			Visible = visible;
			textAlignment = alignment;
			SetTextAlignment();
		}

		private Alignment textAlignment;
		public Alignment TextAlignment
		{
			get => textAlignment;
			set
			{
				if (value == textAlignment)
					return;

				textAlignment = value;
				SetTextAlignment();
			}
		}

		private string text;

		public string Text
		{
			get => text;
			set
			{
				if (value == text) return;
				text = value;
				SetTextAlignment();
			}
		}

		private string renderedText;
		public string FilePath;

		private Vector2 position;

		public Vector2 Offset;
		public Vector2 Position
		{
			get => position + Offset;
			set => position = value;
		}
		public Point Size = new Point(8, 8);

		public Color Color;
		public int Scale;
		public bool Visible;

		private int LongestHorizontalLine(out string[] lines)
		{
			lines = renderedText.Contains("\n") ? renderedText.Split("\n") : [renderedText];

			int longestLine = 0;
			foreach (var line in lines)
			{
				if (line.Length > longestLine)
					longestLine = line.Length;
			}

			return Size.X * longestLine * Scale;
		}

		public Vector2 MeasureString
		{
			get
			{


				string[] lines;

				var x = LongestHorizontalLine(out lines);

				var y = Size.Y * Scale * lines.Length;

				return new Vector2(x, y);
			}
		}

		public void SetTextAlignment()
		{
			renderedText = text ?? string.Empty;

			string[] lines;

			int width = LongestHorizontalLine(out lines);
			int height = Size.Y * lines.Length * Scale;
			switch (TextAlignment)
			{
				case Alignment.Right:
					Offset = new Vector2(-width, 0);
					for (int i = 0; i < lines.Length; i++)
					{
						int spaces = width / Scale / Size.X;
						lines[i] = lines[i].PadLeft(spaces);
					}
					renderedText = string.Join("\n", lines);
					break;
				case Alignment.Center:
					Offset = new Vector2(-width / 2, -height / 2);
					for (int i = 0; i < lines.Length; i++)
					{
						int spaces = ((width / Scale / Size.X - lines[i].Length) / 2) + lines[i].Length;
						lines[i] = lines[i].PadLeft(spaces);
					}
					renderedText = string.Join("\n", lines);
					break;
				default:
					Offset = Vector2.Zero;
					break;
			}

		}

		public void Draw(Func<string, Texture2D> getAsset, SpriteBatch spriteBatch)
		{



			Texture2D texture = getAsset(FilePath);

			int x = 0, y = 0;

			int width = texture.Width / Size.X;
			for (int i = 0; i < renderedText.Length; i++)
			{
				if (renderedText[i] == '\n')
				{
					y += 1;
					x = 0;
					continue;
				}

				Point textureCoordinate = Utils.IndexToCoordinate((renderedText[i] - 32), width);
				Rectangle source = new Rectangle(textureCoordinate.X * Size.X, textureCoordinate.Y * Size.Y, Size.X, Size.Y);
				Vector2 letterPosition = Position + new Vector2(Size.X * Scale * x, Size.Y * Scale * y);

				if (Visible)
					spriteBatch.Draw(texture, letterPosition, source, Color, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
				x++;
			}

		}
	}

}
