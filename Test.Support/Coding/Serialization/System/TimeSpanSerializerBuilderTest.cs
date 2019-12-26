using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class TimeSpanSerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<TimeSpan> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<TimeSpan> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public unsafe void CommonTest(IConstantLengthSerializer<TimeSpan> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				long value = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(long); byteIndex++)
					value |= (long)(byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte);
				IConstantLengthSerializerTest.CommonTest(*(TimeSpan*)&value, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(TimeSpanSerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(TimeSpanSerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(TimeSpanSerializerBuilder.Default);
	}
}