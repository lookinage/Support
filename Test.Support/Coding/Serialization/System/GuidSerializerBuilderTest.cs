using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class GuidSerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<Guid> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<Guid> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<Guid> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
				IConstantLengthSerializerTest.CommonTest(Guid.NewGuid(), serializer);
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(GuidSerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(GuidSerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(GuidSerializerBuilder.Default);
	}
}