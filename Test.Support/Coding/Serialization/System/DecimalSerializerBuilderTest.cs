using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class DecimalSerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<decimal> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<decimal> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public unsafe void CommonTest(IConstantLengthSerializer<decimal> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				long value = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(long); byteIndex++)
					value |= (long)(byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte);
				decimal instance;
				*(long*)&instance = *&value;
				value = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(long); byteIndex++)
					value |= (long)(byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte);
				*((long*)&instance + 0x1) = *&value;
				IConstantLengthSerializerTest.CommonTest(instance, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(DecimalSerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(DecimalSerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(DecimalSerializerBuilder.Default);
	}
}