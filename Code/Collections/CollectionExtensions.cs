using System.Collections.Generic;

namespace GA.Collections
{
	public static class CollectionExtensions
	{
		/// <summary>
		/// Swaps the data at indices indexA and indexB.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="indexA"></param>
		/// <param name="indexB"></param>
		public static void Swap<T>(this IList<T> list, int indexA, int indexB)
		{
			if (list == null)
			{
				throw new System.ArgumentNullException(nameof(list));
			}

			T temp = list[indexA];
			list[indexA] = list[indexB];
			list[indexB] = temp;
		}
	}
}