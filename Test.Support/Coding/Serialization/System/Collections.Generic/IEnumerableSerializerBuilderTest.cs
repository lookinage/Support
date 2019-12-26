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
	public class IEnumerableSerializerBuilderTest
	{
		internal const int _testLenght = 0x10;
		internal const int _testCount = 0x10;

		static public void SerializeTest<T>(ISerializer<IEnumerable<T>> serializer) => ISerializerTest.SerializeTest(new Stack<T>(), serializer);
		static public void DeserializeTest<T>(ISerializer<IEnumerable<T>> serializer) => ISerializerTest.DeserializeTest(0x0, serializer);
		static internal void ValidateDeserialization<T>(IEnumerable<T> instance, IEnumerable<T> deserializedInstance, IEqualityComparer<T> comparer)
		{
			if (instance == null)
			{
				Assert.IsTrue(deserializedInstance == null);
				return;
			}
			IEnumerator<T> instanceEnumerator = instance.GetEnumerator();
			IEnumerator<T> deserializedInstanceEnumerator = instance.GetEnumerator();
			bool iterate;
			for (Assert.IsTrue((iterate = instanceEnumerator.MoveNext()) == deserializedInstanceEnumerator.MoveNext()); iterate; Assert.IsTrue((iterate = instanceEnumerator.MoveNext()) == deserializedInstanceEnumerator.MoveNext()))
				Assert.IsTrue(comparer.Equals(instanceEnumerator.Current, deserializedInstanceEnumerator.Current));
		}
		static internal void ValidateDeserialization<T>(IEnumerable<T> instance, IEnumerable<T> deserializedInstance) => ValidateDeserialization(instance, deserializedInstance, EqualityComparer<T>.Default);
		static public void CommonTest<T>(ISerializer<IEnumerable<T>> serializer, Func<T> randomElementGenerator)
		{
			for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
			{
				T[] instance = new T[_testLenght];
				for (int elementIndex = 0x0; elementIndex != _testLenght; elementIndex++)
					instance[elementIndex] = randomElementGenerator();
				int count = serializer.Count(instance);
				byte[] buffer = new byte[count];
				serializer.Serialize(instance, buffer, 0x0);
				ValidateDeserialization(instance, serializer.Deserialize(count, buffer, 0x0));
			}
		}

		[TestMethod]
		public void CreateSerializerTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => IEnumerableSerializerBuilder.CreateSerializer<int>(null));
			_ = IEnumerableSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default);
		}
		[TestMethod]
		public void SerializeTest()
		{
			SerializeTest(IEnumerableSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			SerializeTest(IEnumerableSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void DeserializeTest()
		{
			DeserializeTest(IEnumerableSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default));
			DeserializeTest(IEnumerableSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default));
		}
		[TestMethod]
		public void CommonTest()
		{
			CommonTest(IEnumerableSerializerBuilder.CreateSerializer(Int32SerializerBuilder.Default), Int32SerializerBuilderTest.GenerateRandomNumber);
			CommonTest(IEnumerableSerializerBuilder.CreateSerializer(StringSerializerBuilder.Default), StringSerializerBuilderTest.GenerateRandomEnString);
		}
	}
}