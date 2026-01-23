using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairUp
{
	public static class Extensions
	{

		/// <summary>
		/// Randomly reorders the elements in the specified list using the provided random number generator.
		/// </summary>
		/// <remarks>This method modifies the input list directly and does not create a new list. The shuffling
		/// algorithm ensures that each possible permutation of the list is equally likely, provided that the random number
		/// generator is unbiased.</remarks>
		/// <typeparam name="T">The type of elements in the list to shuffle.</typeparam>
		/// <param name="list">The list whose elements will be shuffled in place. Cannot be null.</param>
		/// <param name="rng">The random number generator to use for shuffling. Cannot be null.</param>
		/// 
		public static Random random = new Random();
		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = random.Next(0, n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

	}
}
