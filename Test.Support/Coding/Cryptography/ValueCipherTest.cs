using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Cryptography;

namespace Test.Support.Coding.Cryptography
{
	[TestClass]
	public class ValueCipherTest
	{
		[TestMethod]
		public void EncryptTest() => ValueCipher.Cipher.EncryptTest();
		[TestMethod]
		public void DecryptTest() => ValueCipher.Cipher.DecryptTest();
		[TestMethod]
		public void CommonTest() => ValueCipher.Cipher.CommonTest();
	}
}