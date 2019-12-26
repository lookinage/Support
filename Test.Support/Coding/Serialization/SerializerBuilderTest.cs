using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using Support.InputOutput.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Test.Support.Coding.Serialization.Collections.Generic;
using Test.Support.Coding.Serialization.System;
using Test.Support.InputOutput.Communication;

namespace Test.Support.Coding.Serialization
{
	[TestClass]
	public class SerializerBuilderTest
	{
		private struct FailStruct
		{

		}
		public class FailClass1
		{
			private FailClass1() { }
		}
		public abstract class FailClass2
		{
			public readonly int Field1;
		}
		public struct SuitableStruct1
		{
			static internal readonly int _staticField;

			static SuitableStruct1() => _staticField = 0x0;

			internal int _failField;
			public readonly int ConstantLengthField;
			public string VariableLengthField;

			internal SuitableStruct1(int failField)
			{
				_failField = failField;
				ConstantLengthField = default;
				VariableLengthField = default;
			}

			public int ConstantLengthMethod() => 0x0;
			public string VariableLengthMethod() => null;
		}
		public struct SuitableStruct2
		{
			static internal readonly int _staticField;

			static SuitableStruct2() => _staticField = 0x0;

			internal int _failField;
			public readonly int ConstantLengthField;
			public string VariableLengthField;

			internal SuitableStruct2(int failField)
			{
				_failField = failField;
				ConstantLengthField = default;
				VariableLengthField = default;
			}

			public int ConstantLengthMethod() => 0x0;
			public string VariableLengthMethod() => null;
		}
		[Serializable]
		public struct Struct
		{
			internal struct EqualityComparer : IEqualityComparer<Struct>
			{
				static internal readonly EqualityComparer _equalityComparer;

				static EqualityComparer() => _equalityComparer = new EqualityComparer();

				bool IEqualityComparer<Struct>.Equals(Struct x, Struct y) => x.ConstantLengthField1 == y.ConstantLengthField1 && x.ConstantLengthField2 == y.ConstantLengthField2 && x.VariableLengthField1 == y.VariableLengthField1 && x.VariableLengthField2 == y.VariableLengthField2;
				int IEqualityComparer<Struct>.GetHashCode(Struct obj) => obj.GetHashCode();
			}
			internal struct ConstantLengthEqualityComparer : IEqualityComparer<Struct>
			{
				static internal readonly ConstantLengthEqualityComparer _equalityComparer;

				static ConstantLengthEqualityComparer() => _equalityComparer = new ConstantLengthEqualityComparer();

				bool IEqualityComparer<Struct>.Equals(Struct x, Struct y) => x.ConstantLengthField1 == y.ConstantLengthField1 && x.ConstantLengthField2 == y.ConstantLengthField2;
				int IEqualityComparer<Struct>.GetHashCode(Struct obj) => obj.GetHashCode();
			}
			internal struct VariableLengthEqualityComparer : IEqualityComparer<Struct>
			{
				static internal readonly VariableLengthEqualityComparer _equalityComparer;

				static VariableLengthEqualityComparer() => _equalityComparer = new VariableLengthEqualityComparer();

				bool IEqualityComparer<Struct>.Equals(Struct x, Struct y) => x.VariableLengthField1 == y.VariableLengthField1 && x.VariableLengthField2 == y.VariableLengthField2;
				int IEqualityComparer<Struct>.GetHashCode(Struct obj) => obj.GetHashCode();
			}

			static internal Struct GenerateRandomStruct() => new Struct(Int32SerializerBuilderTest.GenerateRandomNumber()) { ConstantLengthField2 = Int32SerializerBuilderTest.GenerateRandomNumber(), VariableLengthField1 = StringSerializerBuilderTest.GenerateRandomEnString(), VariableLengthField2 = StringSerializerBuilderTest.GenerateRandomEnString() };

			public readonly int ConstantLengthField1;
			public int ConstantLengthField2;
			public string VariableLengthField1;
			public string VariableLengthField2;

			public Struct(int constantLengthField1)
			{
				ConstantLengthField1 = constantLengthField1;
				ConstantLengthField2 = default;
				VariableLengthField1 = default;
				VariableLengthField2 = default;
			}
		}
		public class Class
		{
			internal struct EqualityComparer : IEqualityComparer<Class>
			{
				static private readonly IEqualityComparer<Class> _defaultEqualityComparer;
				static internal readonly EqualityComparer _equalityComparer;

				static EqualityComparer()
				{
					_defaultEqualityComparer = EqualityComparer<Class>.Default;
					_equalityComparer = new EqualityComparer();
				}

