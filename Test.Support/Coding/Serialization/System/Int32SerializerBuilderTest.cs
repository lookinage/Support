using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class Int32SerializerBuilderTest
	{
		static public int GenerateRandomNumber() => PseudoRandomManager.GetInt32();
		static public void SerializeTest(IConstantLengthSerializer<int> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<int> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<int> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				int instance = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(int); byteIndex++)
					instance |= (byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte);
				IConstantLengthSerializerTest.CommonTest(instance, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(Int32SerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(Int32SerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(Int32SerializerBuilder.Default);
	}
}