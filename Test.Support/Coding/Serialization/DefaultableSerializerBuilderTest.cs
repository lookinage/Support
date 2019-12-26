using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using System;
using Test.Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization
{
	[TestClass]
	public class DefaultableSerializerBuilderTest
	{
		static public void SerializeTest(ISerializer<string> serializer) => ISerializerTest.SerializeTest(null, serializer);
		static public void DeserializeTest(ISerializer<string> serializer) => ISerializerTest.DeserializeTest(0x0, serializer);
		static private void CommonTest(ISerializer<string> serializer)
		{
			for (byte testIndex = byte.MinValue; testIndex != byte.MaxValue; testIndex++)
				ISerializerTest.CommonTest(PseudoRandomManager.GetInt32() % 0x2 == 0x0 ? StringSerializerBuilderTest.GenerateRandomEnString() : default, serializer);
		}

		[TestMethod]
		public void CreateSerializerTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => DefaultableSerializerBuilder.CreateSerializer<string>(null));
			_ = DefaultableSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default);
		}
		[TestMethod]
		public void SerializeTest() => SerializeTest(DefaultableSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(DefaultableSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		[TestMethod]
		public void CommonTest() => CommonTest(DefaultableSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
	}
}