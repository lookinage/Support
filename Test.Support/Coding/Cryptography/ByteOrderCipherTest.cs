using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Cryptography;

namespace Test.Support.Coding.Cryptography
{
	[TestClass]
	public class ByteOrderCipherTest
	{
		[TestMethod]
		public void EncryptTest() => ByteOrderCipher.Cipher.EncryptTest();
		[TestMethod]
		public void DecryptTest() => ByteOrderCipher.Cipher.DecryptTest();
		[TestMethod]
		public void CommonTest() => ByteOrderCipher.Cipher.CommonTest();
	}
}