using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class DateTimeSerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<DateTime> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<DateTime> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public unsafe void CommonTest(IConstantLengthSerializer<DateTime> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				long value = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(long); byteIndex++)
					value |= (long)(byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte);
				IConstantLengthSerializerTest.CommonTest(*(DateTime*)&value, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(DateTimeSerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(DateTimeSerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(DateTimeSerializerBuilder.Default);
	}
}