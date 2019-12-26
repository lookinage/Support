using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization.System;
using Support.InputOutput.Storage;
using System;
using System.Collections.Generic;

namespace Test.Support.InputOutput.Storage
{
	[TestClass]
	public class LogStorageTest
	{
		private const int _testCount = 0x100;

		static private byte[] _buffer;

		[TestMethod()]
		public void FromFileTest()
		{
			string path = FileStreamHelper.FreeFilePath;
			try
			{
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => LogStorage.FromFile(-0x1, path));
				_ = Assert.ThrowsException<ArgumentNullException>(() => LogStorage.FromFile(0x0, null));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => LogStorage.FromFile(0x0, path, 0x0));
				LogStorage.FromFile(0x0, path).Close();
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
				using (LogStorage storage = LogStorage.FromFile(length, path))
				{
					_ = ArrayHelper.EnsureLength(ref _buffer, length);
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Read(-0x1, length, _buffer, 0x0));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Read(0x0, -0x1, _buffer, 0x0));
					_ = Assert.ThrowsException<ArgumentException>(() => storage.Read(0x1, length, _buffer, 0x0));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Read(length + 0x1, length, _buffer, 0x0));
					_ = Assert.ThrowsException<ArgumentNullException>(() => storage.Read(0x0, length, null, 0x0));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Read(0x0, length, _buffer, -0x1));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Read(0x0, length, _buffer, _buffer.Length + 0x1));
					_ = Assert.ThrowsException<ArgumentException>(() => storage.Read(0x0, length, _buffer, _buffer.Length - length + 0x1));
					storage.Read(0x0, length, _buffer, 0x0);
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
				using (LogStorage stream = LogStorage.FromFile(0x0, path))
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
				LogStorage stream = LogStorage.FromFile(0x0, path);
				_ = ArrayHelper.EnsureLength(ref _buffer, Int32SerializerBuilder.Default.Count);
				int length = 0x0;
				List<int> list = new List<int>();
				for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
				{
					Assert.IsTrue(stream.Length == length);
					int value = PseudoRandomManager.GetInt32();
					Int32SerializerBuilder.Default.Serialize(value, _buffer, 0x0);
					stream.Write(_buffer, 0x0, Int32SerializerBuilder.Default.Count);
					length += Int32SerializerBuilder.Default.Count;
					list.Add(value);
					if (testIndex % 0x4 == PseudoRandomManager.GetInt32Remainder(0x4))
					{
						int count = list.Count;
						for (int index = 0x0; index != count; index++)
						{
							stream.Read(index * Int32SerializerBuilder.Default.Count, Int32SerializerBuilder.Default.Count, _buffer, 0x0);
							Assert.IsTrue(Int32SerializerBuilder.Default.Deserialize(_buffer, 0x0) == list[index]);
						}
					}
				}
				stream.Close();
			}
			finally { FileStreamHelper.Delete(path); }
		}
	}
}