using System;
using System.Security.Cryptography;

namespace Support
{
	/// <summary>
	/// Provides methods to get random data.
	/// </summary>
	static public class RandomManager
	{
		static private readonly RNGCryptoServiceProvider _provider;

		static RandomManager() => _provider = new RNGCryptoServiceProvider();

		/// <summary>
		/// Puts random bytes to a specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <param name="count">The number of the bytes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		static public void GetBytes(byte[] buffer, int index, int count)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
			_provider.GetBytes(buffer, index, count);
		}
	}
}