using System;
using System.IO;

namespace Support.InputOutput
{
	/// <summary>
	/// Provides methods for <see cref="Stream"/> instances manipulation.
	/// </summary>
	static public class StreamHelper
	{
		/// <summary>
		/// Doubles the length of a specified stream while the length is less than a specified desired length. 
		/// </summary>
		/// <param name="stream">The stream which length is to be ensured.</param>
		/// <param name="desiredLength">The desired length of the stream.</param>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="desiredLength"/> is less than 0.</exception>
		static public void EnsureLength(this Stream stream, long desiredLength)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (desiredLength < 0x0)
				throw new ArgumentOutOfRangeException(nameof(desiredLength));
			long length = stream.Length;
			if (length >= desiredLength)
				return;
			if (length == 0x0)
				length = 0x1;
			while (length < desiredLength)
			{
				length <<= 0x1;
				if (length < 0x0)
				{
					length = long.MaxValue;
					break;
				}
			}
			stream.SetLength(length);
		}
	}
}