using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PairUp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace PairUp.Screens
{
	public enum Difficulty
	{
		Easy,
		Normal,
		Intermediate,
		Hard,
		Extreme
	}

	public enum GameState
	{
		Playing,
		GameEnded
	}

	public struct Card
	{
		public Color Color;

		public Color StateColor { private get; set; }

		public Point Size;

		public Vector2 Position;

		private bool isFlipped;
		public bool IsFlipped
		{
			get => isFlipped;
			set
			{
				if (value == true)
				{
					Color = StateColor;
				}
				else
				{
					Color = Color.Gray;
				}
				isFlipped = value;
			}
		}

		private bool isMatched;

		public Rectangle LocalRectangle;

		public Difficulty Difficulty { get; set; }

		public Rectangle Rectangle => new Rectangle((int)Position.X + LocalRectangle.X, (int)Position.Y + LocalRectangle.Y, LocalRectangle.Width, LocalRectangle.Height);

		public bool IsMatched
		{
			get => isMatched;
			set
			{
				IsFlipped = true;
				isMatched = value;

			}
		}
	}
	public class PlayingScreen : Screen
	{


		private InputManager inputManager;

		private Button readyButton;


		private Color[] matchColors =
		[
			Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange, Color.Purple,
			Color.Cyan, Color.Magenta, Color.Lime, Color.Teal, Color.Pink, Color.Brown,
			Color.Gold, Color.Coral, Color.SkyBlue, Color.Chartreuse, Color.DarkRed, Color.DarkBlue
		];


		public GameState currentGameState = GameState.Playing;

		public Difficulty CurrentDifficulty { get; set; }

		public Texture2D Texture { get; set; }

		public Card[,] CardGrid { get; set; }

		private GraphicsDevice graphicsDevice;

		private List<Card> selectedCards = new List<Card>();

		private List<Color> colorList = new List<Color>();


		private float timer;

		private int tries, intermediateTries = 0;

		private Texture2D button;

		private int unmatchedPairs;
		int score;
		private BitmapFont scoreFont, status, levelDisplay, readyFont;

		private ContentManager Content;
		public PlayingScreen(GraphicsDevice graphicsDevice, InputManager inputManager, Difficulty difficulty, ContentManager content)
		{
			this.graphicsDevice = graphicsDevice;
			this.inputManager = inputManager;
			CurrentDifficulty = difficulty;

			Content = content;
		}

		public override void Initialize()
		{
			base.Initialize();

			scoreFont = new BitmapFont("Score:\n\n0", new Vector2(228, 10), Color.White);

			status = new BitmapFont("Heads up", new Vector2(228, 64), Color.White);
			status.Visible = false;

			levelDisplay = new BitmapFont($"Level: {(int)CurrentDifficulty + 1}/5", new Vector2(228,48), Color.White);


			readyButton = new Button(new BitmapFont($"Ready", Color.White), graphicsDevice, new Vector2(230, 112), Color.Gray, 8);
			readyButton.Visible = false;

			button = Utils.RectangleTexture(56, 16, Color.Gray, graphicsDevice);
			//CurrentDifficulty = Difficulty.Extreme;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			SetCardGrid();
		}


		public void SetCardGrid()
		{
			int margin = 8;
			Point spacing;

			Color[] selectedColors;
			Rectangle localRectangle;
			

			switch (CurrentDifficulty)
			{
				case Difficulty.Easy:
					score = 0;
					CardGrid = new Card[4, 4];
					spacing = new Point(7, 7);
					Texture = Utils.RectangleTexture(48, 36, Color.White, graphicsDevice);
					selectedColors = new Color[16];
					localRectangle = new Rectangle(0, 0, 48, 36);
					for (int i = 0; i < 2; i++)
						matchColors[..8].CopyTo(selectedColors, i * 8);
					break;
				case Difficulty.Normal:
					CardGrid = new Card[5, 4];
					spacing = new Point(4, 4);
					margin = 6;
					Texture = Utils.RectangleTexture(40, 39, Color.White, graphicsDevice); // width > height
					localRectangle = new Rectangle(0, 0, 40, 39);
					selectedColors = new Color[20];
					for (int i = 0; i < 2; i++)
						matchColors[..10].CopyTo(selectedColors, i * 10);
					break;
				case Difficulty.Intermediate:
					CardGrid = new Card[6, 4];
					spacing = new Point(5, 6);
					margin = 5;
					Texture = Utils.RectangleTexture(32, 38, Color.White, graphicsDevice); // adjusted width > height
					localRectangle = new Rectangle(0, 0, 32, 38);
					selectedColors = new Color[24];
					for (int i = 0; i < 2; i++)
						matchColors[..12].CopyTo(selectedColors, i * 12);
					break;
				case Difficulty.Hard:
					CardGrid = new Card[6, 5];
					spacing = new Point(4, 5);
					margin = 5;
					Texture = Utils.RectangleTexture(33, 30, Color.White, graphicsDevice); // adjusted height < width
					localRectangle = new Rectangle(0, 0, 33, 30);
					selectedColors = new Color[30];
					for (int i = 0; i < 2; i++)
						matchColors[..15].CopyTo(selectedColors, i * 15);
					break;
				default: //Difficulty.Extreme
					CardGrid = new Card[6, 6];
					spacing = new Point(4, 4);
					Texture = Utils.RectangleTexture(32, 24, Color.White, graphicsDevice);
					selectedColors = new Color[36];
					localRectangle = new Rectangle(0, 0, 32, 24);
					for (int i = 0; i < 2; i++)
						matchColors[..18].CopyTo(selectedColors, i * 18);
					break;



			}
			intermediateTries = 0;
			unmatchedPairs = selectedColors.Length / 2;
			colorList = selectedColors.ToList();
			colorList.Shuffle();


			GridLoop((x, y) =>
			{


				var color = colorList[Utils.CoordinateToIndex(x, y, CardGrid.GetLength(0))];


				CardGrid[x, y] = new Card();

				ref var card = ref CardGrid[x, y];
				card.IsFlipped = false;
				card.Size = new Point(Texture.Width, Texture.Height);
				card.StateColor = color;
				card.LocalRectangle = localRectangle;

				var position = new Vector2(margin + (x * (card.Size.X + spacing.X)), margin + (y * (card.Size.Y + spacing.Y)));
				card.Position = position;
			});


		}

		private void GridLoop(Action<int, int> action)
		{
			for (int y = 0; y < CardGrid.GetLength(1); y++)
			{
				for (int x = 0; x < CardGrid.GetLength(0); x++)
				{
					action(x, y);
				}
			}
		}

		public override void Update(GameTime gameTime)
		{

			if (selectedCards.Count < 2)
			{

				GridLoop((x, y) =>
				{

					if (inputManager.IsMouseOver(CardGrid[x, y].Rectangle) && inputManager.IsMouseButtonPressed() && !CardGrid[x, y].IsFlipped)
					{
						ref var card = ref CardGrid[x, y];
						card.IsFlipped = true;

						if (selectedCards.Contains(card))
							return;
						selectedCards.Add(card);
					}
				});



			}
			else
			{


				bool isMatch = selectedCards[0].Color == selectedCards[1].Color;

				if (isMatch)
				{
					for (int i = 0; i < selectedCards.Count; i++)
					{
						GridLoop((x, y) =>
						{
							ref var card = ref CardGrid[x, y];
							if (card.Color == selectedCards[i].Color && !card.IsMatched)
							{
								card.IsMatched = true;

							}
						});
					}
					tries++;


					score += (75 * ((int)CurrentDifficulty + 1)) * (unmatchedPairs - Math.Clamp(intermediateTries, 0, unmatchedPairs - 1));

					intermediateTries = 0;
					unmatchedPairs--;

					scoreFont.Text = $"Score:\n\n{score}";
					selectedCards.Clear();
				}
				else
				{
					timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

					if (timer >= .66f)
					{
						timer = 0f;

						GridLoop((x, y) =>
						{
							ref var card = ref CardGrid[x, y];
							if (!card.IsMatched)
							{
								card.IsFlipped = false;
							}
						});


						tries++;
						intermediateTries++;
						//fontManager.GetFont("score").Text = $"Tries: {tries}";
						selectedCards.Clear();

					}

				}


			}


			int flippedCount = 0;

			GridLoop((x, y) =>
			{
				if (CardGrid[x, y].IsFlipped)
					flippedCount++;

				if (flippedCount == CardGrid.GetLength(0) * CardGrid.GetLength(1))
					currentGameState = GameState.GameEnded;



			});

			if (currentGameState == GameState.GameEnded)
			{
				status.Visible = true;
				readyButton.Visible = true;

				if (CurrentDifficulty == Difficulty.Extreme)
					status.Text = "You Win!\n\nPlay again?";
				else
					status.Text = "You passed\nthis stage!\n\nReady for\nthe next?";
				
				if (inputManager.IsMousePressedOver(readyButton.Rectangle))
				{
					currentGameState = GameState.Playing;

					readyButton.Visible = false;
					

					tries = 0;
					//fontManager.GetFont("score").Text = $"Tries: {tries}";
					status.Visible = false;


						

					CurrentDifficulty = (CurrentDifficulty != Difficulty.Extreme) ? CurrentDifficulty + 1 : Difficulty.Easy;
					if (CurrentDifficulty == Difficulty.Easy)
					{
						score = 0;
						scoreFont.Text = $"Score:\n\n{score}";
					}
					levelDisplay.Text = $"Level: {(int)CurrentDifficulty + 1}/5";

					

					SetCardGrid();

				}
			}


		}




		public override void Draw(SpriteBatch spriteBatch)
		{
			GridLoop((x, y) => {
				spriteBatch.Draw(Texture, CardGrid[x, y].Position, CardGrid[x, y].Color);
			});

			scoreFont.Draw(Content.Load<Texture2D>, spriteBatch);
			status.Draw(Content.Load<Texture2D>, spriteBatch);
			levelDisplay.Draw(Content.Load<Texture2D>, spriteBatch);
			readyButton.Draw(spriteBatch, Content.Load<Texture2D>);

		}

	}

}

