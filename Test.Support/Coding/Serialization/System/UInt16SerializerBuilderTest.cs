using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class UInt16SerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<ushort> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<ushort> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<ushort> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				ushort instance = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(ushort); byteIndex++)
					instance |= (ushort)((byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte));
				IConstantLengthSerializerTest.CommonTest(instance, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(UInt16SerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(UInt16SerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(UInt16SerializerBuilder.Default);
	}
}