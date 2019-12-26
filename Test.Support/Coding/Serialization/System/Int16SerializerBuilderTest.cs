using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class Int16SerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<short> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<short> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<short> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				short instance = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(short); byteIndex++)
					instance |= (short)((byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte));
				IConstantLengthSerializerTest.CommonTest(instance, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(Int16SerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(Int16SerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(Int16SerializerBuilder.Default);
	}
}