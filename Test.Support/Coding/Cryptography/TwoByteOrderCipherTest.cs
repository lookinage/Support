using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Cryptography;

namespace Test.Support.Coding.Cryptography
{
	[TestClass]
	public class TwoByteOrderCipherTest
	{
		[TestMethod]
		public void EncryptTest() => TwoByteOrderCipher.Cipher.EncryptTest();
		[TestMethod]
		public void DecryptTest() => TwoByteOrderCipher.Cipher.DecryptTest();
		[TestMethod]
		public void CommonTest() => TwoByteOrderCipher.Cipher.CommonTest();
	}
}