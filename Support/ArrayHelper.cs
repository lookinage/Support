using System;

namespace Support
{
	/// <summary>
	/// Provides methods for arrays manipulation.
	/// </summary>
	static public class ArrayHelper
	{
		/// <summary>
		/// Initializes a specified array by the desired length if the array is <see langword="null"/>; otherwise, doubles the length of the array while the length is less than a specified desired length.
		/// </summary>
		/// <param name="array">The array which length is to be ensured.</param>
		/// <param name="desiredLength">The desired length of the array.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="desiredLength"/> is less than 0.</exception>
		static public bool EnsureLength<T>(ref T[] array, long desiredLength)
		{
			if (desiredLength < 0x0)
				throw new ArgumentOutOfRangeException(nameof(desiredLength));
			long currentLength;
			if (array == null || (currentLength = array.LongLength) == 0x0 && desiredLength > 0x0)
			{
				array = new T[desiredLength];
				return true;
			}
			if (currentLength >= desiredLength)
				return false;
			long newLength = currentLength;
			do
			{
				newLength <<= 0x1;
				if (newLength < 0x0)
				{
					newLength = long.MaxValue;
					break;
				}
			}
			while (newLength < desiredLength);
			T[] newArray = new T[newLength];
			Array.Copy(array, newArray, currentLength);
			array = newArray;
			return true;
		}
	}
}