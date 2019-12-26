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
	public class IListSerializerBuilderTest
	{
		static public void SerializeTest<T>(ISerializer<IList<T>> serializer) => ISerializerTest.SerializeTest(new List<T>(), serializer);
		static public void DeserializeTest<T>(ISerializer<IList<T>> serializer) => ISerializerTest.DeserializeTest(0x0, serializer);
		static public void CommonTest<T>(ISerializer<IList<T>> serializer, Func<T> randomElementGenerator)
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
			_ = Assert.ThrowsException<ArgumentNullException>(() => IListSerializerBuilder.CreateSerializer<int>(null));
			_ = IListSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default);
		}
		[TestMethod]
		public void SerializeTest()
		{
			SerializeTest(IListSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			SerializeTest(IListSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void DeserializeTest()
		{
			DeserializeTest(IListSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			DeserializeTest(IListSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void CommonTest()
		{
			CommonTest(IListSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default), Int32SerializerBuilderTest.GenerateRandomNumber);
			CommonTest(IListSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default), StringSerializerBuilderTest.GenerateRandomEnString);
		}
	}
}