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
	public class HashSetSerializerBuilderTest
	{
		static public void SerializeTest<T>(ISerializer<HashSet<T>> serializer) => ISerializerTest.SerializeTest(serializer);
		static public void DeserializeTest<T>(ISerializer<HashSet<T>> serializer) => ISerializerTest.DeserializeTest(0x0, serializer);
		static public void CommonTest<T>(ISerializer<HashSet<T>> serializer, Func<T> randomElementGenerator)
		{
			for (int testIndex = 0x0; testIndex != IEnumerableSerializerBuilderTest._testCount; testIndex++)
			{
				HashSet<T> instance = new HashSet<T>();
				for (int elementIndex = 0x0; elementIndex != IEnumerableSerializerBuilderTest._testLenght; elementIndex++)
					_ = instance.Add(randomElementGenerator());
				int count = serializer.Count(instance);
				byte[] buffer = new byte[count];
				serializer.Serialize(instance, buffer, 0x0);
				IEnumerableSerializerBuilderTest.ValidateDeserialization(instance, serializer.Deserialize(count, buffer, 0x0));
			}
		}

		[TestMethod]
		public void CreateSerializerTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => HashSetSerializerBuilder.CreateSerializer<int>(null));
			_ = HashSetSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default);
		}
		[TestMethod]
		public void SerializeTest()
		{
			SerializeTest(HashSetSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			SerializeTest(HashSetSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void DeserializeTest()
		{
			DeserializeTest(HashSetSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			DeserializeTest(HashSetSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void CommonTest()
		{
			CommonTest(HashSetSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default), Int32SerializerBuilderTest.GenerateRandomNumber);
			CommonTest(HashSetSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default), StringSerializerBuilderTest.GenerateRandomEnString);
		}
	}
}