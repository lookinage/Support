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
	public class ISetSerializerBuilderTest
	{
		static public void SerializeTest<T>(ISerializer<ISet<T>> serializer) => ISerializerTest.SerializeTest(new HashSet<T>(), serializer);
		static public void DeserializeTest<T>(ISerializer<ISet<T>> serializer) => ISerializerTest.DeserializeTest(0x0, serializer);
		static public void CommonTest<T>(ISerializer<ISet<T>> serializer, Func<T> randomElementGenerator)
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
			_ = Assert.ThrowsException<ArgumentNullException>(() => ISetSerializerBuilder.CreateSerializer<int>(null));
			_ = ISetSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default);
		}
		[TestMethod]
		public void SerializeTest()
		{
			SerializeTest(ISetSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			SerializeTest(ISetSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void DeserializeTest()
		{
			DeserializeTest(ISetSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			DeserializeTest(ISetSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void CommonTest()
		{
			CommonTest(ISetSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default), Int32SerializerBuilderTest.GenerateRandomNumber);
			CommonTest(ISetSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default), StringSerializerBuilderTest.GenerateRandomEnString);
		}
	}
}