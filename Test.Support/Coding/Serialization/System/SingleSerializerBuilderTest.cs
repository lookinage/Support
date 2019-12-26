using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using System.Collections.Generic;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class SingleSerializerBuilderTest
	{
		private struct EqailityComparer : IEqualityComparer<float>
		{
			static internal EqailityComparer _eqailityComparer;

			static EqailityComparer() => _eqailityComparer = new EqailityComparer();

			bool IEqualityComparer<float>.Equals(float x, float y) => float.IsNaN(x) ? float.IsNaN(y) : x == y;
			int IEqualityComparer<float>.GetHashCode(float obj) => obj.GetHashCode();
		}

		static public void SerializeTest(IConstantLengthSerializer<float> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<float> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public unsafe void CommonTest(IConstantLengthSerializer<float> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				int value = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(int); byteIndex++)
					value |= (byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte);
				IConstantLengthSerializerTest.CommonTest(*(float*)&value, serializer, EqailityComparer._eqailityComparer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(SingleSerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(SingleSerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(SingleSerializerBuilder.Default);
	}
}