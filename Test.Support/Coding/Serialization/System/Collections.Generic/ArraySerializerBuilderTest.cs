using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using Support.Coding.Serialization.System.Collections.Generic;
using System;
using Test.Support.Coding.Serialization.System;

namespace Test.Support.Coding.Serialization.Collections.Generic
{
	[TestClass]
	public class ArraySerializerBuilderTest
	{
		static public void SerializeTest<T>(ISerializer<T[]> serializer) => ISerializerTest.SerializeTest(new T[0x0], serializer);
		static public void DeserializeTest<T>(ISerializer<T[]> serializer) => ISerializerTest.DeserializeTest(0x0, serializer);
		static public void CommonTest<T>(ISerializer<T[]> serializer, Func<T> randomElementGenerator)
		{
			for (int testIndex = 0x0; testIndex != IEnumerableSerializerBuilderTest._testCount; testIndex++)
			{
				T[] instance = new T[IEnumerableSerializerBuilderTest._testLenght];
				for (int elementIndex = 0x0; elementIndex != IEnumerableSerializerBuilderTest._testLenght; elementIndex++)
					instance[elementIndex] = randomElementGenerator();
				int count = serializer.Count(instance);
				byte[] buffer = new byte[count];
				serializer.Serialize(instance, buffer, 0x0);
				IEnumerableSerializerBuilderTest.ValidateDeserialization(instance, serializer.Deserialize(count, buffer, 0x0));
			}
		}

		[TestMethod]
		public void CreateSerializerTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => ArraySerializerBuilder.CreateSerializer<int>(null));
			_ = ArraySerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default);
		}
		[TestMethod]
		public void SerializeTest()
		{
			SerializeTest(ArraySerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			SerializeTest(ArraySerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void DeserializeTest()
		{
			DeserializeTest(ArraySerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			DeserializeTest(ArraySerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void CommonTest()
		{
			CommonTest(ArraySerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default), Int32SerializerBuilderTest.GenerateRandomNumber);
			CommonTest(ArraySerializerBuilder.CreateSerializer(StringSerializerBuilder.Default), StringSerializerBuilderTest.GenerateRandomEnString);
		}
	}
}