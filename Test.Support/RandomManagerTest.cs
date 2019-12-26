using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using System;

namespace Test.Support
{
	[TestClass]
	public class RandomManagerTest
	{
		[TestMethod]
		public void GetTest()
		{
			const int testCount = 0x100;
			const int minLength = 0x4;
			const int maxLength = 0x100;
			byte[] array = new byte[minLength];
			_ = Assert.ThrowsException<ArgumentNullException>(() => RandomManager.GetBytes(null, 0x0, minLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => RandomManager.GetBytes(array, -0x1, minLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => RandomManager.GetBytes(array, minLength + 0x1, minLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => RandomManager.GetBytes(array, 0x0, -0x1));
			_ = Assert.ThrowsException<ArgumentException>(() => RandomManager.GetBytes(array, 0x0, minLength + 0x1));
			bool[] changes = null;
			for (int testIndex = 0x0; testIndex != testCount; testIndex++)
			{
				int index = PseudoRandomManager.GetNonNegativeInt32(minLength, maxLength);
				int length = PseudoRandomManager.GetNonNegativeInt32(minLength, maxLength);
				_ = ArrayHelper.EnsureLength(ref array, index + length);
				_ = ArrayHelper.EnsureLength(ref changes, index + length);
				Array.Clear(array, index, length);
				Array.Clear(changes, 0x0, length);
				for (; ; )
				{
					RandomManager.GetBytes(array, index, length);
					bool everyoneIsChanged = true;
					for (int offset = 0x0; offset != length; offset++)
					{
						if (array[index + offset] == 0x0)
						{
							changes[offset] = true;
							continue;
						}
						if (changes[offset])
							continue;
						everyoneIsChanged = false;
					}
					if (everyoneIsChanged)
						break;
				}
			}
		}
	}
}