using System;

namespace Support.Coding.Cryptography
{
	/// <summary>
	/// Represents a cipher to encrypt and decrypt order of eight bytes of random value.
	/// </summary>
	public class EightByteOrderCipher : Cipher
	{
		/// <summary>
		/// The instance of the <see cref="EightByteOrderCipher"/>.
		/// </summary>
		static public readonly EightByteOrderCipher Cipher;

		static EightByteOrderCipher() => Cipher = new EightByteOrderCipher();

		static private unsafe void Encrypt(byte* dataFloor, int dataLength, byte* keyFloor, int keyLength)
		{
			int ushortDataLength = dataLength >> 0x1;
			int uintDataLength = ushortDataLength >> 0x1;
			int ulongDataLength = uintDataLength >> 0x1;
			int dataOffset;
			for (dataOffset = 0x0; dataOffset != ulongDataLength; dataOffset++)
				Swap((ulong*)dataFloor, dataOffset, ulongDataLength, keyFloor, keyLength);
			dataOffset <<= 0x1;
			if (uintDataLength - dataOffset == 0x1)
			{
				FourByteOrderCipher.Swap((uint*)dataFloor, dataOffset, uintDataLength, keyFloor, keyLength);
				dataOffset++;
			}
			dataOffset <<= 0x1;
			if (ushortDataLength - dataOffset == 0x1)
			{
				TwoByteOrderCipher.Swap((ushort*)dataFloor, dataOffset, ushortDataLength, keyFloor, keyLength);
				dataOffset++;
			}
			dataOffset <<= 0x1;
			if (dataLength - dataOffset == 0x1)
				ByteOrderCipher.Swap(dataFloor, dataOffset, dataLength, keyFloor, keyLength);
		}
		static private unsafe void Decrypt(byte* dataFloor, int dataLength, byte* keyFloor, int keyLength)
		{
			int ushortDataLength = dataLength >> 0x1;
			int uintDataLength = ushortDataLength >> 0x1;
			int ulongDataLength = uintDataLength >> 0x1;
			int dataOffset;
			dataOffset = ushortDataLength << 0x1;
			if (dataLength - dataOffset == 0x1)
				ByteOrderCipher.Swap(dataFloor, dataOffset, dataLength, keyFloor, keyLength);
			dataOffset = uintDataLength << 0x1;
			if (ushortDataLength - dataOffset == 0x1)
				TwoByteOrderCipher.Swap((ushort*)dataFloor, dataOffset, ushortDataLength, keyFloor, keyLength);
			dataOffset = ulongDataLength << 0x1;
			if (uintDataLength - dataOffset == 0x1)
				FourByteOrderCipher.Swap((uint*)dataFloor, dataOffset, uintDataLength, keyFloor, keyLength);
			for (dataOffset = ulongDataLength - 0x1; dataOffset >= 0x0; dataOffset--)
				Swap((ulong*)dataFloor, dataOffset, ulongDataLength, keyFloor, keyLength);
		}
		static private unsafe void Swap(ulong* dataFloor, int dataOffset, int dataLength, byte* keyFloor, int keyLength)
		{
			int swapOffset = ByteOrderCipher.GetSwapOffset(dataOffset, dataLength, keyFloor, keyLength);
			ulong temp = *(dataFloor + dataOffset);
			*(dataFloor + dataOffset) = *(dataFloor + swapOffset);
			*(dataFloor + swapOffset) = temp;
		}

		/// <summary>
		/// Initializes the <see cref="EightByteOrderCipher"/>.
		/// </summary>
		public EightByteOrderCipher() { }

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
			int ulongDataLength = dataLength >> 0x3;
			if (ulongDataLength == 0x0)
				ulongDataLength = 0x1;
			fixed (byte* dataFloor = &dataBuffer[dataIndex])
			fixed (byte* keyFloor = &keyBuffer[keyIndex])
			{
				int keyOffset = 0x0;
				do
				{
					Encrypt(dataFloor, dataLength, keyFloor + keyOffset, keyLength - keyOffset);
					keyOffset += ulongDataLength;
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
			int ulongDataLength = dataLength >> 0x3;
			if (ulongDataLength == 0x0)
				ulongDataLength = 0x1;
			fixed (byte* dataFloor = &dataBuffer[dataIndex])
			fixed (byte* keyFloor = &keyBuffer[keyIndex])
			{
				int keyOffset = keyLength % ulongDataLength;
				if (keyOffset == 0x0)
					keyOffset = ulongDataLength;
				do
				{
					Decrypt(dataFloor, dataLength, keyFloor + (keyLength - keyOffset), keyOffset);
					keyOffset += ulongDataLength;
				}
				while (keyLength - keyOffset >= 0x0);
			}
		}
	}
}