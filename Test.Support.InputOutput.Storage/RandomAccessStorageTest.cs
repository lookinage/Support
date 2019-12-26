using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization.System;
using Support.InputOutput.Storage;
using System;

namespace Test.Support.InputOutput.Storage
{
	[TestClass]
	public class RandomAccessStorageTest
	{
		private const int _testCount = 0x1000;

		static private byte[] _buffer;

		[TestMethod()]
		public void FromFileTest()
		{
			string path = FileStreamHelper.FreeFilePath;
			try
			{
				_ = Assert.ThrowsException<ArgumentNullException>(() => RandomAccessStorage.FromFile(null));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => RandomAccessStorage.FromFile(FileStreamHelper.FreeFilePath, -0x1));
				RandomAccessStorage.FromFile(path).Close();
			}
			finally { FileStreamHelper.Delete(path); }
		}
		[TestMethod()]
		public void ReadTest()
		{
			int count = Int32SerializerBuilder.Default.Count;
			_ = ArrayHelper.EnsureLength(ref _buffer, count);
			string path = FileStreamHelper.FreeFilePath;
			try
			{
				using (RandomAccessStorage storage = RandomAccessStorage.FromFile(path))
				{
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Read(-0x1, count, _buffer, 0x0));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Read(0x0, -0x1, _buffer, 0x0));
					_ = Assert.ThrowsException<ArgumentException>(() => storage.Read(long.MaxValue - count + 0x1, count, _buffer, 0x0));
					_ = Assert.ThrowsException<ArgumentNullException>(() => storage.Read(0x0, count, null, 0x0));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Read(0x0, count, _buffer, -0x1));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Read(0x0, count, _buffer, _buffer.Length + 0x1));
					_ = Assert.ThrowsException<ArgumentException>(() => storage.Read(0x0, count, _buffer, _buffer.Length - count + 0x1));
					storage.Read(0x0, count, _buffer, 0x0);
				}
			}
			finally { FileStreamHelper.Delete(path); }
		}
		[TestMethod()]
		public void WriteTest()
		{
			int count = Int32SerializerBuilder.Default.Count;
			_ = ArrayHelper.EnsureLength(ref _buffer, count);
			string path = FileStreamHelper.FreeFilePath;
			try
			{
				using (RandomAccessStorage storage = RandomAccessStorage.FromFile(path))
				{
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Write(_buffer, 0x0, count, -0x1));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Write(_buffer, 0x0, -0x1, 0x0));
					_ = Assert.ThrowsException<ArgumentException>(() => storage.Write(_buffer, 0x0, count, long.MaxValue - count + 0x1));
					_ = Assert.ThrowsException<ArgumentNullException>(() => storage.Write(null, 0x0, count, 0x0));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Write(_buffer, -0x1, count, 0x0));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => storage.Write(_buffer, _buffer.Length + 0x1, count, 0x0));
					_ = Assert.ThrowsException<ArgumentException>(() => storage.Write(_buffer, _buffer.Length - count + 0x1, count, 0x0));
					storage.Write(_buffer, 0x0, count, 0x0);
				}
			}
			finally { FileStreamHelper.Delete(path); }
		}
		[TestMethod()]
		public void CommonTest()
		{
			int count = 0x100;
			string path = FileStreamHelper.FreeFilePath;
			try
			{
				RandomAccessStorage storage = RandomAccessStorage.FromFile(path);
				_ = ArrayHelper.EnsureLength(ref _buffer, Int32SerializerBuilder.Default.Count);
				byte[] cache = new byte[count];
				for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
				{
					int value = PseudoRandomManager.GetInt32();
					int position = PseudoRandomManager.GetNonNegativeInt32(count - Int32SerializerBuilder.Default.Count);
					Int32SerializerBuilder.Default.Serialize(value, _buffer, 0x0);
					storage.Write(_buffer, 0x0, Int32SerializerBuilder.Default.Count, position);
					Array.Copy(_buffer, 0x0, cache, position, Int32SerializerBuilder.Default.Count);
					for (int index = 0x0; index != count / Int32SerializerBuilder.Default.Count / 0x2; index += Int32SerializerBuilder.Default.Count)
					{
						storage.Read(index, Int32SerializerBuilder.Default.Count, _buffer, 0x0);
						Assert.IsTrue(Int32SerializerBuilder.Default.Deserialize(_buffer, 0x0) == Int32SerializerBuilder.Default.Deserialize(cache, index));
					}
					for (int index = count - Int32SerializerBuilder.Default.Count; index >= count / Int32SerializerBuilder.Default.Count / 0x2; index -= Int32SerializerBuilder.Default.Count)
					{
						storage.Read(index, Int32SerializerBuilder.Default.Count, _buffer, 0x0);
						Assert.IsTrue(Int32SerializerBuilder.Default.Deserialize(_buffer, 0x0) == Int32SerializerBuilder.Default.Deserialize(cache, index));
					}
				}
				storage.Close();
			}
			finally { FileStreamHelper.Delete(path); }
		}
	}
}