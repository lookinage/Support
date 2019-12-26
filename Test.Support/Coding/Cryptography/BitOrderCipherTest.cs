using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Cryptography;

namespace Test.Support.Coding.Cryptography
{
	[TestClass]
	public class BitOrderCipherTest
	{
		[TestMethod]
		public void EncryptTest() => BitOrderCipher.Cipher.EncryptTest();
		[TestMethod]
		public void DecryptTest() => BitOrderCipher.Cipher.DecryptTest();
		[TestMethod]
		public void CommonTest() => BitOrderCipher.Cipher.CommonTest();
	}
}