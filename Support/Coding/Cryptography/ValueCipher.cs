using System;

namespace Support.Coding.Cryptography
{
	/// <summary>
	/// Represents a cipher to encrypt and decrypt values which order is random.
	/// </summary>
	public class ValueCipher : Cipher
	{
		/// <summary>
		/// The instance of the <see cref="ValueCipher"/>.
		/// </summary>
		static public readonly ValueCipher Cipher;

		static ValueCipher() => Cipher = new ValueCipher();

		static private unsafe void EncryptDecrypt(byte* dataFloor, int dataLength, byte* keyFloor, int keyLength)
		{
			int ushortDataLength = dataLength >> 0x1;
			int uintDataLength = ushortDataLength >> 0x1;
			int ulongDataLength = uintDataLength >> 0x1;
			int ushortKeyLength = keyLength >> 0x1;
			int uintKeyLength = ushortKeyLength >> 0x1;
			int ulongKeyLength = uintKeyLength >> 0x1;
			int dataOffset;
			int keyOffset = 0x0;
			if ((keyLength & 0x1) != 0)
				for (dataOffset = 0x0; dataOffset != dataLength; dataOffset++)
				{
					*(dataFloor + dataOffset) ^= *(keyFloor + keyOffset);
					keyOffset++;
					if (keyOffset == keyLength)
						keyOffset = 0x0;
				}
			else if ((keyLength & 0x2) != 0)
			{
				for (dataOffset = 0x0; dataOffset != ushortDataLength; dataOffset++)
				{
					*((ushort*)dataFloor + dataOffset) ^= *((ushort*)keyFloor + keyOffset);
					keyOffset++;
					if (keyOffset == ushortKeyLength)
						keyOffset = 0x0;
				}
				dataOffset <<= 0x1;
				keyOffset <<= 0x1;
				if (dataLength - dataOffset == 0x1)
					*(dataFloor + dataOffset) ^= *(keyFloor + keyOffset);
			}
			else if ((keyLength & 0x4) != 0)
			{
				for (dataOffset = 0x0; dataOffset != uintDataLength; dataOffset++)
				{
					*((uint*)dataFloor + dataOffset) ^= *((uint*)keyFloor + keyOffset);
					keyOffset++;
					if (keyOffset == uintKeyLength)
						keyOffset = 0x0;
				}
				dataOffset <<= 0x1;
				keyOffset <<= 0x1;
				if (ushortDataLength - dataOffset >= 0x1)
				{
					*((ushort*)dataFloor + dataOffset) ^= *((ushort*)keyFloor + keyOffset);
					keyOffset++;
					dataOffset++;
				}
				dataOffset <<= 0x1;
				keyOffset <<= 0x1;
				if (dataLength - dataOffset == 0x1)
					*(dataFloor + dataOffset) ^= *(keyFloor + keyOffset);
			}
			else
			{
				for (dataOffset = 0x0; dataOffset != ulongDataLength; dataOffset++)
				{
					*((ulong*)dataFloor + dataOffset) ^= *((ulong*)keyFloor + keyOffset);
					keyOffset++;
					if (keyOffset == ulongKeyLength)
						keyOffset = 0x0;
				}
				dataOffset <<= 0x1;
				keyOffset <<= 0x1;
				if (uintDataLength - dataOffset == 0x1)
				{
					*((uint*)dataFloor + dataOffset) ^= *((uint*)keyFloor + keyOffset);
					keyOffset++;
					dataOffset++;
				}
				dataOffset <<= 0x1;
				keyOffset <<= 0x1;
				if (ushortDataLength - dataOffset >= 0x1)
				{
					*((ushort*)dataFloor + dataOffset) ^= *((ushort*)keyFloor + keyOffset);
					keyOffset++;
					dataOffset++;
				}
				dataOffset <<= 0x1;
				keyOffset <<= 0x1;
				if (dataLength - dataOffset == 0x1)
					*(dataFloor + dataOffset) ^= *(keyFloor + keyOffset);
			}
		}

		/// <summary>
		/// Initializes the <see cref="ValueCipher"/>.
		/// </summary>
		public ValueCipher() { }

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
					EncryptDecrypt(dataFloor, dataLength, keyFloor + keyOffset, keyLength - keyOffset);
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
					EncryptDecrypt(dataFloor, dataLength, keyFloor + (keyLength - keyOffset), keyOffset);
					keyOffset += dataLength;
				}
				while (keyLength - keyOffset >= 0x0);
			}
		}
	}
}