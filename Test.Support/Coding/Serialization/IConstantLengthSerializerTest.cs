using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using System;
using System.Collections.Generic;

namespace Test.Support.Coding.Serialization
{
	public class IConstantLengthSerializerTest
	{
		[ThreadStatic]
		static private byte[] _buffer;

		static IConstantLengthSerializerTest() => _buffer = new byte[0x0];

		static public void SerializeTest<T>(T validInstance, IConstantLengthSerializer<T> serializer) => ISerializerTest.SerializeTest(validInstance, serializer);
		static public void SerializeTest<T>(IConstantLengthSerializer<T> serializer) where T : new() => SerializeTest(new T(), serializer);
		static public void DeserializeTest<T>(IConstantLengthSerializer<T> serializer)
		{
			ISerializerTest.DeserializeTest(serializer.Count, serializer);
			_ = ArrayHelper.EnsureLength(ref _buffer, serializer.Count);
			_ = Assert.ThrowsException<ArgumentNullException>(() => serializer.Deserialize(null, 0x0));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Deserialize(_buffer, -0x1));
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Deserialize(_buffer, _buffer.Length + 0x1));
			int index = 0x0;
			_ = Assert.ThrowsException<ArgumentNullException>(() => serializer.Deserialize(null, ref index));
			index = -0x1;
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Deserialize(_buffer, ref index));
			index = _buffer.Length + 0x1;
			_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => serializer.Deserialize(_buffer, ref index));
		}
		static public void CommonTest<T>(T instance, IConstantLengthSerializer<T> serializer, IEqualityComparer<T> comparer)
		{
			ISerializerTest.CommonTest(instance, serializer, comparer);
			int count = serializer.Count;
			int index = count;
			_ = ArrayHelper.EnsureLength(ref _buffer, index + count);
			serializer.Serialize(instance, _buffer, index);
			Assert.IsTrue(comparer.Equals(instance, serializer.Deserialize(_buffer, index)));
			serializer.Serialize(instance, _buffer, ref index);
			Assert.IsTrue(index == count << 0x1);
			index = count;
			Assert.IsTrue(comparer.Equals(instance, serializer.Deserialize(_buffer, ref index)));
			Assert.IsTrue(index == count << 0x1);
		}
		static public void CommonTest<T>(T instance, IConstantLengthSerializer<T> serializer) => CommonTest(instance, serializer, EqualityComparer<T>.Default);
	}
}