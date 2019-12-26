using System;

namespace Support.Coding.Cryptography
{
	/// <summary>
	/// Represents a cipher to encrypt and decrypt data.
	/// </summary>
	public abstract class Cipher : ICipher
	{
		/// <summary>
		/// Validates arguments for <see cref="Encrypt(byte[], int, int, byte[], int, int)"/> and <see cref="Decrypt(byte[], int, int, byte[], int, int)"/> methods.
		/// </summary>
		/// <param name="dataBuffer">The buffer to contain the data to encrypt or decrypt.</param>
		/// <param name="dataIndex">The position of the data in <paramref name="dataBuffer"/>.</param>
		/// <param name="dataLength">The length of the data.</param>
		/// <param name="keyBuffer">The buffer to contain the key to encrypt or decrypt the data.</param>
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
		static protected void Validate(byte[] dataBuffer, int dataIndex, int dataLength, byte[] keyBuffer, int keyIndex, int keyLength)
		{
			if (dataBuffer == null)
				throw new ArgumentNullException(nameof(dataBuffer));
			if (dataIndex < 0x0 || dataIndex > dataBuffer.Length)
				throw new ArgumentOutOfRangeException(nameof(dataIndex));
			if (dataLength < 0x0)
				throw new ArgumentOutOfRangeException(nameof(dataLength));
			if (dataLength > dataBuffer.Length - dataIndex)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(dataLength), nameof(dataIndex), nameof(dataBuffer)));
			if (keyBuffer == null)
				throw new ArgumentNullException(nameof(keyBuffer));
			if (keyIndex < 0x0 || keyIndex > keyBuffer.Length)
				throw new ArgumentOutOfRangeException(nameof(keyIndex));
			if (keyLength < 0x0)
				throw new ArgumentOutOfRangeException(nameof(keyLength));
			if (keyLength > keyBuffer.Length - keyIndex)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(keyLength), nameof(keyIndex), nameof(keyBuffer)));
		}

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
		public abstract void Encrypt(byte[] dataBuffer, int dataIndex, int dataLength, byte[] keyBuffer, int keyIndex, int keyLength);
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
		public abstract void Decrypt(byte[] dataBuffer, int dataIndex, int dataLength, byte[] keyBuffer, int keyIndex, int keyLength);
	}
}