using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class NullableSerializerBuilderTest
	{
		static public void SerializeTest(ISerializer<Nullable<Int32>> serializer) => ISerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(ISerializer<Nullable<Int32>> serializer) => ISerializerTest.DeserializeTest(0x0, serializer);
		static public void CommonTest(ISerializer<Nullable<Int32>> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
				ISerializerTest.CommonTest(PseudoRandomManager.GetInt32() % 0x2 == 0x0 ? PseudoRandomManager.GetInt32() : default, serializer);
		}

		[TestMethod]
		public void CreateSerializerTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => NullableSerializerBuilder.CreateSerializer<int>(null));
			_ = NullableSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default);
		}
		[TestMethod]
		public void SerializeTest() => SerializeTest(NullableSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(NullableSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
		[TestMethod]
		public void CommonTest() => CommonTest(NullableSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
	}
}