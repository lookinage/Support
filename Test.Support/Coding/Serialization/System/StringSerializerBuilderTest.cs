using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using System;
using System.Text;

namespace Test.Support.Coding.Serialization.System
{
	[TestClass]
	public class StringSerializerBuilderTest
	{
		static public string GenerateRandomEnString()
		{
			int length = PseudoRandomManager.GetNonNegativeInt32(short.MaxValue);
			char[] chars = new char[length];
			for (int charIndex = 0x0; charIndex != length; charIndex++)
			{
				int rnd = PseudoRandomManager.GetInt32Remainder(0x3);
				chars[charIndex] = (char)(rnd == 0x0 ? PseudoRandomManager.GetNonNegativeInt32('a', 'z') : rnd == 0x1 ? PseudoRandomManager.GetNonNegativeInt32('A', 'Z') : PseudoRandomManager.GetNonNegativeInt32('0', '9'));
			}
			return new string(chars);
		}
		static public string GenerateRandomEnRuString()
		{
			int length = PseudoRandomManager.GetNonNegativeInt32(short.MaxValue);
			char[] chars = new char[length];
			for (int charIndex = 0x0; charIndex != length; charIndex++)
			{
				int rnd = PseudoRandomManager.GetInt32Remainder(5);
				chars[charIndex] = (char)(rnd == 0x0 ? PseudoRandomManager.GetNonNegativeInt32('a', 'z') : rnd == 0x1 ? PseudoRandomManager.GetNonNegativeInt32('A', 'Z') : rnd == 0x2 ? PseudoRandomManager.GetNonNegativeInt32('а', 'я') : rnd == 0x3 ? PseudoRandomManager.GetNonNegativeInt32('А', 'Я') : PseudoRandomManager.GetNonNegativeInt32('0', '9'));
			}
			return new string(chars);
		}
		static public void SerializeTest(ISerializer<string> serializer) => ISerializerTest.SerializeTest(string.Empty, serializer);
		static public void DeserializeTest(ISerializer<string> serializer) => ISerializerTest.DeserializeTest(0x0, serializer);
		static public void CommonTest(ISerializer<string> serializer, Func<string> randomStringGenerator)
		{
			for (byte testIndex = byte.MinValue; testIndex != byte.MaxValue; testIndex++)
				ISerializerTest.CommonTest(randomStringGenerator(), serializer);
		}

		[TestMethod]
		public void CreateSerializerTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => StringSerializerBuilder.CreateSerializer(null));
			_ = StringSerializerBuilder.CreateSerializer(Encoding.Default);
		}
		[TestMethod]
		public void SerializeTest()
		{
			SerializeTest(StringSerializerBuilder.UTF8);
			SerializeTest(StringSerializerBuilder.UTF7);
			SerializeTest(StringSerializerBuilder.UTF32);
			SerializeTest(StringSerializerBuilder.Unicode);
			SerializeTest(StringSerializerBuilder.BigEndianUnicode);
			SerializeTest(StringSerializerBuilder.ASCII);
		}
		[TestMethod]
		public void DeserializeTest()
		{
			DeserializeTest(StringSerializerBuilder.UTF8);
			DeserializeTest(StringSerializerBuilder.UTF7);
			DeserializeTest(StringSerializerBuilder.UTF32);
			DeserializeTest(StringSerializerBuilder.Unicode);
			DeserializeTest(StringSerializerBuilder.BigEndianUnicode);
			DeserializeTest(StringSerializerBuilder.ASCII);
		}
		[TestMethod]
		public void CommonTest()
		{
			CommonTest(StringSerializerBuilder.UTF8, GenerateRandomEnRuString);
			CommonTest(StringSerializerBuilder.UTF7, GenerateRandomEnRuString);
			CommonTest(StringSerializerBuilder.UTF32, GenerateRandomEnRuString);
			CommonTest(StringSerializerBuilder.Unicode, GenerateRandomEnRuString);
			CommonTest(StringSerializerBuilder.BigEndianUnicode, GenerateRandomEnRuString);
			CommonTest(StringSerializerBuilder.ASCII, GenerateRandomEnString);
		}
	}
}