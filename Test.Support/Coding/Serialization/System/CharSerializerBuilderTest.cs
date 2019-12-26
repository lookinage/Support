using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class CharSerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<char> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<char> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<char> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				char instance = '\0';
				for (int byteIndex = 0x0; byteIndex != sizeof(char); byteIndex++)
					instance |= (char)((char)(byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte));
				IConstantLengthSerializerTest.CommonTest(instance, serializer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(CharSerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(CharSerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(CharSerializerBuilder.Default);
	}
}