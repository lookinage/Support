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
	public class KeyValuePairSerializerBuilderTest
	{
		static public void SerializeTest<TKey, TValue>(ISerializer<KeyValuePair<TKey, TValue>> serializer) => ISerializerTest.SerializeTest(serializer);
		static public void DeserializeTest<TKey, TValue>(ISerializer<KeyValuePair<TKey, TValue>> serializer) => ISerializerTest.DeserializeTest(0x0, serializer);
		static public void SerializeTest<TKey, TValue>(IConstantLengthSerializer<KeyValuePair<TKey, TValue>> serializer) => IConstantLengthSerializerTest.SerializeTest(serializer);
		static public void DeserializeTest<TKey, TValue>(IConstantLengthSerializer<KeyValuePair<TKey, TValue>> serializer) => IConstantLengthSerializerTest.DeserializeTest(serializer);
		static public void CommonTest<TKey, TValue>(ISerializer<KeyValuePair<TKey, TValue>> serializer, Func<TKey> randomKeyGenerator, Func<TValue> randomValueGenerator, IEqualityComparer<KeyValuePair<TKey, TValue>> comparer)
		{
			for (byte i = byte.MinValue; i < byte.MaxValue; i++)
			{
				KeyValuePair<TKey, TValue> instance = new KeyValuePair<TKey, TValue>(randomKeyGenerator(), randomValueGenerator());
				int count = serializer.Count(instance);
				byte[] buffer = new byte[count];
				serializer.Serialize(instance, buffer, 0x0);
				Assert.IsTrue(comparer.Equals(instance, serializer.Deserialize(count, buffer, 0x0)));
			}
		}
		static public void CommonTest<TKey, TValue>(ISerializer<KeyValuePair<TKey, TValue>> serializer, Func<TKey> randomKeyGenerator, Func<TValue> randomValueGenerator) => CommonTest(serializer, randomKeyGenerator, randomValueGenerator, EqualityComparer<KeyValuePair<TKey, TValue>>.Default);

		[TestMethod]
		public void CreateSerializerTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => KeyValuePairSerializerBuilder.CreateSerializer<int, int>(null, Int32SerializerBuilder.Default));
			_ = Assert.ThrowsException<ArgumentNullException>(() => KeyValuePairSerializerBuilder.CreateSerializer<int, int>(Int32SerializerBuilder.Default, null));
			_ = KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, Int32SerializerBuilder.Default);
		}
		[TestMethod]
		public void SerializeTest()
		{
			SerializeTest(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, Int32SerializerBuilder.Default));
			SerializeTest(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, StringSerializerBuilder.Default));
			SerializeTest(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, Int32SerializerBuilder.Default));
			SerializeTest(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void DeserializeTest()
		{
			DeserializeTest(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, Int32SerializerBuilder.Default));
			DeserializeTest(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, StringSerializerBuilder.Default));
			DeserializeTest(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, Int32SerializerBuilder.Default));
			DeserializeTest(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void CommonTest()
		{
			CommonTest(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, Int32SerializerBuilder.Default), Int32SerializerBuilderTest.GenerateRandomNumber, Int32SerializerBuilderTest.GenerateRandomNumber);
			CommonTest(KeyValuePairSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default, StringSerializerBuilder.Default), Int32SerializerBuilderTest.GenerateRandomNumber, StringSerializerBuilderTest.GenerateRandomEnString);
			CommonTest(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, Int32SerializerBuilder.Default), StringSerializerBuilderTest.GenerateRandomEnString, Int32SerializerBuilderTest.GenerateRandomNumber);
			CommonTest(KeyValuePairSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default, StringSerializerBuilder.Default), StringSerializerBuilderTest.GenerateRandomEnString, StringSerializerBuilderTest.GenerateRandomEnString);
		}
	}
}