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
	public class QueueSerializerBuilderTest
	{
		static public void SerializeTest<T>(ISerializer<Queue<T>> serializer) => ISerializerTest.SerializeTest(serializer);
		static public void DeserializeTest<T>(ISerializer<Queue<T>> serializer) => ISerializerTest.DeserializeTest(0x0, serializer);
		static public void CommonTest<T>(ISerializer<Queue<T>> serializer, Func<T> randomElementGenerator)
		{
			for (int testIndex = 0x0; testIndex != IEnumerableSerializerBuilderTest._testCount; testIndex++)
			{
				Queue<T> instance = new Queue<T>(IEnumerableSerializerBuilderTest._testLenght);
				for (int elementIndex = 0x0; elementIndex != IEnumerableSerializerBuilderTest._testLenght; elementIndex++)
					instance.Enqueue(randomElementGenerator());
				int count = serializer.Count(instance);
				byte[] buffer = new byte[count];
				serializer.Serialize(instance, buffer, 0x0);
				IEnumerableSerializerBuilderTest.ValidateDeserialization(instance, serializer.Deserialize(count, buffer, 0x0));
			}
		}

		[TestMethod]
		public void CreateSerializerTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => QueueSerializerBuilder.CreateSerializer<int>(null));
			_ = QueueSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default);
		}
		[TestMethod]
		public void SerializeTest()
		{
			SerializeTest(QueueSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			SerializeTest(QueueSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void DeserializeTest()
		{
			DeserializeTest(QueueSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			DeserializeTest(QueueSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void CommonTest()
		{
			CommonTest(QueueSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default), Int32SerializerBuilderTest.GenerateRandomNumber);
			CommonTest(QueueSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default), StringSerializerBuilderTest.GenerateRandomEnString);
		}
	}
}