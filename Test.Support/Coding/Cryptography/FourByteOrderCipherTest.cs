using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Cryptography;

namespace Test.Support.Coding.Cryptography
{
	[TestClass]
	public class FourByteOrderCipherTest
	{
		[TestMethod]
		public void EncryptTest() => FourByteOrderCipher.Cipher.EncryptTest();
		[TestMethod]
		public void DecryptTest() => FourByteOrderCipher.Cipher.DecryptTest();
		[TestMethod]
		public void CommonTest() => FourByteOrderCipher.Cipher.CommonTest();
	}
}