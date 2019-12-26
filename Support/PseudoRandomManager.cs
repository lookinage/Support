using System;

namespace Support
{
	/// <summary>
	/// Provides methods to get pseudo-random data.
	/// </summary>
	static public class PseudoRandomManager
	{
		[ThreadStatic]
		static private bool _initialized;
		[ThreadStatic]
		static private int[] _seeds;
		[ThreadStatic]
		static private int _inext;
		[ThreadStatic]
		static private int _inextp;

		static private void Initialize()
		{
			_seeds = new int[0x38];
			int seed = Environment.TickCount;
			int subtraction = seed == int.MinValue ? int.MaxValue : Math.Abs(seed);
			int mj = 0x9A4EC86 - subtraction;
			_seeds[0x37] = mj;
			int mk = 0x1;
			for (int i = 0x1; i < 0x37; i++)
			{
				int ii = 0x15 * i % 0x37;
				_seeds[ii] = mk;
				mk = mj - mk;
				if (mk < 0x0)
					mk += int.MaxValue;
				mj = _seeds[ii];
			}
			for (int k = 0x1; k < 0x5; k++)
				for (int i = 0x1; i < 0x38; i++)
				{
					_seeds[i] -= _seeds[0x1 + (i + 0x1E) % 0x37];
					if (_seeds[i] < 0x0)
						_seeds[i] += int.MaxValue;
				}
			_inext = 0x0;
			_inextp = 0x15;
		}
		static private int GetNonPositiveInt32Internal(int min) => GetNonPositiveInt32() % (min - 0x1);
		static private int GetNonPositiveInt32Internal(int min, int max) => max + GetNonPositiveInt32Internal(min - max);
		/// <summary>
		/// Returns a pseudo-random <see cref="int"/>.
		/// </summary>
		/// <returns>A pseudo-random <see cref="int"/>.</returns>
		static public int GetInt32()
		{
			if (!_initialized)
			{
				_initialized = true;
				Initialize();
			}
			if (++_inext >= 0x38)
				_inext = 0x1;
			if (++_inextp >= 0x38)
				_inextp = 0x1;
			int result = _seeds[_inext] - _seeds[_inextp];
			_seeds[_inext] = result < 0x0 ? result + int.MaxValue : result == int.MaxValue ? int.MaxValue - 0x1 : result;
			return result;
		}
		/// <summary>
		/// Returns a pseudo-random <see cref="int"/> that is within a specified range.
		/// </summary>
		/// <param name="min">The minimum value for the number to be generated.</param>
		/// <param name="max">The maximum value for the number to be generated.</param>
		/// <returns>A number that is greater than or equal to <paramref name="min"/> and less than or equal to <paramref name="max"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> equals to <see cref="int.MinValue"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is less than <paramref name="min"/>.</exception>
		static public int GetInt32(int min, int max)
		{
			if (min == int.MinValue)
				throw new ArgumentOutOfRangeException(nameof(min));
			if (max < min)
				throw new ArgumentOutOfRangeException(nameof(max));
			return (int)(max - ((long)int.MaxValue + GetInt32()) % (-0x1L + min - max));
		}
		/// <summary>
		/// Returns a pseudo-random <see cref="int"/> that is less than or equal to 0.
		/// </summary>
		/// <returns>A pseudo-random <see cref="int"/> that is less than or equal to 0.</returns>
		static public int GetNonPositiveInt32() => (int)((uint)GetInt32() | 0x80000000);
		/// <summary>
		/// Returns a pseudo-random <see cref="int"/> that is less than or equal to 0 and greater than or equal to a specified minimum value.
		/// </summary>
		/// <param name="min">The minimum value for the number to be generated.</param>
		/// <returns>A number that is less than or equal to 0 and greater than or equal to <paramref name="min"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> is greater than 0 or equal to <see cref="int.MinValue"/>.</exception>
		static public int GetNonPositiveInt32(int min)
		{
			if (min > 0x0 || min == int.MinValue)
				throw new ArgumentOutOfRangeException(nameof(min));
			return GetNonPositiveInt32Internal(min);
		}
		/// <summary>
		/// Returns a pseudo-random <see cref="int"/> that is within a specified range.
		/// </summary>
		/// <param name="min">The minimum value for the number to be generated.</param>
		/// <param name="max">The maximum value for the number to be generated.</param>
		/// <returns>A number that is greater than or equal to <paramref name="min"/> and less than or equal <paramref name="max"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is greater than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> is greater than <paramref name="max"/> or equal to <see cref="int.MinValue"/>.</exception>
		static public int GetNonPositiveInt32(int min, int max)
		{
			if (max > 0x0)
				throw new ArgumentOutOfRangeException(nameof(max));
			if (min > max || min == int.MinValue)
				throw new ArgumentOutOfRangeException(nameof(min));
			return GetNonPositiveInt32Internal(min, max);
		}
		/// <summary>
		/// Returns a pseudo-random <see cref="int"/> that is greater than or equal to 0.
		/// </summary>
		/// <returns>A pseudo-random <see cref="int"/> that is greater than or equal to 0.</returns>
		static public int GetNonNegativeInt32() => (int)((uint)GetInt32() & 0x7FFFFFFF);
		/// <summary>
		/// Returns a pseudo-random <see cref="int"/> that is greater than or equal to 0 and less than or equal to a specified maximum value.
		/// </summary>
		/// <param name="max">The maximum value for the number to be generated.</param>
		/// <returns>A number that is greater than or equal to 0 and less than or equal to <paramref name="max"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is less than 0.</exception>
		static public int GetNonNegativeInt32(int max)
		{
			if (max < 0x0)
				throw new ArgumentOutOfRangeException(nameof(max));
			return -GetNonPositiveInt32Internal(-max);
		}
		/// <summary>
		/// Returns a pseudo-random <see cref="int"/> that is within a specified range.
		/// </summary>
		/// <param name="min">The minimum value for the number to be generated.</param>
		/// <param name="max">The maximum value for the number to be generated.</param>
		/// <returns>A number that is greater than or equal to <paramref name="min"/> and less than or equal to <paramref name="max"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="max"/> is less than <paramref name="min"/>.</exception>
		static public int GetNonNegativeInt32(int min, int max)
		{
			if (min < 0x0)
				throw new ArgumentOutOfRangeException(nameof(min));
			if (max < min)
				throw new ArgumentOutOfRangeException(nameof(max));
			return -GetNonPositiveInt32Internal(-max, -min);
		}
		/// <summary>
		/// Returns a remainder of positive pseudo-random number division by a specified divider.
		/// </summary>
		/// <param name="divider">The divider.</param>
		/// <returns>A remainder of positive pseudo-random number division by <paramref name="divider"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="divider"/> is less than 1.</exception>
		static public int GetInt32Remainder(int divider)
		{
			if (divider < 0x1)
				throw new ArgumentOutOfRangeException(nameof(divider));
			return GetNonNegativeInt32() % divider;
		}
		/// <summary>
		/// Returns a pseudo-random <see cref="float"/> that is greater than -1F and less than 1F.
		/// </summary>
		/// <returns>A pseudo-random <see cref="float"/> that is greater than -1F and less than 1F.</returns>
		static public unsafe float GetSingleFloat()
		{
			uint result = (uint)GetInt32() & 0xBFFFFFFF;
			return *(float*)&result * 0.5F;
		}
		/// <summary>
		/// Returns a pseudo-random <see cref="float"/> that is greater than -1F and less than or equal to 0.
		/// </summary>
		/// <returns>A pseudo-random <see cref="float"/> that is greater than -1F and less than 0.</returns>
		static public unsafe float GetNonPositiveSingleFloat()
		{
			uint result = (uint)GetNonPositiveInt32() & 0xBFFFFFFF;
			return *(float*)&result * 0.5F;
		}
		/// <summary>
		/// Returns a pseudo-random <see cref="float"/> that is greater than or equal to 0 and less than 1F.
		/// </summary>
		/// <returns>A pseudo-random <see cref="float"/> that is greater than or equal to 0 and less than 1F.</returns>
		static public unsafe float GetNonNegativeSingleFloat()
		{
			uint result = (uint)GetNonNegativeInt32() & 0xBFFFFFFF;
			return *(float*)&result * 0.5F;
		}
		/// <summary>
		/// Puts pseudo-random bytes to a specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <param name="count">The number of the bytes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		static public unsafe void GetBytes(byte[] buffer, int index, int count)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
			if (count == 0x0)
				return;
			int ushortCount = count >> 0x1;
			int uintCount = ushortCount >> 0x1;
			fixed (byte* floor = &buffer[index])
			{
				int offset = 0x0;
				for (; offset != uintCount; offset++)
					*((uint*)floor + offset) = (uint)GetInt32();
				offset <<= 0x1;
				if (ushortCount - offset == 0x1)
				{
					*((ushort*)floor + offset) = (ushort)(GetInt32() % (ushort.MaxValue + 0x1));
					offset++;
				}
				offset <<= 0x1;
				if (count - offset == 0x1)
					*(floor + offset) = (byte)(GetInt32() % (byte.MaxValue + 0x1));
			}
		}
	}
}