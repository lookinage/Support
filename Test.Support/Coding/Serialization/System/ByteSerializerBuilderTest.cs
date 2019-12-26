using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class ByteSerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<byte> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<byte> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<byte> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				byte instance = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(byte); byteIndex++)
					instance |= (byte)((byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte));
				IConstantLengthSerializerTest.CommonTest(instance, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(ByteSerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(ByteSerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(ByteSerializerBuilder.Default);
	}
}