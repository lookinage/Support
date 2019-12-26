using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.InputOutput;
using System;
using System.IO;

namespace Test.Support.InputOutput.Storage
{
	[TestClass]
	public class StreamHelperTest
	{
		private const int _maxLength = 0x1000;
		private const int _testCount = 0x1000;

		[TestMethod]
		public void EnsureLengthTest()
		{
			string path = FileStreamHelper.FreeFilePath;
			_ = Assert.ThrowsException<ArgumentNullException>(() => StreamHelper.EnsureLength(null, 0x0));
			try
			{
				using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
				{
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => stream.EnsureLength(-0x1));
					for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
					{
						int desiredLength = PseudoRandomManager.GetNonNegativeInt32(_maxLength);
						stream.EnsureLength(desiredLength);
						Assert.IsTrue(stream.Length >= desiredLength);
					}
				}
			}
			finally { File.Delete(path); }
		}
	}
}