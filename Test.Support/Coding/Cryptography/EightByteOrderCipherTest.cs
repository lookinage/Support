using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Cryptography;

namespace Test.Support.Coding.Cryptography
{
	[TestClass]
	public class EightByteOrderCipherTest
	{
		[TestMethod]
		public void EncryptTest() => EightByteOrderCipher.Cipher.EncryptTest();
		[TestMethod]
		public void DecryptTest() => EightByteOrderCipher.Cipher.DecryptTest();
		[TestMethod]
		public void CommonTest() => EightByteOrderCipher.Cipher.CommonTest();
	}
}