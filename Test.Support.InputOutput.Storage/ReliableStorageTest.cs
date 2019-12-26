using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization.System;
using Support.InputOutput.Storage;
using System;

namespace Test.Support.InputOutput.Storage
{
	[TestClass]
	public class ReliableStorageTest
	{
		private const int _testCount = 0x100;

		static private byte[] _buffer;

		[TestMethod()]
		public void FromFileTest()
		{
			string path = FileStreamHelper.FreeFilePath;
			try
			{
				_ = Assert.ThrowsException<ArgumentNullException>(() => ReliableStorage.FromFile(null));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => ReliableStorage.FromFile(path, 0x0));
				ReliableStorage.FromFile(path).Close();
			}
			finally { FileStreamHelper.Delete(path); }
		}
		[TestMethod()]
		public void ReadTest()
		{
			string path = FileStreamHelper.FreeFilePath;
			int length = Int32SerializerBuilder.Default.Count;
			try
			{
				using (ReliableStorage storage = ReliableStorage.FromFile(path))
				{
					_ = ArrayHelper.EnsureLength(ref _buffer, length);
					_ = Assert.ThrowsException<ArgumentNullException>(() => storage.Read(null, 0x0));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Read(_buffer, -0x1));
					storage.Read(_buffer, 0x0);
				}
			}
			finally { FileStreamHelper.Delete(path); }
		}
		[TestMethod()]
		public void WriteTest()
		{
			string path = FileStreamHelper.FreeFilePath;
			try
			{
				using (ReliableStorage stream = ReliableStorage.FromFile(path))
				{
					int length = Int32SerializerBuilder.Default.Count;
					_ = ArrayHelper.EnsureLength(ref _buffer, length);
					_ = Assert.ThrowsException<ArgumentNullException>(() => stream.Write(null, 0x0, length));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => stream.Write(_buffer, -0x1, length));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => stream.Write(_buffer, _buffer.Length + 0x1, length));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => stream.Write(_buffer, 0x0, -0x1));
					_ = Assert.ThrowsException<ArgumentException>(() => stream.Write(_buffer, _buffer.Length - length + 0x1, length));
					stream.Write(_buffer, 0x0, length);
					Assert.IsTrue(stream.Length == length);
				}
			}
			finally { FileStreamHelper.Delete(path); }
		}
		[TestMethod()]
		public void CommonTest()
		{
			string path = FileStreamHelper.FreeFilePath;
			try
			{
				ReliableStorage storage = ReliableStorage.FromFile(path);
				_ = ArrayHelper.EnsureLength(ref _buffer, Int32SerializerBuilder.Default.Count);
				for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
				{
					int value = PseudoRandomManager.GetInt32();
					Int32SerializerBuilder.Default.Serialize(value, _buffer, 0x0);
					storage.Write(_buffer, 0x0, Int32SerializerBuilder.Default.Count);
					storage.Close();
					storage = ReliableStorage.FromFile(path);
					storage.Read(_buffer, 0x0);
					Assert.IsTrue(Int32SerializerBuilder.Default.Deserialize(_buffer, 0x0) == value);
				}
				storage.Close();
			}
			finally { FileStreamHelper.Delete(path); }
		}
	}
}