				bool IEqualityComparer<Class>.Equals(Class x, Class y) => x == null ? y == null : y != null && x.ConstantLengthField1 == y.ConstantLengthField1 && x.ConstantLengthField2 == y.ConstantLengthField2 && x.VariableLengthField1 == y.VariableLengthField1 && x.VariableLengthField2 == y.VariableLengthField2;
				int IEqualityComparer<Class>.GetHashCode(Class obj) => _defaultEqualityComparer.GetHashCode(obj);
			}
			internal struct ConstantLengthEqualityComparer : IEqualityComparer<Class>
			{
				static private readonly IEqualityComparer<Class> _defaultEqualityComparer;
				static internal readonly ConstantLengthEqualityComparer _equalityComparer;

				static ConstantLengthEqualityComparer()
				{
					_defaultEqualityComparer = EqualityComparer<Class>.Default;
					_equalityComparer = new ConstantLengthEqualityComparer();
				}

				bool IEqualityComparer<Class>.Equals(Class x, Class y) => x == null ? y == null : y != null && x.ConstantLengthField1 == y.ConstantLengthField1 && x.ConstantLengthField2 == y.ConstantLengthField2;
				int IEqualityComparer<Class>.GetHashCode(Class obj) => _defaultEqualityComparer.GetHashCode(obj);
			}
			internal struct VariableLengthEqualityComparer : IEqualityComparer<Class>
			{
				static private readonly IEqualityComparer<Class> _defaultEqualityComparer;
				static internal readonly VariableLengthEqualityComparer _equalityComparer;

				static VariableLengthEqualityComparer()
				{
					_defaultEqualityComparer = EqualityComparer<Class>.Default;
					_equalityComparer = new VariableLengthEqualityComparer();
				}

				bool IEqualityComparer<Class>.Equals(Class x, Class y) => x == null ? y == null : y != null && x.VariableLengthField1 == y.VariableLengthField1 && x.VariableLengthField2 == y.VariableLengthField2;
				int IEqualityComparer<Class>.GetHashCode(Class obj) => _defaultEqualityComparer.GetHashCode(obj);
			}

			static internal Class GenerateRandomDefaultClass() => PseudoRandomManager.GetInt32() % 0x2 == 0x0 ? null : new Class(Int32SerializerBuilderTest.GenerateRandomNumber()) { ConstantLengthField2 = Int32SerializerBuilderTest.GenerateRandomNumber(), VariableLengthField1 = StringSerializerBuilderTest.GenerateRandomEnString(), VariableLengthField2 = StringSerializerBuilderTest.GenerateRandomEnString() };
			static internal Class GenerateRandomClass() => new Class(Int32SerializerBuilderTest.GenerateRandomNumber()) { ConstantLengthField2 = Int32SerializerBuilderTest.GenerateRandomNumber(), VariableLengthField1 = StringSerializerBuilderTest.GenerateRandomEnString(), VariableLengthField2 = StringSerializerBuilderTest.GenerateRandomEnString() };

			public readonly int ConstantLengthField1;
			public int ConstantLengthField2;
			public string VariableLengthField1;
			public string VariableLengthField2;

			public Class() { }
			public Class(int constantLengthField1) => ConstantLengthField1 = constantLengthField1;
		}

		private const int _testCount = byte.MaxValue;

