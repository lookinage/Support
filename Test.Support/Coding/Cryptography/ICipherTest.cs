using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Cryptography;
using System;

namespace Test.Support.Coding.Cryptography
{
	static public class ICipherTest
	{
		private const int _maxLength = 0x1000;
		private const int _testCount = 0x1000;

		static public void EncryptTest(this ICipher cipher)
		{
			byte[] dataBuffer = new byte[_maxLength];
			byte[] keyBuffer = new byte[_maxLength];
			_ = Assert.ThrowsException<ArgumentNullException>(() => cipher.Encrypt(null, 0x0, _maxLength, keyBuffer, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Encrypt(dataBuffer, -0x1, _maxLength, keyBuffer, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Encrypt(dataBuffer, _maxLength + 0x1, _maxLength, keyBuffer, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Encrypt(dataBuffer, 0x0, -0x1, keyBuffer, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentException>(() => cipher.Encrypt(dataBuffer, 0x0, _maxLength + 0x1, keyBuffer, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentNullException>(() => cipher.Encrypt(dataBuffer, 0x0, _maxLength, null, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Encrypt(dataBuffer, 0x0, _maxLength, keyBuffer, -0x1, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Encrypt(dataBuffer, 0x0, _maxLength, keyBuffer, _maxLength + 0x1, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Encrypt(dataBuffer, 0x0, _maxLength, keyBuffer, 0x0, -0x1));
			_ = Assert.ThrowsException<ArgumentException>(() => cipher.Encrypt(dataBuffer, 0x0, _maxLength, keyBuffer, 0x0, _maxLength + 0x1));
			cipher.Encrypt(dataBuffer, 0x0, _maxLength, keyBuffer, 0x0, _maxLength);
		}
		static public void DecryptTest(this ICipher cipher)
		{
			byte[] dataBuffer = new byte[_maxLength];
			byte[] keyBuffer = new byte[_maxLength];
			_ = Assert.ThrowsException<ArgumentNullException>(() => cipher.Decrypt(null, 0x0, _maxLength, keyBuffer, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Decrypt(dataBuffer, -0x1, _maxLength, keyBuffer, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Decrypt(dataBuffer, _maxLength + 0x1, _maxLength, keyBuffer, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Decrypt(dataBuffer, 0x0, -0x1, keyBuffer, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentException>(() => cipher.Decrypt(dataBuffer, 0x0, _maxLength + 0x1, keyBuffer, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentNullException>(() => cipher.Decrypt(dataBuffer, 0x0, _maxLength, null, 0x0, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Decrypt(dataBuffer, 0x0, _maxLength, keyBuffer, -0x1, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Decrypt(dataBuffer, 0x0, _maxLength, keyBuffer, _maxLength + 0x1, _maxLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => cipher.Decrypt(dataBuffer, 0x0, _maxLength, keyBuffer, 0x0, -0x1));
			_ = Assert.ThrowsException<ArgumentException>(() => cipher.Decrypt(dataBuffer, 0x0, _maxLength, keyBuffer, 0x0, _maxLength + 0x1));
			cipher.Decrypt(dataBuffer, 0x0, _maxLength, keyBuffer, 0x0, _maxLength);
		}
		static public void CommonTest(this ICipher cipher)
		{
			byte[] dataBuffer = null;
			byte[] dataBackupBuffer = null;
			byte[] keyBuffer = null;
			bool oneChanged = false;
			for (int testIndex = 0x0; testIndex != _testCount || !oneChanged; testIndex++)
			{
				int dataIndex = PseudoRandomManager.GetNonNegativeInt32(_maxLength);
				int dataLength = PseudoRandomManager.GetNonNegativeInt32(_maxLength);
				_ = ArrayHelper.EnsureLength(ref dataBuffer, dataIndex + dataLength);
				_ = ArrayHelper.EnsureLength(ref dataBackupBuffer, dataIndex + dataLength);
				RandomManager.GetBytes(dataBuffer, dataIndex, dataLength);
				Array.Copy(dataBuffer, dataIndex, dataBackupBuffer, dataIndex, dataLength);
				int keyIndex = PseudoRandomManager.GetNonNegativeInt32(_maxLength);
				int keyLength = PseudoRandomManager.GetNonNegativeInt32(_maxLength);
				_ = ArrayHelper.EnsureLength(ref keyBuffer, keyIndex + keyLength);
				RandomManager.GetBytes(keyBuffer, keyIndex, keyLength);
				cipher.Encrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
				for (int dataOffset = 0x0; dataOffset != dataLength; dataOffset++)
				{
					if (dataBuffer[dataIndex + dataOffset] == dataBackupBuffer[dataIndex + dataOffset])
						continue;
					oneChanged = true;
					break;
				}
				cipher.Decrypt(dataBuffer, dataIndex, dataLength, keyBuffer, keyIndex, keyLength);
				for (int dataOffset = 0x0; dataOffset != dataLength; dataOffset++)
					Assert.IsTrue(dataBuffer[dataIndex + dataOffset] == dataBackupBuffer[dataIndex + dataOffset]);
			}
		}
	}
}