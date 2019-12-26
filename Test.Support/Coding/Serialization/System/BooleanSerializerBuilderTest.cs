using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class BooleanSerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<bool> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<bool> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<bool> serializer)
		{
			IConstantLengthSerializerTest.CommonTest(false, serializer);
			IConstantLengthSerializerTest.CommonTest(true, serializer);
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(BooleanSerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(BooleanSerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(BooleanSerializerBuilder.Default);
	}
}