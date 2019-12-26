using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class UInt64SerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<ulong> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<ulong> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<ulong> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				ulong instance = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(ulong); byteIndex++)
					instance |= (ulong)(byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte);
				IConstantLengthSerializerTest.CommonTest(instance, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(UInt64SerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(UInt64SerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(UInt64SerializerBuilder.Default);
	}
}