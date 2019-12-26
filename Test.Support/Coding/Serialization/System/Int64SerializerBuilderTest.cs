using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class Int64SerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<long> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<long> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<long> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				long instance = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(long); byteIndex++)
					instance |= (long)(byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte);
				IConstantLengthSerializerTest.CommonTest(instance, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(Int64SerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(Int64SerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(Int64SerializerBuilder.Default);
	}
}