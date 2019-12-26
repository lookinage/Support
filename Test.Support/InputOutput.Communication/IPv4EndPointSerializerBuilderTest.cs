using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.InputOutput.Communication;
using Test.Support.Coding.Serialization;

namespace Test.Support.InputOutput.Communication
{
	[TestClass]
	public class IPv4EndPointSerializerBuilderTest
	{
		static public void SerializeTest(IConstantLengthSerializer<IPv4EndPoint> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<IPv4EndPoint> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest(IConstantLengthSerializer<IPv4EndPoint> serializer)
		{
			byte[] buffer = new byte[serializer.Count];
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				IPv4EndPoint instance = new IPv4EndPoint((uint)PseudoRandomManager.GetInt32(), (ushort)PseudoRandomManager.GetInt32());
				serializer.Serialize(instance, buffer, 0x0);
				Assert.IsTrue(serializer.Deserialize(buffer, 0x0) == instance);
			}
		}


		[TestMethod]
		public void SerializeTest() => SerializeTest(IPv4EndPointSerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(IPv4EndPointSerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(IPv4EndPointSerializerBuilder.Default);
	}
}