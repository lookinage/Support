using System;

namespace Support.Coding.Cryptography
{
	/// <summary>
	/// Represents a cipher to encrypt and decrypt order of bits of random value. The cipher selects one of order ciphers that is enough for maximum strong encryption.
	/// </summary>
	public class OrderCipher : Cipher
	{
		/// <summary>
		/// The instance of the <see cref="OrderCipher"/>.
		/// </summary>
		static public readonly OrderCipher Cipher;

		static OrderCipher() => Cipher = new OrderCipher();

		/// <summary>
		/// Initializes the <see cref="OrderCipher"/>.
		/// </summary>
		public OrderCipher() { }

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
			int ushortDataLength = dataLength >> 0x1;
			int uintDataLength = ushortDataLength >> 0x1;
			int ulongDataLength = uintDataLength >> 0x1;
			long swapCount = (long)keyLength << 0x8;
			if (swapCount / dataLength > dataLength)
			{
				BitOrderCipher.Cipher.Encrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
				return;
			}
			if (swapCount / ushortDataLength > ushortDataLength)
			{
				ByteOrderCipher.Cipher.Encrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
				return;
			}
			if (swapCount / uintDataLength > uintDataLength)
			{
				TwoByteOrderCipher.Cipher.Encrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
				return;
			}
			if (swapCount / ulongDataLength > ulongDataLength)
			{
				FourByteOrderCipher.Cipher.Encrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
				return;
			}
			EightByteOrderCipher.Cipher.Encrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
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
			int ushortDataLength = dataLength >> 0x1;
			int uintDataLength = ushortDataLength >> 0x1;
			int ulongDataLength = uintDataLength >> 0x1;
			long swapCount = (long)keyLength << 0x8;
			if (swapCount / dataLength > dataLength)
			{
				BitOrderCipher.Cipher.Decrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
				return;
			}
			if (swapCount / ushortDataLength > ushortDataLength)
			{
				ByteOrderCipher.Cipher.Decrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
				return;
			}
			if (swapCount / uintDataLength > uintDataLength)
			{
				TwoByteOrderCipher.Cipher.Decrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
				return;
			}
			if (swapCount / ulongDataLength > ulongDataLength)
			{
				FourByteOrderCipher.Cipher.Decrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
				return;
			}
			EightByteOrderCipher.Cipher.Decrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
		}
	}
}