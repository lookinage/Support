using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using System;

namespace Test.Support
{
	[TestClass]
	public class ArrayHelperTest
	{
		private const int _maxLength = 0x100;
		private const int _testCount = 0x100000;

		[TestMethod]
		public void EnsureLengthTest()
		{
			byte[] array = null;
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => ArrayHelper.EnsureLength(ref array, -0x1));
			for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
			{
				if (PseudoRandomManager.GetInt32Remainder(0x100) == 0x1)
					array = null;
				int desiredLength = PseudoRandomManager.GetNonNegativeInt32(_maxLength);
				_ = ArrayHelper.EnsureLength(ref array, desiredLength);
				Assert.IsTrue(array.Length >= desiredLength);
			}
		}
	}
}