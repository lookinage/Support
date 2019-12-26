using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using System.Collections.Generic;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class DoubleSerializerBuilderTest
	{
		private struct EqualityComparer : IEqualityComparer<double>
		{
			static internal EqualityComparer _equalityComparer;

			static EqualityComparer() => _equalityComparer = new EqualityComparer();

			bool IEqualityComparer<double>.Equals(double x, double y) => double.IsNaN(x) ? double.IsNaN(y) : x == y;
			int IEqualityComparer<double>.GetHashCode(double obj) => obj.GetHashCode();
		}

		static public void SerializeTest(IConstantLengthSerializer<double> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest(IConstantLengthSerializer<double> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public unsafe void CommonTest(IConstantLengthSerializer<double> serializer)
		{
			for (ushort testIndex = ushort.MinValue; testIndex != ushort.MaxValue; testIndex++)
			{
				long value = 0x0;
				for (int byteIndex = 0x0; byteIndex != sizeof(long); byteIndex++)
					value |= (long)(byte)PseudoRandomManager.GetInt32() << byteIndex * 0x8 * sizeof(byte);
				IConstantLengthSerializerTest.CommonTest(*(double*)&value, serializer, EqualityComparer._equalityComparer);
			}
		}

		[TestMethod]
		public void SerializeTest() => SerializeTest(DoubleSerializerBuilder.Default);
		[TestMethod]
		public void DeserializeTest() => DeserializeTest(DoubleSerializerBuilder.Default);
		[TestMethod]
		public void CommonTest() => CommonTest(DoubleSerializerBuilder.Default);
	}
}