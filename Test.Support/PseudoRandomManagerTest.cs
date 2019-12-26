using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using System;

namespace Test.Support
{
	[TestClass]
	public class PseudoRandomManagerTest
	{
		private const int _testCount = 0x10000;

		private void GetInt32Test(int min, int max)
		{
			bool wasMin = false;
			bool wasMax = false;
			for (int testIndex = 0x0; testIndex != _testCount || !wasMin || !wasMax; testIndex++)
			{
				int result = PseudoRandomManager.GetInt32(min, max);
				if (result == min)
					wasMin = true;
				if (result == max)
					wasMax = true;
				Assert.IsTrue(result >= min && result <= max);
			}
		}
		private void GetNonPositiveInt32Test(int min)
		{
			bool wasMin = false;
			bool wasMax = false;
			for (int testIndex = 0x0; testIndex != _testCount || !wasMin || !wasMax; testIndex++)
			{
				int result = PseudoRandomManager.GetNonPositiveInt32(min);
				if (result == min)
					wasMin = true;
				if (result == 0x0)
					wasMax = true;
				Assert.IsTrue(result >= min && result <= 0x0);
			}
		}
		private void GetNonPositiveInt32Test(int min, int max)
		{
			bool wasMin = false;
			bool wasMax = false;
			for (int testIndex = 0x0; testIndex != _testCount || !wasMin || !wasMax; testIndex++)
			{
				int result = PseudoRandomManager.GetNonPositiveInt32(min, max);
				if (result == min)
					wasMin = true;
				if (result == max)
					wasMax = true;
				Assert.IsTrue(result >= min && result <= max);
			}
		}
		private void GetNonNegativeInt32Test(int max)
		{
			bool wasMin = false;
			bool wasMax = false;
			for (int testIndex = 0x0; testIndex != _testCount || !wasMin || !wasMax; testIndex++)
			{
				int result = PseudoRandomManager.GetNonNegativeInt32(max);
				if (result == 0x0)
					wasMin = true;
				if (result == max)
					wasMax = true;
				Assert.IsTrue(result >= 0x0 && result <= max);
			}
		}
		private void GetNonNegativeInt32Test(int min, int max)
		{
			bool wasMin = false;
			bool wasMax = false;
			for (int testIndex = 0x0; testIndex != _testCount || !wasMin || !wasMax; testIndex++)
			{
				int result = PseudoRandomManager.GetNonNegativeInt32(min, max);
				if (result == min)
					wasMin = true;
				if (result == max)
					wasMax = true;
				Assert.IsTrue(result >= min && result <= max);
			}
		}
		private void GetRemainderInt32Test(int count)
		{
			bool wasMin = false;
			bool wasMax = false;
			for (int testIndex = 0x0; testIndex != _testCount || !wasMin || !wasMax; testIndex++)
			{
				int result = PseudoRandomManager.GetInt32Remainder(count);
				if (result == 0x0)
					wasMin = true;
				if (result == count - 0x1)
					wasMax = true;
				Assert.IsTrue(result >= 0x0 && result < count);
			}
		}

		[TestMethod]
		public void GetInt32Test()
		{
			_ = PseudoRandomManager.GetInt32();
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetInt32(0x0, -0x1));
			GetInt32Test(0x0, 0x0);
			GetInt32Test(0x0, 0x100);
			GetInt32Test(-0x10, 0x100);
			GetInt32Test(-0x100, 0x100);
			GetInt32Test(-0x100, 0x0);
			GetInt32Test(-0x100, -0x10);
		}
		[TestMethod]
		public void GetNonPositiveInt32Test()
		{
			for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
				Assert.IsTrue(PseudoRandomManager.GetNonPositiveInt32() <= 0x0);
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetNonPositiveInt32(0x1));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetNonPositiveInt32(int.MinValue));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetNonPositiveInt32(0x0, 0x1));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetNonPositiveInt32(-0x1, 0x2));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetNonPositiveInt32(int.MinValue, 0x0));
			GetNonPositiveInt32Test(-0x100);
			GetNonPositiveInt32Test(-0x10);
			GetNonPositiveInt32Test(-0x0);
			GetNonPositiveInt32Test(0x0, 0x0);
			GetNonPositiveInt32Test(-0x100, 0x0);
			GetNonPositiveInt32Test(-0x100, -0x10);
		}
		[TestMethod]
		public void GetNonNegativeInt32Test()
		{
			for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
				Assert.IsTrue(PseudoRandomManager.GetNonNegativeInt32() >= 0x0);
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetNonNegativeInt32(-0x1));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetNonNegativeInt32(0x0, -0x1));
			GetNonNegativeInt32Test(0x100);
			GetNonNegativeInt32Test(0x5);
			GetNonNegativeInt32Test(0x0);
			GetNonNegativeInt32Test(0x0, 0x0);
			GetNonNegativeInt32Test(0x0, 0x100);
			GetNonNegativeInt32Test(0x5, 0x100);
		}
		[TestMethod]
		public void GetInt32RemainderTest()
		{
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetInt32Remainder(0x0));
			GetRemainderInt32Test(0x100);
			GetRemainderInt32Test(0x10);
			GetRemainderInt32Test(0x1);
		}
		[TestMethod]
		public void GetSingleFloatTest()
		{
			for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
			{
				float result = PseudoRandomManager.GetSingleFloat();
				Assert.IsTrue(result > -1F && result < 1F);
			}
		}
		[TestMethod]
		public void GetNonPositiveSingleFloatTest()
		{
			for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
			{
				float result = PseudoRandomManager.GetNonPositiveSingleFloat();
				Assert.IsTrue(result > -1F && result <= 0F);
			}
		}
		[TestMethod]
		public void GetNonNegativeSingleFloatTest()
		{
			for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
			{
				float result = PseudoRandomManager.GetNonNegativeSingleFloat();
				Assert.IsTrue(result >= 0F && result < 1F);
			}
		}
		[TestMethod]
		public void GetBytesTest()
		{
			const int testCount = 0x100;
			const int minLength = 0x4;
			const int maxLength = 0x100;
			byte[] array = new byte[minLength];
			_ = Assert.ThrowsException<ArgumentNullException>(() => PseudoRandomManager.GetBytes(null, 0x0, minLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetBytes(array, -0x1, minLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetBytes(array, minLength + 0x1, minLength));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => PseudoRandomManager.GetBytes(array, 0x0, -0x1));
			_ = Assert.ThrowsException<ArgumentException>(() => PseudoRandomManager.GetBytes(array, 0x0, minLength + 0x1));
			bool[] changes = null;
			for (int testIndex = 0x0; testIndex != testCount; testIndex++)
			{
				int index = PseudoRandomManager.GetNonNegativeInt32(maxLength);
				int length = PseudoRandomManager.GetNonNegativeInt32(maxLength);
				_ = ArrayHelper.EnsureLength(ref array, index + length);
				_ = ArrayHelper.EnsureLength(ref changes, index + length);
				Array.Clear(array, index, length);
				Array.Clear(changes, 0x0, length);
				for (; ; )
				{
					PseudoRandomManager.GetBytes(array, index, length);
					bool everyoneIsChanged = true;
					for (int offset = 0x0; offset != length; offset++)
					{
						if (array[index + offset] != 0x0)
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