using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class UInt32SerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<uint> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<uint> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<uint> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				uint instance = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(uint); byteIndex++)
					instance |= (uint)(byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte);
				IConstantLengthSerializerTest.CommonTest(instance, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(UInt32SerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(UInt32SerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(UInt32SerializerBuilder.Default);
	}
}