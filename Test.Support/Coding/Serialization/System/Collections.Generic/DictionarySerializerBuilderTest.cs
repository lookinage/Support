using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using Support.Coding.Serialization.System.Collections.Generic;
using System;
using System.Collections.Generic;
using Test.Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.Collections.Generic
{
	[TestClass]
	public class DictionarySerializerBuilderTest
	{
		static public void SerializeTest<TKey, TValue>(ISerializer<Dictionary<TKey, TValue>> serializer) => ISerializerTest.SerializeTest(serializer);
		static public void DeserializeTest<TKey, TValue>(ISerializer<Dictionary<TKey, TValue>> serializer) => ISerializerTest.DeserializeTest(0x0, serializer);
		static public void CommonTest<TKey, TValue>(ISerializer<Dictionary<TKey, TValue>> serializer, Func<TKey> randomKeyGenerator, Func<TValue> randomValueGenerator)
		{
			for (int testIndex = 0x0; testIndex != IEnumerableSerializerBuilderTest._testCount; testIndex++)
			{
				Dictionary<TKey, TValue> instance = new Dictionary<TKey, TValue>(IEnumerableSerializerBuilderTest._testLenght);
				for (int elementIndex = 0x0; elementIndex != IEnumerableSerializerBuilderTest._testLenght; elementIndex++)
					instance.Add(randomKeyGenerator(), randomValueGenerator());
				int count = serializer.Count(instance);
				byte[] buffer = new byte[count];
				serializer.Serialize(instance, buffer, 0x0);
				IEnumerableSerializerBuilderTest.ValidateDeserialization(instance, serializer.Deserialize(count, buffer, 0x0));
			}
		}

		[TestMethod]
		public void CreateSerializerTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => DictionarySerializerBuilder.CreateSerializer<int, int>(null));
			_ = DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, Int32SerializerBuilder.Default));
		}
		[TestMethod]
		public void SerializeTest()
		{
			SerializeTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, Int32SerializerBuilder.Default)));
			SerializeTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, StringSerializerBuilder.Default)));
			SerializeTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, Int32SerializerBuilder.Default)));
			SerializeTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, StringSerializerBuilder.Default)));
		}
		[TestMethod]
		public void DeserializeTest()
		{
			DeserializeTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, Int32SerializerBuilder.Default)));
			DeserializeTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, StringSerializerBuilder.Default)));
			DeserializeTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, Int32SerializerBuilder.Default)));
			DeserializeTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, StringSerializerBuilder.Default)));
		}
		[TestMethod]
		public void CommonTest()
		{
			CommonTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, Int32SerializerBuilder.Default)), Int32SerializerBuilderTest.GenerateRandomNumber, Int32SerializerBuilderTest.GenerateRandomNumber);
			CommonTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, StringSerializerBuilder.Default)), Int32SerializerBuilderTest.GenerateRandomNumber, StringSerializerBuilderTest.GenerateRandomEnString);
			CommonTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, Int32SerializerBuilder.Default)), StringSerializerBuilderTest.GenerateRandomEnString, Int32SerializerBuilderTest.GenerateRandomNumber);
			CommonTest(DictionarySerializerBuilder.CreateSerializer(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, StringSerializerBuilder.Default)), StringSerializerBuilderTest.GenerateRandomEnString, StringSerializerBuilderTest.GenerateRandomEnString);
		}
	}
}