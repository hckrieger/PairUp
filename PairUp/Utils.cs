using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairUp
{
	public static class Utils
	{


		/// <summary>
		/// Creates a solid color rectangle texture.
		/// </summary>
		/// <param name="width">The width of the rectangle.</param>
		/// <param name="height">The height of the rectangle.</param>
		/// <param name="color">The color of the rectangle.</param>
		/// <param name="graphicsDevice">The graphics device to create the texture on.</param>
		/// <returns>A Texture2D representing the solid color rectangle.</returns>
		public static Texture2D RectangleTexture(int width, int height, Color color, GraphicsDevice graphicsDevice)
		{
			Texture2D texture = new Texture2D(graphicsDevice, width, height);
			Color[] colorData = new Color[width * height];
			for (int i = 0; i < colorData.Length; i++)
			{
				colorData[i] = color;
			}
			texture.SetData(colorData);
			return texture;
		}


		/// <summary>
		/// Converts two-dimensional coordinates to a zero-based linear index for a row-major ordered grid.
		/// </summary>
		/// <remarks>This method assumes the grid is stored in row-major order, where each row is contiguous in
		/// memory. The caller is responsible for ensuring that the provided coordinates are within the bounds of the
		/// grid.</remarks>
		/// <param name="x">The zero-based column index within the row. Must be greater than or equal to 0 and less than the value of width.</param>
		/// <param name="y">The zero-based row index within the grid. Must be greater than or equal to 0.</param>
		/// <param name="width">The total number of columns in each row of the grid. Must be greater than 0.</param>
		/// <returns>A zero-based linear index corresponding to the specified (x, y) coordinates in a row-major ordered grid.</returns>
		public static int CoordinateToIndex(int x, int y, int width)
		{
			return y * width + x;
		}

		/// <summary>
		/// Converts a linear index into two-dimensional (x, y) coordinates based on the specified row width.
		/// </summary>
		/// <remarks>This method is commonly used to map a one-dimensional array index to two-dimensional grid
		/// coordinates, where x represents the column and y represents the row.</remarks>
		/// <param name="index">The zero-based linear index to convert. Must be greater than or equal to 0.</param>
		/// <param name="width">The number of columns in each row. Must be greater than 0.</param>
		/// <returns>A tuple containing the x (column) and y (row) coordinates corresponding to the specified index.</returns>
		public static Point IndexToCoordinate(int index, int width)
		{
			int y = index / width;
			int x = index % width;
			return new Point(x, y);
		}
	}
}
