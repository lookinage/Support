using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class SByteSerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<sbyte> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<sbyte> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<sbyte> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				sbyte instance = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(sbyte); byteIndex++)
					instance |= (sbyte)((sbyte)(byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte));
				IConstantLengthSerializerTest.CommonTest(instance, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(SByteSerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(SByteSerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(SByteSerializerBuilder.Default);
	}
}