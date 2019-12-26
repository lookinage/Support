using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using System;
using System.Collections.Generic;

namespace Test.Support.Coding.Serialization
{
	public class ISerializerTest
	{
		static private byte[] _buffer;

		static ISerializerTest() => _buffer = new byte[0x4];

		static public void SerializeTest<T>(T instance, ISerializer<T> serializer)
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => serializer.Serialize(instance, null, 0x0));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Serialize(instance, _buffer, -0x1));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Serialize(instance, _buffer, _buffer.Length + 0x1));
			int index = 0x0;
			_ = Assert.ThrowsException<ArgumentNullException>(() => serializer.Serialize(instance, null, ref index));
			index = -0x1;
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Serialize(instance, _buffer, ref index));
			index = _buffer.Length + 0x1;
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Serialize(instance, _buffer, ref index));
		}
		static public void SerializeTest<T>(ISerializer<T> serializer) where T : new() => SerializeTest(new T(), serializer);
		static public void DeserializeTest<T>(int count, ISerializer<T> serializer)
		{
			_ = ArrayHelper.EnsureLength(ref _buffer, count);
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Deserialize(-0x1, _buffer, 0x0));
			_ = Assert.ThrowsException<ArgumentNullException>(() => serializer.Deserialize(count, null, 0x0));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Deserialize(count, _buffer, -0x1));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Deserialize(count, _buffer, _buffer.Length + 0x1));
			int index = 0x0;
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Deserialize(-0x1, _buffer, ref index));
			_ = Assert.ThrowsException<ArgumentNullException>(() => serializer.Deserialize(count, null, ref index));
			index = -0x1;
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Deserialize(count, _buffer, ref index));
			index = _buffer.Length + 0x1;
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Deserialize(count, _buffer, ref index));
		}
		static public void CommonTest<T>(T instance, ISerializer<T> serializer, IEqualityComparer<T> comparer)
		{
			int count = serializer.Count(instance);
			int index = count;
			_ = ArrayHelper.EnsureLength(ref _buffer, index + count);
			serializer.Serialize(instance, _buffer, index);
			Assert.IsTrue(comparer.Equals(instance, serializer.Deserialize(count, _buffer, index)));
			serializer.Serialize(instance, _buffer, ref index);
			Assert.IsTrue(index == count << 0x1);
			index = count;
			Assert.IsTrue(comparer.Equals(instance, serializer.Deserialize(count, _buffer, ref index)));
			Assert.IsTrue(index == count << 0x1);
		}
		static public void CommonTest<T>(T instance, ISerializer<T> serializer) => CommonTest(instance, serializer, EqualityComparer<T>.Default);
	}
}