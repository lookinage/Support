using System;

namespace Support.Coding.Cryptography
{
	/// <summary>
	/// Represents a cipher to encrypt and decrypt order of bits of random value.
	/// </summary>
	public class BitOrderCipher : Cipher
	{
		/// <summary>
		/// The instance of the <see cref="BitOrderCipher"/>.
		/// </summary>
		static public readonly BitOrderCipher Cipher;

		static BitOrderCipher() => Cipher = new BitOrderCipher();

		static private unsafe long GetSwapOffset(long dataOffset, long dataLength, byte* keyFloor, int keyLength)
		{
			long swapOffset = dataOffset + *(keyFloor + dataOffset % keyLength);
			return (swapOffset < 0x0 ? -swapOffset : swapOffset) % dataLength;
		}
		static private unsafe void Swap(byte* dataFloor, long dataOffset, long dataLength, byte* keyFloor, int keyLength)
		{
			long byteOffset = Math.DivRem(dataOffset, 0x8, out long bitOffset);
			long swapByteOffset = Math.DivRem(GetSwapOffset(dataOffset, dataLength, keyFloor, keyLength), 0x8, out long swapBitOffset);
			if ((*(dataFloor + byteOffset) & (0x1 << (int)bitOffset)) == 0x0)
			{
				if ((*(dataFloor + swapByteOffset) & (0x1 << (int)swapBitOffset)) == 0x0)
					return;
				*(dataFloor + byteOffset) |= (byte)(0x1 << (int)bitOffset);
				*(dataFloor + swapByteOffset) &= (byte)~(0x1 << (int)swapBitOffset);
				return;
			}
			if ((*(dataFloor + swapByteOffset) & (0x1 << (int)swapBitOffset)) != 0x0)
				return;
			*(dataFloor + byteOffset) &= (byte)~(0x1 << (int)bitOffset);
			*(dataFloor + swapByteOffset) |= (byte)(0x1 << (int)swapBitOffset);
		}
		static private unsafe void Encrypt(byte* dataFloor, int dataLength, byte* keyFloor, int keyLength)
		{
			long bitDataLength = (long)dataLength << 0x3;
			for (long dataOffset = 0x0; dataOffset != bitDataLength; dataOffset++)
				Swap(dataFloor, dataOffset, bitDataLength, keyFloor, keyLength);
		}
		static private unsafe void Decrypt(byte* dataFloor, int dataLength, byte* keyFloor, int keyLength)
		{
			long bitDataLength = (long)dataLength << 0x3;
			for (long dataOffset = bitDataLength - 0x1; dataOffset >= 0x0; dataOffset--)
				Swap(dataFloor, dataOffset, bitDataLength, keyFloor, keyLength);
		}

		/// <summary>
		/// Initializes the <see cref="BitOrderCipher"/>.
		/// </summary>
		public BitOrderCipher() { }

		/// <summary>
		/// Encrypts a specified data by a specified key.
		/// </summary>
		/// <param name="dataBuffer">The buffer to contain the data to encrypt.</param>
		/// <param name="dataIndex">The position of the data in <paramref name="dataBuffer"/>.</param>
		/// <param name="dataLength">The length of the data.</param>
		/// <param name="keyBuffer">The buffer to contain the key to encrypt the data.</param>
		/// <param name="keyIndex">The position of the key in <paramref name="keyBuffer"/>.</param>
		/// <param name="keyLength">The length of the key.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dataBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="dataIndex"/> is outside the range of valid indices of <paramref name="dataBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="dataLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="dataLength"/> is greater than the number of bytes from <paramref name="dataIndex"/> to the end of <paramref name="dataBuffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="keyBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyIndex"/> is outside the range of valid indices of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyLength"/> is greater than the number of bytes from <paramref name="keyIndex"/> to the end of <paramref name="keyBuffer"/>.</exception>
		public override unsafe void Encrypt(byte[] dataBuffer, int dataIndex, int dataLength, byte[] keyBuffer, int keyIndex, int keyLength)
		{
			Validate(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
			if (dataLength == 0x0 || keyLength == 0x0)
				return;
			long bitDataLength = (long)dataLength << 0x3;
			fixed (byte* dataFloor = &dataBuffer[dataIndex])
			fixed (byte* keyFloor = &keyBuffer[keyIndex])
			{
				long keyOffset = 0x0;
				do
				{
					Encrypt(dataFloor, dataLength, keyFloor + keyOffset, keyLength - (int)keyOffset);
					keyOffset += bitDataLength;
				}
				while (keyLength - keyOffset > 0x0);
			}
		}
		/// <summary>
		/// Decrypts a specified data by a specified key.
		/// </summary>
		/// <param name="dataBuffer">The buffer to contain the data to decrypt.</param>
		/// <param name="dataIndex">The position of the data in <paramref name="dataBuffer"/>.</param>
		/// <param name="dataLength">The length of the data.</param>
		/// <param name="keyBuffer">The buffer to contain the key to decrypt the data.</param>
		/// <param name="keyIndex">The position of the key in <paramref name="keyBuffer"/>.</param>
		/// <param name="keyLength">The length of the key.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dataBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="dataIndex"/> is outside the range of valid indices of <paramref name="dataBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="dataLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="dataLength"/> is greater than the number of bytes from <paramref name="dataIndex"/> to the end of <paramref name="dataBuffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="keyBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyIndex"/> is outside the range of valid indices of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyLength"/> is greater than the number of bytes from <paramref name="keyIndex"/> to the end of <paramref name="keyBuffer"/>.</exception>
		public override unsafe void Decrypt(byte[] dataBuffer, int dataIndex, int dataLength, byte[] keyBuffer, int keyIndex, int keyLength)
		{
			Validate(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
			if (dataLength == 0x0 || keyLength == 0x0)
				return;
			long bitDataLength = (long)dataLength << 0x3;
			fixed (byte* dataFloor = &dataBuffer[dataIndex])
			fixed (byte* keyFloor = &keyBuffer[keyIndex])
			{
				long keyOffset = keyLength % bitDataLength;
				if (keyOffset == 0x0)
					keyOffset = bitDataLength;
				do
				{
					Decrypt(dataFloor, dataLength, keyFloor + (keyLength - keyOffset), (int)keyOffset);
					keyOffset += bitDataLength;
				}
				while (keyLength - keyOffset >= 0x0);
			}
		}
	}
}