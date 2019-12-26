using System;

namespace Support.Coding.Cryptography
{
	/// <summary>
	/// Represents a cipher to encrypt and decrypt order of bytes of random value.
	/// </summary>
	public class ByteOrderCipher : Cipher
	{
		/// <summary>
		/// The instance of the <see cref="ByteOrderCipher"/>.
		/// </summary>
		static public readonly ByteOrderCipher Cipher;

		static ByteOrderCipher() => Cipher = new ByteOrderCipher();

		static private unsafe void Encrypt(byte* dataFloor, int dataLength, byte* keyFloor, int keyLength)
		{
			for (int dataOffset = 0x0; dataOffset != dataLength; dataOffset++)
				Swap(dataFloor, dataOffset, dataLength, keyFloor, keyLength);
		}
		static private unsafe void Decrypt(byte* dataFloor, int dataLength, byte* keyFloor, int keyLength)
		{
			for (int dataOffset = dataLength - 0x1; dataOffset >= 0x0; dataOffset--)
				Swap(dataFloor, dataOffset, dataLength, keyFloor, keyLength);
		}
		static internal unsafe int GetSwapOffset(int dataOffset, int dataLength, byte* keyFloor, int keyLength)
		{
			int swapOffset = dataOffset + *(keyFloor + dataOffset % keyLength);
			return (swapOffset < 0x0 ? -swapOffset : swapOffset) % dataLength;
		}
		static internal unsafe void Swap(byte* dataFloor, int dataOffset, int dataLength, byte* keyFloor, int keyLength)
		{
			int swapOffset = GetSwapOffset(dataOffset, dataLength, keyFloor, keyLength);
			byte temp = *(dataFloor + dataOffset);
			*(dataFloor + dataOffset) = *(dataFloor + swapOffset);
			*(dataFloor + swapOffset) = temp;
		}

		/// <summary>
		/// Initializes the <see cref="ByteOrderCipher"/>.
		/// </summary>
		public ByteOrderCipher() { }

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
			fixed (byte* dataFloor = &dataBuffer[dataIndex])
			fixed (byte* keyFloor = &keyBuffer[keyIndex])
			{
				int keyOffset = 0x0;
				do
				{
					Encrypt(dataFloor, dataLength, keyFloor + keyOffset, keyLength - keyOffset);
					keyOffset += dataLength;
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
			fixed (byte* dataFloor = &dataBuffer[dataIndex])
			fixed (byte* keyFloor = &keyBuffer[keyIndex])
			{
				int keyOffset = keyLength % dataLength;
				if (keyOffset == 0x0)
					keyOffset = dataLength;
				do
				{
					Decrypt(dataFloor, dataLength, keyFloor + (keyLength - keyOffset), keyOffset);
					keyOffset += dataLength;
				}
				while (keyLength - keyOffset >= 0x0);
			}
		}
	}
}