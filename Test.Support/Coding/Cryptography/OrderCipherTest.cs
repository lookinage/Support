using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Cryptography;

namespace Test.Support.Coding.Cryptography
{
	[TestClass]
	public class OrderCipherTest
	{
		[TestMethod]
		public void EncryptTest() => OrderCipher.Cipher.EncryptTest();
		[TestMethod]
		public void DecryptTest() => OrderCipher.Cipher.DecryptTest();
		[TestMethod]
		public void CommonTest() => OrderCipher.Cipher.CommonTest();
	}
}