		[TestMethod]
		public void DefaultSerializeTest()
		{
			BooleanSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<bool>)SerializerBuilder<bool>.Default);
			ByteSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<byte>)SerializerBuilder<byte>.Default);
			SByteSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<sbyte>)SerializerBuilder<sbyte>.Default);
			Int16SerializerBuilderTest.SerializeTest((IConstantLengthSerializer<short>)SerializerBuilder<short>.Default);
			UInt16SerializerBuilderTest.SerializeTest((IConstantLengthSerializer<ushort>)SerializerBuilder<ushort>.Default);
			CharSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<char>)SerializerBuilder<char>.Default);
			Int32SerializerBuilderTest.SerializeTest((IConstantLengthSerializer<int>)SerializerBuilder<int>.Default);
			UInt32SerializerBuilderTest.SerializeTest((IConstantLengthSerializer<uint>)SerializerBuilder<uint>.Default);
			Int64SerializerBuilderTest.SerializeTest((IConstantLengthSerializer<long>)SerializerBuilder<long>.Default);
			UInt64SerializerBuilderTest.SerializeTest((IConstantLengthSerializer<ulong>)SerializerBuilder<ulong>.Default);
			DecimalSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<decimal>)SerializerBuilder<decimal>.Default);
			SingleSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<float>)SerializerBuilder<float>.Default);
			DoubleSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<double>)SerializerBuilder<double>.Default);
			DateTimeSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<DateTime>)SerializerBuilder<DateTime>.Default);
			TimeSpanSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<TimeSpan>)SerializerBuilder<TimeSpan>.Default);
			GuidSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<Guid>)SerializerBuilder<Guid>.Default);
			IPv4EndPointSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<IPv4EndPoint>)SerializerBuilder<IPv4EndPoint>.Default);
			StringSerializerBuilderTest.SerializeTest(SerializerBuilder<string>.Default);
			NullableSerializerBuilderTest.SerializeTest(SerializerBuilder<Nullable<Int32>>.Default);
			KeyValuePairSerializerBuilderTest.SerializeTest((IConstantLengthSerializer<KeyValuePair<int, int>>)SerializerBuilder<KeyValuePair<int, int>>.Default);
			KeyValuePairSerializerBuilderTest.SerializeTest(SerializerBuilder<KeyValuePair<int, string>>.Default);
			KeyValuePairSerializerBuilderTest.SerializeTest(SerializerBuilder<KeyValuePair<string, int>>.Default);
			KeyValuePairSerializerBuilderTest.SerializeTest(SerializerBuilder<KeyValuePair<string, string>>.Default);
			IEnumerableSerializerBuilderTest.SerializeTest(SerializerBuilder<IEnumerable<int>>.Default);
			IEnumerableSerializerBuilderTest.SerializeTest(SerializerBuilder<IEnumerable<string>>.Default);
			ICollectionSerializerBuilderTest.SerializeTest(SerializerBuilder<ICollection<int>>.Default);
			ICollectionSerializerBuilderTest.SerializeTest(SerializerBuilder<ICollection<string>>.Default);
			IListSerializerBuilderTest.SerializeTest(SerializerBuilder<IList<int>>.Default);
			IListSerializerBuilderTest.SerializeTest(SerializerBuilder<IList<string>>.Default);
			ISetSerializerBuilderTest.SerializeTest(SerializerBuilder<ISet<int>>.Default);
			ISetSerializerBuilderTest.SerializeTest(SerializerBuilder<ISet<string>>.Default);
			ArraySerializerBuilderTest.SerializeTest(SerializerBuilder<int[]>.Default);
			ArraySerializerBuilderTest.SerializeTest(SerializerBuilder<string[]>.Default);
			QueueSerializerBuilderTest.SerializeTest(SerializerBuilder<Queue<int>>.Default);
			QueueSerializerBuilderTest.SerializeTest(SerializerBuilder<Queue<string>>.Default);
			StackSerializerBuilderTest.SerializeTest(SerializerBuilder<Stack<int>>.Default);
			StackSerializerBuilderTest.SerializeTest(SerializerBuilder<Stack<string>>.Default);
			DictionarySerializerBuilderTest.SerializeTest(SerializerBuilder<Dictionary<int, int>>.Default);
			DictionarySerializerBuilderTest.SerializeTest(SerializerBuilder<Dictionary<int, string>>.Default);
			DictionarySerializerBuilderTest.SerializeTest(SerializerBuilder<Dictionary<string, int>>.Default);
			DictionarySerializerBuilderTest.SerializeTest(SerializerBuilder<Dictionary<string, string>>.Default);
			ListSerializerBuilderTest.SerializeTest(SerializerBuilder<List<int>>.Default);
			ListSerializerBuilderTest.SerializeTest(SerializerBuilder<List<string>>.Default);
			HashSetSerializerBuilderTest.SerializeTest(SerializerBuilder<HashSet<int>>.Default);
			HashSetSerializerBuilderTest.SerializeTest(SerializerBuilder<HashSet<string>>.Default);
		}
		[TestMethod]
		public void DefaultDeserializeTest()
		{
			BooleanSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<bool>)SerializerBuilder<bool>.Default);
			ByteSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<byte>)SerializerBuilder<byte>.Default);
			SByteSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<sbyte>)SerializerBuilder<sbyte>.Default);
			Int16SerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<short>)SerializerBuilder<short>.Default);
			UInt16SerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<ushort>)SerializerBuilder<ushort>.Default);
			CharSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<char>)SerializerBuilder<char>.Default);
			Int32SerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<int>)SerializerBuilder<int>.Default);
			UInt32SerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<uint>)SerializerBuilder<uint>.Default);
			Int64SerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<long>)SerializerBuilder<long>.Default);
			UInt64SerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<ulong>)SerializerBuilder<ulong>.Default);
			DecimalSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<decimal>)SerializerBuilder<decimal>.Default);
			SingleSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<float>)SerializerBuilder<float>.Default);
			DoubleSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<double>)SerializerBuilder<double>.Default);
			DateTimeSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<DateTime>)SerializerBuilder<DateTime>.Default);
			TimeSpanSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<TimeSpan>)SerializerBuilder<TimeSpan>.Default);
			GuidSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<Guid>)SerializerBuilder<Guid>.Default);
			IPv4EndPointSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<IPv4EndPoint>)SerializerBuilder<IPv4EndPoint>.Default);
			StringSerializerBuilderTest.DeserializeTest(SerializerBuilder<string>.Default);
			NullableSerializerBuilderTest.DeserializeTest(SerializerBuilder<Nullable<Int32>>.Default);
			KeyValuePairSerializerBuilderTest.DeserializeTest((IConstantLengthSerializer<KeyValuePair<int, int>>)SerializerBuilder<KeyValuePair<int, int>>.Default);
			KeyValuePairSerializerBuilderTest.DeserializeTest(SerializerBuilder<KeyValuePair<int, string>>.Default);
			KeyValuePairSerializerBuilderTest.DeserializeTest(SerializerBuilder<KeyValuePair<string, int>>.Default);
			KeyValuePairSerializerBuilderTest.DeserializeTest(SerializerBuilder<KeyValuePair<string, string>>.Default);
			IEnumerableSerializerBuilderTest.DeserializeTest(SerializerBuilder<IEnumerable<int>>.Default);
			IEnumerableSerializerBuilderTest.DeserializeTest(SerializerBuilder<IEnumerable<string>>.Default);
			ICollectionSerializerBuilderTest.DeserializeTest(SerializerBuilder<ICollection<int>>.Default);
			ICollectionSerializerBuilderTest.DeserializeTest(SerializerBuilder<ICollection<string>>.Default);
			IListSerializerBuilderTest.DeserializeTest(SerializerBuilder<IList<int>>.Default);
			IListSerializerBuilderTest.DeserializeTest(SerializerBuilder<IList<string>>.Default);
			ISetSerializerBuilderTest.DeserializeTest(SerializerBuilder<ISet<int>>.Default);
			ISetSerializerBuilderTest.DeserializeTest(SerializerBuilder<ISet<string>>.Default);
			ArraySerializerBuilderTest.DeserializeTest(SerializerBuilder<int[]>.Default);
			ArraySerializerBuilderTest.DeserializeTest(SerializerBuilder<string[]>.Default);
			QueueSerializerBuilderTest.DeserializeTest(SerializerBuilder<Queue<int>>.Default);
			QueueSerializerBuilderTest.DeserializeTest(SerializerBuilder<Queue<string>>.Default);
			StackSerializerBuilderTest.DeserializeTest(SerializerBuilder<Stack<int>>.Default);
			StackSerializerBuilderTest.DeserializeTest(SerializerBuilder<Stack<string>>.Default);
			DictionarySerializerBuilderTest.DeserializeTest(SerializerBuilder<Dictionary<int, int>>.Default);
			DictionarySerializerBuilderTest.DeserializeTest(SerializerBuilder<Dictionary<int, string>>.Default);
			DictionarySerializerBuilderTest.DeserializeTest(SerializerBuilder<Dictionary<string, int>>.Default);
			DictionarySerializerBuilderTest.DeserializeTest(SerializerBuilder<Dictionary<string, string>>.Default);
			ListSerializerBuilderTest.DeserializeTest(SerializerBuilder<List<int>>.Default);
			ListSerializerBuilderTest.DeserializeTest(SerializerBuilder<List<string>>.Default);
			HashSetSerializerBuilderTest.DeserializeTest(SerializerBuilder<HashSet<int>>.Default);
			HashSetSerializerBuilderTest.DeserializeTest(SerializerBuilder<HashSet<string>>.Default);
		}
		[TestMethod]
		public void DefaultCommonTest()
		{
			BooleanSerializerBuilderTest.CommonTest((IConstantLengthSerializer<bool>)SerializerBuilder<bool>.Default);
			ByteSerializerBuilderTest.CommonTest((IConstantLengthSerializer<byte>)SerializerBuilder<byte>.Default);
			SByteSerializerBuilderTest.CommonTest((IConstantLengthSerializer<sbyte>)SerializerBuilder<sbyte>.Default);
			Int16SerializerBuilderTest.CommonTest((IConstantLengthSerializer<short>)SerializerBuilder<short>.Default);
			UInt16SerializerBuilderTest.CommonTest((IConstantLengthSerializer<ushort>)SerializerBuilder<ushort>.Default);
			CharSerializerBuilderTest.CommonTest((IConstantLengthSerializer<char>)SerializerBuilder<char>.Default);
			Int32SerializerBuilderTest.CommonTest((IConstantLengthSerializer<int>)SerializerBuilder<int>.Default);
			UInt32SerializerBuilderTest.CommonTest((IConstantLengthSerializer<uint>)SerializerBuilder<uint>.Default);
			Int64SerializerBuilderTest.CommonTest((IConstantLengthSerializer<long>)SerializerBuilder<long>.Default);
			UInt64SerializerBuilderTest.CommonTest((IConstantLengthSerializer<ulong>)SerializerBuilder<ulong>.Default);
			DecimalSerializerBuilderTest.CommonTest((IConstantLengthSerializer<decimal>)SerializerBuilder<decimal>.Default);
			SingleSerializerBuilderTest.CommonTest((IConstantLengthSerializer<float>)SerializerBuilder<float>.Default);
			DoubleSerializerBuilderTest.CommonTest((IConstantLengthSerializer<double>)SerializerBuilder<double>.Default);
			DateTimeSerializerBuilderTest.CommonTest((IConstantLengthSerializer<DateTime>)SerializerBuilder<DateTime>.Default);
			TimeSpanSerializerBuilderTest.CommonTest((IConstantLengthSerializer<TimeSpan>)SerializerBuilder<TimeSpan>.Default);
			GuidSerializerBuilderTest.CommonTest((IConstantLengthSerializer<Guid>)SerializerBuilder<Guid>.Default);
			IPv4EndPointSerializerBuilderTest.CommonTest((IConstantLengthSerializer<IPv4EndPoint>)SerializerBuilder<IPv4EndPoint>.Default);
			StringSerializerBuilderTest.CommonTest(SerializerBuilder<string>.Default, StringSerializerBuilderTest.GenerateRandomEnString);
			NullableSerializerBuilderTest.CommonTest(SerializerBuilder<Nullable<Int32>>.Default);
			KeyValuePairSerializerBuilderTest.CommonTest((IConstantLengthSerializer<KeyValuePair<int, int>>)SerializerBuilder<KeyValuePair<int, int>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber, Int32SerializerBuilderTest.GenerateRandomNumber);
			KeyValuePairSerializerBuilderTest.CommonTest(SerializerBuilder<KeyValuePair<int, string>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber, StringSerializerBuilderTest.GenerateRandomEnString);
			KeyValuePairSerializerBuilderTest.CommonTest(SerializerBuilder<KeyValuePair<string, int>>.Default, StringSerializerBuilderTest.GenerateRandomEnString, Int32SerializerBuilderTest.GenerateRandomNumber);
			KeyValuePairSerializerBuilderTest.CommonTest(SerializerBuilder<KeyValuePair<string, string>>.Default, StringSerializerBuilderTest.GenerateRandomEnString, StringSerializerBuilderTest.GenerateRandomEnString);
			IEnumerableSerializerBuilderTest.CommonTest(SerializerBuilder<IEnumerable<int>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber);
			IEnumerableSerializerBuilderTest.CommonTest(SerializerBuilder<IEnumerable<string>>.Default, StringSerializerBuilderTest.GenerateRandomEnString);
			ICollectionSerializerBuilderTest.CommonTest(SerializerBuilder<ICollection<int>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber);
			ICollectionSerializerBuilderTest.CommonTest(SerializerBuilder<ICollection<string>>.Default, StringSerializerBuilderTest.GenerateRandomEnString);
			IListSerializerBuilderTest.CommonTest(SerializerBuilder<IList<int>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber);
			IListSerializerBuilderTest.CommonTest(SerializerBuilder<IList<string>>.Default, StringSerializerBuilderTest.GenerateRandomEnString);
			ISetSerializerBuilderTest.CommonTest(SerializerBuilder<ISet<int>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber);
			ISetSerializerBuilderTest.CommonTest(SerializerBuilder<ISet<string>>.Default, StringSerializerBuilderTest.GenerateRandomEnString);
			ArraySerializerBuilderTest.CommonTest(SerializerBuilder<int[]>.Default, Int32SerializerBuilderTest.GenerateRandomNumber);
			ArraySerializerBuilderTest.CommonTest(SerializerBuilder<string[]>.Default, StringSerializerBuilderTest.GenerateRandomEnString);
			QueueSerializerBuilderTest.CommonTest(SerializerBuilder<Queue<int>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber);
			QueueSerializerBuilderTest.CommonTest(SerializerBuilder<Queue<string>>.Default, StringSerializerBuilderTest.GenerateRandomEnString);
			StackSerializerBuilderTest.CommonTest(SerializerBuilder<Stack<int>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber);
			StackSerializerBuilderTest.CommonTest(SerializerBuilder<Stack<string>>.Default, StringSerializerBuilderTest.GenerateRandomEnString);
			DictionarySerializerBuilderTest.CommonTest(SerializerBuilder<Dictionary<int, int>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber, Int32SerializerBuilderTest.GenerateRandomNumber);
			DictionarySerializerBuilderTest.CommonTest(SerializerBuilder<Dictionary<int, string>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber, StringSerializerBuilderTest.GenerateRandomEnString);
			DictionarySerializerBuilderTest.CommonTest(SerializerBuilder<Dictionary<string, int>>.Default, StringSerializerBuilderTest.GenerateRandomEnString, Int32SerializerBuilderTest.GenerateRandomNumber);
			DictionarySerializerBuilderTest.CommonTest(SerializerBuilder<Dictionary<string, string>>.Default, StringSerializerBuilderTest.GenerateRandomEnString, StringSerializerBuilderTest.GenerateRandomEnString);
			ListSerializerBuilderTest.CommonTest(SerializerBuilder<List<int>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber);
			ListSerializerBuilderTest.CommonTest(SerializerBuilder<List<string>>.Default, StringSerializerBuilderTest.GenerateRandomEnString);
			HashSetSerializerBuilderTest.CommonTest(SerializerBuilder<HashSet<int>>.Default, Int32SerializerBuilderTest.GenerateRandomNumber);
			HashSetSerializerBuilderTest.CommonTest(SerializerBuilder<HashSet<string>>.Default, StringSerializerBuilderTest.GenerateRandomEnString);
		}
		[TestMethod]
		public void StructDefaultSerializeTest() => ISerializerTest.SerializeTest(SerializerBuilder<Struct>.Default);
		[TestMethod]
		public void StructDefaultDeserializeTest() => ISerializerTest.DeserializeTest(0x0, SerializerBuilder<Struct>.Default);
		[TestMethod]
		public void StructDefaultCommonTest()
		{
			for (byte testIndex = 0x0; testIndex != _testCount; testIndex++)
				ISerializerTest.CommonTest(Struct.GenerateRandomStruct(), SerializerBuilder<Struct>.Default, Struct.EqualityComparer._equalityComparer);
		}
		[TestMethod]
		public void StructConstantLengthSerializeTest()
		{
			SerializerBuilder<Struct> builder = new SerializerBuilder<Struct>();
			builder.AddField(x => x.ConstantLengthField1, Int32SerializerBuilder.Default);
			builder.AddField(x => x.ConstantLengthField2, Int32SerializerBuilder.Default);
			IConstantLengthSerializerTest.SerializeTest((IConstantLengthSerializer<Struct>)builder.CreateSerializer());
		}
		[TestMethod]
		public void StructConstantLengthDeserializeTest()
		{
			SerializerBuilder<Struct> builder = new SerializerBuilder<Struct>();
			builder.AddField(x => x.ConstantLengthField1, Int32SerializerBuilder.Default);
			builder.AddField(x => x.ConstantLengthField2, Int32SerializerBuilder.Default);
			IConstantLengthSerializerTest.DeserializeTest((IConstantLengthSerializer<Struct>)builder.CreateSerializer());
		}
		[TestMethod]
		public void StructConstantLengthCommonTest()
		{
			SerializerBuilder<Struct> builder = new SerializerBuilder<Struct>();
			builder.AddField(x => x.ConstantLengthField1, Int32SerializerBuilder.Default);
			builder.AddField(x => x.ConstantLengthField2, Int32SerializerBuilder.Default);
			for (byte testIndex = 0x0; testIndex != _testCount; testIndex++)
			{
				IConstantLengthSerializerTest.CommonTest(Struct.GenerateRandomStruct(), (IConstantLengthSerializer<Struct>)builder.CreateSerializer(), Struct.ConstantLengthEqualityComparer._equalityComparer);
			MustThrow:
				try { _ = Assert.ThrowsException<AssertFailedException>(() => IConstantLengthSerializerTest.CommonTest(Struct.GenerateRandomStruct(), (IConstantLengthSerializer<Struct>)builder.CreateSerializer(), Struct.EqualityComparer._equalityComparer)); }
				catch { goto MustThrow; }
			}
		}
		[TestMethod]
		public void StructVariableLengthSerializeTest()
		{
			SerializerBuilder<Struct> builder = new SerializerBuilder<Struct>();
			builder.AddField(x => x.VariableLengthField1, StringSerializerBuilder.Default);
			builder.AddField(x => x.VariableLengthField2, StringSerializerBuilder.Default);
			ISerializerTest.SerializeTest(builder.CreateSerializer());
		}
		[TestMethod]
		public void StructVariableLengthDeserializeTest()
		{
			SerializerBuilder<Struct> builder = new SerializerBuilder<Struct>();
			builder.AddField(x => x.VariableLengthField1, StringSerializerBuilder.Default);
			builder.AddField(x => x.VariableLengthField2, StringSerializerBuilder.Default);
			ISerializerTest.DeserializeTest(0x0, builder.CreateSerializer());
		}
		[TestMethod]
		public void StructVariableLengthCommonTest()
		{
			SerializerBuilder<Struct> builder = new SerializerBuilder<Struct>();
			builder.AddField(x => x.VariableLengthField1, StringSerializerBuilder.Default);
			builder.AddField(x => x.VariableLengthField2, StringSerializerBuilder.Default);
			for (byte testIndex = 0x0; testIndex != _testCount; testIndex++)
			{
				ISerializerTest.CommonTest(Struct.GenerateRandomStruct(), builder.CreateSerializer(), Struct.VariableLengthEqualityComparer._equalityComparer);
			MustThrow:
				try { _ = Assert.ThrowsException<AssertFailedException>(() => ISerializerTest.CommonTest(Struct.GenerateRandomStruct(), builder.CreateSerializer(), Struct.EqualityComparer._equalityComparer)); }
				catch { goto MustThrow; }
			}
		}
		[TestMethod]
		public void ClassDefaultSerializeTest() => ISerializerTest.SerializeTest(SerializerBuilder<Class>.Default);
		[TestMethod]
		public void ClassDefaultDeserializeTest() => ISerializerTest.DeserializeTest(0x0, SerializerBuilder<Class>.Default);
		[TestMethod]
		public void ClassDefaultCommonTest()
		{
			for (byte testIndex = 0x0; testIndex != _testCount; testIndex++)
				ISerializerTest.CommonTest(Class.GenerateRandomDefaultClass(), SerializerBuilder<Class>.Default, Class.EqualityComparer._equalityComparer);
		}
		[TestMethod]
		public void ClassConstantLengthSerializeTest()
		{
			SerializerBuilder<Class> builder = new SerializerBuilder<Class>();
			builder.AddField(x => x.ConstantLengthField1, Int32SerializerBuilder.Default);
			builder.AddField(x => x.ConstantLengthField2, Int32SerializerBuilder.Default);
			IConstantLengthSerializerTest.SerializeTest((IConstantLengthSerializer<Class>)builder.CreateSerializer());
		}
		[TestMethod]
		public void ClassConstantLengthDeserializeTest()
		{
			SerializerBuilder<Class> builder = new SerializerBuilder<Class>();
			builder.AddField(x => x.ConstantLengthField1, Int32SerializerBuilder.Default);
			builder.AddField(x => x.ConstantLengthField2, Int32SerializerBuilder.Default);
			IConstantLengthSerializerTest.DeserializeTest((IConstantLengthSerializer<Class>)builder.CreateSerializer());
		}
		[TestMethod]
		public void ClassConstantLengthCommonTest()
		{
			SerializerBuilder<Class> builder = new SerializerBuilder<Class>();
			builder.AddField(x => x.ConstantLengthField1, Int32SerializerBuilder.Default);
			builder.AddField(x => x.ConstantLengthField2, Int32SerializerBuilder.Default);
			for (byte testIndex = 0x0; testIndex != _testCount; testIndex++)
			{
				IConstantLengthSerializerTest.CommonTest(Class.GenerateRandomClass(), (IConstantLengthSerializer<Class>)builder.CreateSerializer(), Class.ConstantLengthEqualityComparer._equalityComparer);
			MustThrow:
				try { _ = Assert.ThrowsException<AssertFailedException>(() => IConstantLengthSerializerTest.CommonTest(Class.GenerateRandomClass(), (IConstantLengthSerializer<Class>)builder.CreateSerializer(), Class.EqualityComparer._equalityComparer)); }
				catch { goto MustThrow; }
			}
		}
		[TestMethod]
		public void ClassVariableLengthSerializeTest()
		{
			SerializerBuilder<Class> builder = new SerializerBuilder<Class>();
			builder.AddField(x => x.VariableLengthField1, StringSerializerBuilder.Default);
			builder.AddField(x => x.VariableLengthField2, StringSerializerBuilder.Default);
			ISerializerTest.SerializeTest(builder.CreateSerializer());
		}
		[TestMethod]
		public void ClassVariableLengthDeserializeTest()
		{
			SerializerBuilder<Class> builder = new SerializerBuilder<Class>();
			builder.AddField(x => x.VariableLengthField1, StringSerializerBuilder.Default);
			builder.AddField(x => x.VariableLengthField2, StringSerializerBuilder.Default);
			ISerializerTest.DeserializeTest(0x0, builder.CreateSerializer());
		}
		[TestMethod]
		public void ClassVariableLengthCommonTest()
		{
			SerializerBuilder<Class> builder = new SerializerBuilder<Class>();
			builder.AddField(x => x.VariableLengthField1, StringSerializerBuilder.Default);
			builder.AddField(x => x.VariableLengthField2, StringSerializerBuilder.Default);
			for (byte testIndex = 0x0; testIndex != _testCount; testIndex++)
			{
				ISerializerTest.CommonTest(Class.GenerateRandomClass(), builder.CreateSerializer(), Class.VariableLengthEqualityComparer._equalityComparer);
			MustThrow:
				try { _ = Assert.ThrowsException<AssertFailedException>(() => ISerializerTest.CommonTest(Class.GenerateRandomClass(), builder.CreateSerializer(), Class.EqualityComparer._equalityComparer)); }
				catch { goto MustThrow; }
			}
		}
		[TestMethod()]
		public void ConstructorTest()
		{
			_ = Assert.ThrowsException<InvalidOperationException>(() => _ = new SerializerBuilder<FailStruct>());
			_ = Assert.ThrowsException<InvalidOperationException>(() => _ = new SerializerBuilder<FailClass1>());
			_ = Assert.ThrowsException<InvalidOperationException>(() => _ = new SerializerBuilder<FailClass2>());
			_ = new SerializerBuilder<Struct>();
		}
		[TestMethod()]
		public void AddFieldTest()
		{
			SerializerBuilder<SuitableStruct1> builder = new SerializerBuilder<SuitableStruct1>();
			_ = Assert.ThrowsException<ArgumentNullException>(() => builder.AddField((FieldInfo)null, Int32SerializerBuilder.Default));
			_ = Assert.ThrowsException<ArgumentNullException>(() => builder.AddField(typeof(SuitableStruct1).GetField(nameof(SuitableStruct1.ConstantLengthField)), null));
			_ = Assert.ThrowsException<ArgumentException>(() => builder.AddField(typeof(SuitableStruct1).GetField(nameof(SuitableStruct1._staticField), BindingFlags.Static | BindingFlags.NonPublic), Int32SerializerBuilder.Default));
			_ = Assert.ThrowsException<ArgumentException>(() => builder.AddField(typeof(SuitableStruct1).GetField(nameof(SuitableStruct1._failField), BindingFlags.Instance | BindingFlags.NonPublic), StringSerializerBuilder.Default));
			_ = Assert.ThrowsException<ArgumentException>(() => builder.AddField(typeof(SuitableStruct2).GetField(nameof(SuitableStruct2.ConstantLengthField)), Int32SerializerBuilder.Default));
			_ = Assert.ThrowsException<ArgumentException>(() => builder.AddField(typeof(SuitableStruct1).GetField(nameof(SuitableStruct1.ConstantLengthField)), 0x0));
			_ = Assert.ThrowsException<ArgumentException>(() => builder.AddField(typeof(SuitableStruct1).GetField(nameof(SuitableStruct1.ConstantLengthField)), StringSerializerBuilder.Default));
			_ = Assert.ThrowsException<ArgumentNullException>(() => builder.AddField(null, Int32SerializerBuilder.Default));
			_ = Assert.ThrowsException<ArgumentNullException>(() => builder.AddField(x => x.ConstantLengthField, null));
			_ = Assert.ThrowsException<ArgumentException>(() => builder.AddField(x => SuitableStruct1._staticField, Int32SerializerBuilder.Default));
			_ = Assert.ThrowsException<ArgumentException>(() => builder.AddField(x => x._failField, Int32SerializerBuilder.Default));
			_ = Assert.ThrowsException<ArgumentException>(() => builder.AddField(x => x.ConstantLengthMethod(), Int32SerializerBuilder.Default));
			_ = Assert.ThrowsException<ArgumentException>(() => builder.AddField(x => new SuitableStruct2().ConstantLengthField, Int32SerializerBuilder.Default));
			builder.AddField(x => x.ConstantLengthField, Int32SerializerBuilder.Default);
			_ = Assert.ThrowsException<ArgumentException>(() => builder.AddField(x => x.ConstantLengthField, Int32SerializerBuilder.Default));
			builder.AddField(x => x.VariableLengthField, StringSerializerBuilder.Default);
			_ = Assert.ThrowsException<ArgumentException>(() => builder.AddField(x => x.VariableLengthField, StringSerializerBuilder.Default));
		}

		static public Struct S;

		[TestMethod]
		public void MyTestMethod()
		{
			Struct s = new Struct(54345) { ConstantLengthField2 = 4535, VariableLengthField1 = "fhghjhgf", VariableLengthField2 = "jytrrtyuiuygf" };
			ISerializer<Struct> serializer = SerializerBuilder<Struct>.Default;
			byte[] bytes = new byte[serializer.Count(s)];
			for (int i = 0; i < 10000; i++)
			{
				serializer.Serialize(s, bytes, 0x0);
				S = serializer.Deserialize(bytes.Length, bytes, 0x0);
			}
			//MemoryStream stream = new MemoryStream();
			//BinaryFormatter formatter = new BinaryFormatter();
			//for (int i = 0; i < 10000; i++)
			//{
			//	formatter.Serialize(stream, s);
			//	stream.Position = 0;
			//	S = (Struct)formatter.Deserialize(stream);
			//}
		}
	}
}