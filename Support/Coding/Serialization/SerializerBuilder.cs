using Support.Coding.Serialization.System;
using Support.Coding.Serialization.System.Collections.Generic;
using Support.InputOutput.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Support.Coding.Serialization
{
	static internal class SerializerBuilder
	{
		static private readonly Dictionary<AppDomain, ModuleBuilder> _moduleBuilders;
		static internal readonly Type _objectType;
		static internal readonly Type _voidType;
		static internal readonly Type _booleanType;
		static internal readonly Type _byteType;
		static internal readonly Type _sByteType;
		static internal readonly Type _int16Type;
		static internal readonly Type _uInt16Type;
		static internal readonly Type _charType;
		static internal readonly Type _int32Type;
		static internal readonly Type _uInt32Type;
		static internal readonly Type _int64Type;
		static internal readonly Type _uInt64Type;
		static internal readonly Type _decimalType;
		static internal readonly Type _singleType;
		static internal readonly Type _doubleType;
		static internal readonly Type _dateTimeType;
		static internal readonly Type _timeSpanType;
		static internal readonly Type _guidType;
		static internal readonly Type _ipv4EndPoint;
		static internal readonly Type _stringType;
		static internal readonly Type _nullableTypeDefinition;
		static internal readonly Type _keyValuePairTypeDefinition;
		static internal readonly Type _iEnumerableTypeDefinition;
		static internal readonly Type _iCollectionTypeDefinition;
		static internal readonly Type _iListTypeDefinition;
		static internal readonly Type _iSetTypeDefinition;
		static internal readonly Type _queueTypeDefinition;
		static internal readonly Type _stackTypeDefinition;
		static internal readonly Type _dictionaryTypeDefinition;
		static internal readonly Type _listTypeDefinition;
		static internal readonly Type _hashSetTypeDefinition;
		static internal readonly Type _byteArrayType;
		static internal readonly Type _int32ByRefType;
		static internal readonly Type _iSerializerTypeDefinition;
		static internal readonly Type _iConstantLengthSerializerTypeDefinition;
		static internal readonly Type _serializerTypeDefinition;
		static internal readonly Type _constantLengthSerializerTypeDefinition;
		static internal readonly Type _serializerBuilderTypeDefinition;
		static internal readonly FieldInfo _int32SerializerBuilderTypeInstanceFieldInfo;
		static internal readonly ConstructorInfo _objectTypeConstructorInfo;
		static internal readonly ConstructorInfo _argumentNullExceptionTypeConstructorInfo;
		static internal readonly MethodInfo _constantLengthSerializerInt32TypeCountPropertyGetMethodInfo;
		static internal readonly MethodInfo _constantLengthSerializerInt32TypeSerializeMethodInfo;
		static internal readonly MethodInfo _constantLengthSerializerInt32TypeDeserializeMethodInfo;
		static internal readonly MethodInfo _defaultableSerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _nullableSerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _keyValuePairSerializerBuilderTypeCreateSerializerKeyConstantLengthValueConstantLengthMethodDefinition;
		static internal readonly MethodInfo _keyValuePairSerializerBuilderTypeCreateSerializerKeyConstantLengthMethodDefinition;
		static internal readonly MethodInfo _keyValuePairSerializerBuilderTypeCreateSerializerValueConstantLengthMethodDefinition;
		static internal readonly MethodInfo _keyValuePairSerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _iEnumerableSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition;
		static internal readonly MethodInfo _iEnumerableSerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _iCollectionSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition;
		static internal readonly MethodInfo _iCollectionSerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _iListSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition;
		static internal readonly MethodInfo _iListSerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _iSetSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition;
		static internal readonly MethodInfo _iSetSerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _arraySerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition;
		static internal readonly MethodInfo _arraySerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _queueSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition;
		static internal readonly MethodInfo _queueSerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _stackSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition;
		static internal readonly MethodInfo _stackSerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _dictionarySerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition;
		static internal readonly MethodInfo _dictionarySerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _listSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition;
		static internal readonly MethodInfo _listSerializerBuilderTypeCreateSerializerMethodDefinition;
		static internal readonly MethodInfo _hashSetSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition;
		static internal readonly MethodInfo _hashSetSerializerBuilderTypeCreateSerializerMethodDefinition;

		static SerializerBuilder()
		{
			_moduleBuilders = new Dictionary<AppDomain, ModuleBuilder>();
			_objectType = typeof(object);
			_voidType = typeof(void);
			_booleanType = typeof(bool);
			_byteType = typeof(byte);
			_sByteType = typeof(sbyte);
			_int16Type = typeof(short);
			_uInt16Type = typeof(ushort);
			_charType = typeof(char);
			_int32Type = typeof(int);
			_uInt32Type = typeof(uint);
			_int64Type = typeof(long);
			_uInt64Type = typeof(ulong);
			_decimalType = typeof(decimal);
			_singleType = typeof(float);
			_doubleType = typeof(double);
			_dateTimeType = typeof(DateTime);
			_timeSpanType = typeof(TimeSpan);
			_guidType = typeof(Guid);
			_ipv4EndPoint = typeof(IPv4EndPoint);
			_stringType = typeof(string);
			_nullableTypeDefinition = typeof(Nullable<>);
			_keyValuePairTypeDefinition = typeof(KeyValuePair<,>);
			_iEnumerableTypeDefinition = typeof(IEnumerable<>);
			_iCollectionTypeDefinition = typeof(ICollection<>);
			_iListTypeDefinition = typeof(IList<>);
			_iSetTypeDefinition = typeof(ISet<>);
			_queueTypeDefinition = typeof(Queue<>);
			_stackTypeDefinition = typeof(Stack<>);
			_dictionaryTypeDefinition = typeof(Dictionary<,>);
			_listTypeDefinition = typeof(List<>);
			_hashSetTypeDefinition = typeof(HashSet<>);
			_byteArrayType = typeof(byte[]);
			_int32ByRefType = _int32Type.MakeByRefType();
			_iSerializerTypeDefinition = typeof(ISerializer<>);
			_iConstantLengthSerializerTypeDefinition = typeof(IConstantLengthSerializer<>);
			_serializerTypeDefinition = typeof(Serializer<>);
			_constantLengthSerializerTypeDefinition = typeof(ConstantLengthSerializer<>);
			_serializerBuilderTypeDefinition = typeof(SerializerBuilder<>);
			_int32SerializerBuilderTypeInstanceFieldInfo = typeof(Int32SerializerBuilder).GetField(nameof(Int32SerializerBuilder.Default));
			Type constantLengthSerializerInt32Type = typeof(ConstantLengthSerializer<int>);
			_objectTypeConstructorInfo = _objectType.GetConstructor(Type.EmptyTypes);
			_argumentNullExceptionTypeConstructorInfo = typeof(ArgumentNullException).GetConstructor(new Type[] { _stringType });
			_constantLengthSerializerInt32TypeCountPropertyGetMethodInfo = constantLengthSerializerInt32Type.GetProperty(nameof(ConstantLengthSerializer<int>.Count)).GetGetMethod();
			_constantLengthSerializerInt32TypeSerializeMethodInfo = constantLengthSerializerInt32Type.GetMethod(nameof(ConstantLengthSerializer<int>.Serialize), new Type[] { _int32Type, _byteArrayType, _int32Type });
			_constantLengthSerializerInt32TypeDeserializeMethodInfo = constantLengthSerializerInt32Type.GetMethod(nameof(ConstantLengthSerializer<int>.Deserialize), new Type[] { _byteArrayType, _int32ByRefType });
			Type defaultableSerializerBuilderType = typeof(DefaultableSerializerBuilder);
			_defaultableSerializerBuilderTypeCreateSerializerMethodDefinition = defaultableSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			Type nullableSerializerBuilderType = typeof(NullableSerializerBuilder);
			_nullableSerializerBuilderTypeCreateSerializerMethodDefinition = nullableSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			Type keyValuePairSerializerBuilderType = typeof(KeyValuePairSerializerBuilder);
			_keyValuePairSerializerBuilderTypeCreateSerializerKeyConstantLengthValueConstantLengthMethodDefinition = keyValuePairSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]) && x.GetParameters()[0x1].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x1]));
			_keyValuePairSerializerBuilderTypeCreateSerializerKeyConstantLengthMethodDefinition = keyValuePairSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]) && x.GetParameters()[0x1].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x1]));
			_keyValuePairSerializerBuilderTypeCreateSerializerValueConstantLengthMethodDefinition = keyValuePairSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]) && x.GetParameters()[0x1].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x1]));
			_keyValuePairSerializerBuilderTypeCreateSerializerMethodDefinition = keyValuePairSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]) && x.GetParameters()[0x1].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x1]));
			Type iEnumerableSerializerBuilderType = typeof(IEnumerableSerializerBuilder);
			_iEnumerableSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition = iEnumerableSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			_iEnumerableSerializerBuilderTypeCreateSerializerMethodDefinition = iEnumerableSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			Type iCollectionSerializerBuilderType = typeof(ICollectionSerializerBuilder);
			_iCollectionSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition = iCollectionSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			_iCollectionSerializerBuilderTypeCreateSerializerMethodDefinition = iCollectionSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			Type iListSerializerBuilderType = typeof(IListSerializerBuilder);
			_iListSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition = iListSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			_iListSerializerBuilderTypeCreateSerializerMethodDefinition = iListSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			Type iSetSerializerBuilderType = typeof(ISetSerializerBuilder);
			_iSetSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition = iSetSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			_iSetSerializerBuilderTypeCreateSerializerMethodDefinition = iSetSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			Type arraySerializerBuilderType = typeof(ArraySerializerBuilder);
			_arraySerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition = arraySerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			_arraySerializerBuilderTypeCreateSerializerMethodDefinition = arraySerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			Type queueSerializerBuilderType = typeof(QueueSerializerBuilder);
			_queueSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition = queueSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			_queueSerializerBuilderTypeCreateSerializerMethodDefinition = queueSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			Type stackSerializerBuilderType = typeof(StackSerializerBuilder);
			_stackSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition = stackSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			_stackSerializerBuilderTypeCreateSerializerMethodDefinition = stackSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			Type dictionarySerializerBuilderType = typeof(DictionarySerializerBuilder);
			_dictionarySerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition = dictionarySerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(_keyValuePairTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0], x.GetGenericArguments()[0x1])));
			_dictionarySerializerBuilderTypeCreateSerializerMethodDefinition = dictionarySerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(_keyValuePairTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0], x.GetGenericArguments()[0x1])));
			Type listSerializerBuilderType = typeof(ListSerializerBuilder);
			_listSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition = listSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			_listSerializerBuilderTypeCreateSerializerMethodDefinition = listSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			Type hashSetSerializerBuilderType = typeof(HashSetSerializerBuilder);
			_hashSetSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition = hashSetSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iConstantLengthSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
			_hashSetSerializerBuilderTypeCreateSerializerMethodDefinition = hashSetSerializerBuilderType.GetMethods(BindingFlags.Static | BindingFlags.Public).Single(x => x.GetParameters()[0x0].ParameterType == _iSerializerTypeDefinition.MakeGenericType(x.GetGenericArguments()[0x0]));
		}

		static internal ModuleBuilder Module
		{
			get
			{
				lock (_moduleBuilders)
					return _moduleBuilders.TryGetValue(AppDomain.CurrentDomain, out ModuleBuilder moduleBuilder) ? moduleBuilder : AddModule();
			}
		}

		static private ModuleBuilder AddModule()
		{
			AssemblyName assemblyName;
			ModuleBuilder moduleBuilder;
			_moduleBuilders.Add(AppDomain.CurrentDomain, moduleBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName = new AssemblyName("CustomSerializers"), AssemblyBuilderAccess.Run).DefineDynamicModule(assemblyName.Name));
			return moduleBuilder;
		}
		static internal bool IsIConstantLengthSerializer(object serializer)
		{
			foreach (Type serializerInterface in serializer.GetType().GetInterfaces())
			{
				if (!serializerInterface.IsGenericType || serializerInterface.GetGenericTypeDefinition() != _iConstantLengthSerializerTypeDefinition)
					continue;
				return true;
			}
			return false;
		}
	}

	/// <summary>
	/// Represents a builder of an <see cref="ISerializer{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type which instances are to be able to be serialize to a sequence of bytes and deserialize back.</typeparam>
	public class SerializerBuilder<T>
	{
		private struct Field
		{
			internal struct ConstantLengthSerializer
			{
				internal readonly Type _type;
				internal readonly object _serializer;
				internal readonly MethodInfo _countPropertyGetFieldInfo;
				internal readonly MethodInfo _serializeMethodInfo;
				internal readonly MethodInfo _deserializeConstantLengthMethodInfo;

				internal ConstantLengthSerializer(Type fieldType, object serializer)
				{
					_type = SerializerBuilder._constantLengthSerializerTypeDefinition.MakeGenericType(fieldType);
					_serializer = serializer;
					_countPropertyGetFieldInfo = _type.GetProperty(nameof(ConstantLengthSerializer<byte>.Count)).GetGetMethod();
					_serializeMethodInfo = _type.GetMethod(nameof(ConstantLengthSerializer<byte>.Serialize), new Type[] { fieldType, SerializerBuilder._byteArrayType, SerializerBuilder._int32ByRefType });
					_deserializeConstantLengthMethodInfo = _type.GetMethod(nameof(ConstantLengthSerializer<byte>.Deserialize), new Type[] { SerializerBuilder._byteArrayType, SerializerBuilder._int32ByRefType });
				}
			}
			internal struct VariableLengthSerializer
			{
				internal readonly Type _type;
				internal readonly object _serializer;
				internal readonly MethodInfo _countMethodInfo;
				internal readonly MethodInfo _serialzeMethodInfo;
				internal readonly MethodInfo _deserializeMethodInfo;

				internal VariableLengthSerializer(Type fieldType, object serializer)
				{
					_type = SerializerBuilder._serializerTypeDefinition.MakeGenericType(fieldType);
					_serializer = serializer;
					_countMethodInfo = _type.GetMethod(nameof(Serializer<byte>.Count), new Type[] { fieldType });
					_serialzeMethodInfo = _type.GetMethod(nameof(Serializer<byte>.Serialize), new Type[] { fieldType, SerializerBuilder._byteArrayType, SerializerBuilder._int32ByRefType });
					_deserializeMethodInfo = _type.GetMethod(nameof(Serializer<byte>.Deserialize), new Type[] { SerializerBuilder._int32Type, SerializerBuilder._byteArrayType, SerializerBuilder._int32ByRefType });
				}
			}

			internal readonly FieldInfo _info;
			internal readonly int _serializerIndex;

			internal Field(FieldInfo info, int serializerIndex)
			{
				_info = info;
				_serializerIndex = serializerIndex;
			}
		}

		static private readonly Type _type;
		static private readonly Type _genericTypeDefinition;
		static private readonly Type[] _genericArguments;
		static private readonly ConstructorInfo _constructorInfo;
		static private readonly Type[] _validateSerializeMethodParameterTypes;
		static private readonly bool _isPublicType;
		static private readonly bool _isAbstractType;
		static private readonly bool _isValueType;
		static private readonly bool _hasPublicParameterlessConstructor;
		static private readonly bool _hasOnlyPublicFields;
		static private readonly bool _buildDefaultSerializer;
		/// <summary>
		/// The default <see cref="ISerializer{T}"/> of the type.
		/// </summary>
		static public readonly ISerializer<T> Default;

		static SerializerBuilder()
		{
			_type = typeof(T);
			if (_type.IsGenericType)
				_genericTypeDefinition = _type.GetGenericTypeDefinition();
			_genericArguments = _type.GetGenericArguments();
			_constructorInfo = _type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, Type.EmptyTypes, null);
			_validateSerializeMethodParameterTypes = new Type[] { _type, SerializerBuilder._byteArrayType, SerializerBuilder._int32Type };
			_isPublicType = _type.IsPublic || _type.IsNestedPublic;
			_isAbstractType = _type.IsAbstract;
			_isValueType = _type.IsValueType;
			_hasPublicParameterlessConstructor = _isValueType || _constructorInfo != null;
			_hasOnlyPublicFields = _type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Length == 0x0;
			_buildDefaultSerializer = _isPublicType && !_isAbstractType && _hasPublicParameterlessConstructor && _hasOnlyPublicFields;
			if (_type == SerializerBuilder._booleanType)
				Default = (ISerializer<T>)BooleanSerializerBuilder.Default;
			if (_type == SerializerBuilder._byteType)
				Default = (ISerializer<T>)ByteSerializerBuilder.Default;
			else if (_type == SerializerBuilder._sByteType)
				Default = (ISerializer<T>)SByteSerializerBuilder.Default;
			else if (_type == SerializerBuilder._int16Type)
				Default = (ISerializer<T>)Int16SerializerBuilder.Default;
			else if (_type == SerializerBuilder._uInt16Type)
				Default = (ISerializer<T>)UInt16SerializerBuilder.Default;
			else if (_type == SerializerBuilder._charType)
				Default = (ISerializer<T>)CharSerializerBuilder.Default;
			else if (_type == SerializerBuilder._int32Type)
				Default = (ISerializer<T>)Int32SerializerBuilder.Default;
			else if (_type == SerializerBuilder._uInt32Type)
				Default = (ISerializer<T>)UInt32SerializerBuilder.Default;
			else if (_type == SerializerBuilder._int64Type)
				Default = (ISerializer<T>)Int64SerializerBuilder.Default;
			else if (_type == SerializerBuilder._uInt64Type)
				Default = (ISerializer<T>)UInt64SerializerBuilder.Default;
			else if (_type == SerializerBuilder._decimalType)
				Default = (ISerializer<T>)DecimalSerializerBuilder.Default;
			else if (_type == SerializerBuilder._singleType)
				Default = (ISerializer<T>)SingleSerializerBuilder.Default;
			else if (_type == SerializerBuilder._doubleType)
				Default = (ISerializer<T>)DoubleSerializerBuilder.Default;
			else if (_type == SerializerBuilder._dateTimeType)
				Default = (ISerializer<T>)DateTimeSerializerBuilder.Default;
			else if (_type == SerializerBuilder._timeSpanType)
				Default = (ISerializer<T>)TimeSpanSerializerBuilder.Default;
			else if (_type == SerializerBuilder._guidType)
				Default = (ISerializer<T>)GuidSerializerBuilder.Default;
			else if (_type == SerializerBuilder._ipv4EndPoint)
				Default = (ISerializer<T>)IPv4EndPointSerializerBuilder.Default;
			else if (_type == SerializerBuilder._stringType)
				Default = (ISerializer<T>)StringSerializerBuilder.Default;
			else if (_genericTypeDefinition == SerializerBuilder._nullableTypeDefinition)
			{
				object underlyingSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(_genericArguments).GetField(nameof(Default)).GetValue(null);
				if (underlyingSerializer == null)
					return;
				Default = (ISerializer<T>)SerializerBuilder._nullableSerializerBuilderTypeCreateSerializerMethodDefinition.MakeGenericMethod(_genericArguments).Invoke(null, new object[] { underlyingSerializer });
			}
			else if (_genericTypeDefinition == SerializerBuilder._keyValuePairTypeDefinition)
			{
				object keySerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(_genericArguments[0x0]).GetField(nameof(Default)).GetValue(null);
				object valueSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(_genericArguments[0x1]).GetField(nameof(Default)).GetValue(null);
				if (keySerializer == null || valueSerializer == null)
					return;
				Default = (ISerializer<T>)(SerializerBuilder.IsIConstantLengthSerializer(keySerializer) ? SerializerBuilder.IsIConstantLengthSerializer(valueSerializer) ? SerializerBuilder._keyValuePairSerializerBuilderTypeCreateSerializerKeyConstantLengthValueConstantLengthMethodDefinition : SerializerBuilder._keyValuePairSerializerBuilderTypeCreateSerializerKeyConstantLengthMethodDefinition : SerializerBuilder.IsIConstantLengthSerializer(valueSerializer) ? SerializerBuilder._keyValuePairSerializerBuilderTypeCreateSerializerValueConstantLengthMethodDefinition : SerializerBuilder._keyValuePairSerializerBuilderTypeCreateSerializerMethodDefinition).MakeGenericMethod(_genericArguments).Invoke(null, new object[] { keySerializer, valueSerializer });
			}
			else if (_genericTypeDefinition == SerializerBuilder._iEnumerableTypeDefinition)
			{
				object elementSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(_genericArguments).GetField(nameof(Default)).GetValue(null);
				if (elementSerializer == null)
					return;
				Default = (ISerializer<T>)(SerializerBuilder.IsIConstantLengthSerializer(elementSerializer) ? SerializerBuilder._iEnumerableSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition : SerializerBuilder._iEnumerableSerializerBuilderTypeCreateSerializerMethodDefinition).MakeGenericMethod(_genericArguments).Invoke(null, new object[] { elementSerializer });
			}
			else if (_genericTypeDefinition == SerializerBuilder._iCollectionTypeDefinition)
			{
				object elementSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(_genericArguments).GetField(nameof(Default)).GetValue(null);
				if (elementSerializer != null)
					if (elementSerializer == null)
						return;
				Default = (ISerializer<T>)(SerializerBuilder.IsIConstantLengthSerializer(elementSerializer) ? SerializerBuilder._iCollectionSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition : SerializerBuilder._iCollectionSerializerBuilderTypeCreateSerializerMethodDefinition).MakeGenericMethod(_genericArguments).Invoke(null, new object[] { elementSerializer });
			}
			else if (_genericTypeDefinition == SerializerBuilder._iListTypeDefinition)
			{
				object elementSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(_genericArguments).GetField(nameof(Default)).GetValue(null);
				if (elementSerializer != null)
					if (elementSerializer == null)
						return;
				Default = (ISerializer<T>)(SerializerBuilder.IsIConstantLengthSerializer(elementSerializer) ? SerializerBuilder._iListSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition : SerializerBuilder._iListSerializerBuilderTypeCreateSerializerMethodDefinition).MakeGenericMethod(_genericArguments).Invoke(null, new object[] { elementSerializer });
			}
			else if (_genericTypeDefinition == SerializerBuilder._iSetTypeDefinition)
			{
				object elementSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(_genericArguments).GetField(nameof(Default)).GetValue(null);
				if (elementSerializer != null)
					if (elementSerializer == null)
						return;
				Default = (ISerializer<T>)(SerializerBuilder.IsIConstantLengthSerializer(elementSerializer) ? SerializerBuilder._iSetSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition : SerializerBuilder._iSetSerializerBuilderTypeCreateSerializerMethodDefinition).MakeGenericMethod(_genericArguments).Invoke(null, new object[] { elementSerializer });
			}
			else if (_type.IsArray && _type.GetArrayRank() == 0x1)
			{
				Type elementType = _type.GetElementType();
				object elementSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(elementType).GetField(nameof(Default)).GetValue(null);
				if (elementSerializer == null)
					return;
				Default = (ISerializer<T>)(SerializerBuilder.IsIConstantLengthSerializer(elementSerializer) ? SerializerBuilder._arraySerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition : SerializerBuilder._arraySerializerBuilderTypeCreateSerializerMethodDefinition).MakeGenericMethod(elementType).Invoke(null, new object[] { elementSerializer });
			}
			else if (_genericTypeDefinition == SerializerBuilder._queueTypeDefinition)
			{
				object elementSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(_genericArguments).GetField(nameof(Default)).GetValue(null);
				if (elementSerializer == null)
					return;
				Default = (ISerializer<T>)(SerializerBuilder.IsIConstantLengthSerializer(elementSerializer) ? SerializerBuilder._queueSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition : SerializerBuilder._queueSerializerBuilderTypeCreateSerializerMethodDefinition).MakeGenericMethod(_genericArguments).Invoke(null, new object[] { elementSerializer });
			}
			else if (_genericTypeDefinition == SerializerBuilder._stackTypeDefinition)
			{
				object elementSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(_genericArguments).GetField(nameof(Default)).GetValue(null);
				if (elementSerializer == null)
					return;
				Default = (ISerializer<T>)(SerializerBuilder.IsIConstantLengthSerializer(elementSerializer) ? SerializerBuilder._stackSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition : SerializerBuilder._stackSerializerBuilderTypeCreateSerializerMethodDefinition).MakeGenericMethod(_genericArguments).Invoke(null, new object[] { elementSerializer });
			}
			else if (_genericTypeDefinition == SerializerBuilder._dictionaryTypeDefinition)
			{
				object elementSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(SerializerBuilder._keyValuePairTypeDefinition.MakeGenericType(_genericArguments)).GetField(nameof(Default)).GetValue(null);
				if (elementSerializer == null)
					return;
				Default = (ISerializer<T>)(SerializerBuilder.IsIConstantLengthSerializer(elementSerializer) ? SerializerBuilder._dictionarySerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition : SerializerBuilder._dictionarySerializerBuilderTypeCreateSerializerMethodDefinition).MakeGenericMethod(_genericArguments).Invoke(null, new object[] { elementSerializer });
			}
			else if (_genericTypeDefinition == SerializerBuilder._listTypeDefinition)
			{
				object elementSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(_genericArguments).GetField(nameof(Default)).GetValue(null);
				if (elementSerializer == null)
					return;
				Default = (ISerializer<T>)(SerializerBuilder.IsIConstantLengthSerializer(elementSerializer) ? SerializerBuilder._listSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition : SerializerBuilder._listSerializerBuilderTypeCreateSerializerMethodDefinition).MakeGenericMethod(_genericArguments).Invoke(null, new object[] { elementSerializer });
			}
			else if (_genericTypeDefinition == SerializerBuilder._hashSetTypeDefinition)
			{
				object elementSerializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(_genericArguments).GetField(nameof(Default)).GetValue(null);
				if (elementSerializer == null)
					return;
				Default = (ISerializer<T>)(SerializerBuilder.IsIConstantLengthSerializer(elementSerializer) ? SerializerBuilder._hashSetSerializerBuilderTypeCreateSerializerElementConstantLengthMethodDefinition : SerializerBuilder._hashSetSerializerBuilderTypeCreateSerializerMethodDefinition).MakeGenericMethod(_genericArguments).Invoke(null, new object[] { elementSerializer });
			}
			else if (_buildDefaultSerializer)
			{
				SerializerBuilder<T> defaultSerializerBuilder = new SerializerBuilder<T>();
				FieldInfo[] fields = _type.GetFields(BindingFlags.Instance | BindingFlags.Public);
				foreach (FieldInfo fieldInfo in fields)
				{
					object serializer = SerializerBuilder._serializerBuilderTypeDefinition.MakeGenericType(fieldInfo.FieldType).GetField(nameof(Default)).GetValue(null);
					if (serializer == null)
						return;
					defaultSerializerBuilder.AddField(fieldInfo, serializer);
				}
				Default = defaultSerializerBuilder.CreateSerializer();
			}
			else
				return;
			if (_isValueType)
				return;
			Default = (ISerializer<T>)SerializerBuilder._defaultableSerializerBuilderTypeCreateSerializerMethodDefinition.MakeGenericMethod(_type).Invoke(null, new object[] { Default });
		}

		private readonly List<Field.ConstantLengthSerializer> _constantLengthSerializers;
		private readonly List<Field.VariableLengthSerializer> _variableLengthSerializers;
		private readonly Dictionary<object, int> _constantLengthSerializerIndices;
		private readonly Dictionary<object, int> _variableLengthSerializerIndices;
		private readonly HashSet<FieldInfo> _fields;
		private readonly List<Field> _constantLengthFields;
		private readonly List<Field> _variableLengthFields;
		private bool _isConstantLength;
		private int _lastCreateFieldCount;
		private ISerializer<T> _serializer;

		/// <summary>
		/// Initializes the <see cref="SerializerBuilder{T}"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">The type is not public or the type is abstract type or the type does not have a public parameterless constructor.</exception>
		public SerializerBuilder()
		{
			if (!_isPublicType)
				throw new InvalidOperationException(string.Format("The type is not a public type.", nameof(T)));
			if (_isAbstractType)
				throw new InvalidOperationException(string.Format("The type is abstract type.", nameof(T)));
			if (!_hasPublicParameterlessConstructor)
				throw new InvalidOperationException(string.Format("The type does not have a public parameterless constructor.", nameof(T)));
			_constantLengthSerializers = new List<Field.ConstantLengthSerializer>();
			_variableLengthSerializers = new List<Field.VariableLengthSerializer>();
			_constantLengthSerializerIndices = new Dictionary<object, int>();
			_variableLengthSerializerIndices = new Dictionary<object, int>();
			_fields = new HashSet<FieldInfo>();
			_constantLengthFields = new List<Field>();
			_variableLengthFields = new List<Field>();
			_isConstantLength = true;
			_lastCreateFieldCount = -0x1;
		}

		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/>.
		/// </summary>
		/// <returns>An <see cref="ISerializer{T}"/> built by the <see cref="SerializerBuilder{T}"/>.</returns>
		public ISerializer<T> CreateSerializer()
		{
			if (_lastCreateFieldCount == _fields.Count)
				return _serializer;
			Type baseType = (_isConstantLength ? SerializerBuilder._constantLengthSerializerTypeDefinition : SerializerBuilder._serializerTypeDefinition).MakeGenericType(_type);
			ModuleBuilder moduleBuilder = SerializerBuilder.Module;
			TypeBuilder typeBuilder = moduleBuilder.DefineType(new StringBuilder().Append(_type.FullName).Append("Serializer").Append(Guid.NewGuid().ToString()).ToString(), TypeAttributes.Public | TypeAttributes.Sealed, baseType);
			int constantLengthSerializerCount = _constantLengthSerializers.Count;
			int variableLengthSerializerCount = _variableLengthSerializers.Count;
			int serializerCount = constantLengthSerializerCount + variableLengthSerializerCount;
			Type[] serializerTypes = new Type[serializerCount];
			object[] serializers = new object[serializerCount];
			FieldBuilder[] serializerFieldBuilders = new FieldBuilder[serializerCount];
			int serializerIndex = 0x0;
			for (; serializerIndex < constantLengthSerializerCount; serializerIndex++)
			{
				serializerTypes[serializerIndex] = _constantLengthSerializers[serializerIndex]._type;
				serializers[serializerIndex] = _constantLengthSerializers[serializerIndex]._serializer;
				serializerFieldBuilders[serializerIndex] = typeBuilder.DefineField("_serializer" + serializerIndex.ToString(), serializerTypes[serializerIndex], FieldAttributes.Private);
			}
			for (; serializerIndex < serializerCount; serializerIndex++)
			{
				serializerTypes[serializerIndex] = _variableLengthSerializers[serializerIndex - constantLengthSerializerCount]._type;
				serializers[serializerIndex] = _variableLengthSerializers[serializerIndex - constantLengthSerializerCount]._serializer;
				serializerFieldBuilders[serializerIndex] = typeBuilder.DefineField("_serializer" + serializerIndex.ToString(), serializerTypes[serializerIndex], FieldAttributes.Private);
			}
			int constantLengthFieldCount = _constantLengthFields.Count;
			int variableLengthFieldCount = _variableLengthFields.Count;
			int decrementedVariableLengthFieldCount = variableLengthFieldCount - 0x1;
			ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, serializerTypes);
			ILGenerator ilGenerator = constructorBuilder.GetILGenerator();
			ilGenerator.Emit(OpCodes.Ldarg, 0x0);
			Field field;
			if (_isConstantLength)
			{
				if (constantLengthFieldCount == 0x0)
					ilGenerator.Emit(OpCodes.Ldc_I4, 0x0);
				else
					for (int fieldIndex = 0x0; fieldIndex < constantLengthFieldCount; fieldIndex++)
					{
						field = _constantLengthFields[fieldIndex];
						ilGenerator.Emit(OpCodes.Ldarg, 0x1 + field._serializerIndex);
						ilGenerator.Emit(OpCodes.Callvirt, _constantLengthSerializers[field._serializerIndex]._countPropertyGetFieldInfo);
						if (fieldIndex == 0x0)
							continue;
						ilGenerator.Emit(OpCodes.Add_Ovf);
					}
				ilGenerator.Emit(OpCodes.Call, baseType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { SerializerBuilder._int32Type }, null));
			}
			else
				ilGenerator.Emit(OpCodes.Call, SerializerBuilder._objectTypeConstructorInfo);
			for (serializerIndex = 0x0; serializerIndex < serializerCount; serializerIndex++)
			{
				ilGenerator.Emit(OpCodes.Ldarg, 0x0);
				ilGenerator.Emit(OpCodes.Ldarg, 0x1 + serializerIndex);
				ilGenerator.Emit(OpCodes.Stfld, serializerFieldBuilders[serializerIndex]);
			}
			ilGenerator.Emit(OpCodes.Ret);
			Type[] methodParameterTypes;
			MethodInfo methodDeclaration;
			MethodBuilder methodBuilder;
			if (!_isConstantLength)
			{
				methodParameterTypes = new Type[] { _type };
				methodDeclaration = baseType.GetMethod(nameof(Serializer<byte>.Count), methodParameterTypes);
				methodBuilder = typeBuilder.DefineMethod(nameof(Serializer<byte>.Count), MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final, SerializerBuilder._int32Type, methodParameterTypes);
				ilGenerator = methodBuilder.GetILGenerator();
				ilGenerator.Emit(OpCodes.Ldarg, 0x0);
				ilGenerator.Emit(OpCodes.Ldarg, 0x1);
				ilGenerator.Emit(OpCodes.Call, baseType.GetMethod("ValidateCount", BindingFlags.NonPublic | BindingFlags.Instance));
				if (constantLengthFieldCount == 0x0)
					ilGenerator.Emit(OpCodes.Ldc_I4, 0x0);
				else
					for (int fieldIndex = 0x0; fieldIndex < constantLengthFieldCount; fieldIndex++)
					{
						field = _constantLengthFields[fieldIndex];
						ilGenerator.Emit(OpCodes.Ldarg, 0x0);
						ilGenerator.Emit(OpCodes.Ldfld, serializerFieldBuilders[field._serializerIndex]);
						ilGenerator.Emit(OpCodes.Callvirt, _constantLengthSerializers[field._serializerIndex]._countPropertyGetFieldInfo);
						if (fieldIndex == 0x0)
							continue;
						ilGenerator.Emit(OpCodes.Add_Ovf);
					}
				for (int fieldIndex = 0x0; fieldIndex != variableLengthFieldCount; fieldIndex++)
				{
					if (fieldIndex != decrementedVariableLengthFieldCount)
					{
						ilGenerator.Emit(OpCodes.Ldsfld, SerializerBuilder._int32SerializerBuilderTypeInstanceFieldInfo);
						ilGenerator.Emit(OpCodes.Callvirt, SerializerBuilder._constantLengthSerializerInt32TypeCountPropertyGetMethodInfo);
						ilGenerator.Emit(OpCodes.Add_Ovf);
					}
					field = _variableLengthFields[fieldIndex];
					ilGenerator.Emit(OpCodes.Ldarg, 0x0);
					ilGenerator.Emit(OpCodes.Ldfld, serializerFieldBuilders[constantLengthSerializerCount + field._serializerIndex]);
					ilGenerator.Emit(_isValueType ? OpCodes.Ldarga : OpCodes.Ldarg, 0x1);
					ilGenerator.Emit(OpCodes.Ldfld, field._info);
					ilGenerator.Emit(OpCodes.Callvirt, _variableLengthSerializers[field._serializerIndex]._countMethodInfo);
					ilGenerator.Emit(OpCodes.Add_Ovf);
				}
				ilGenerator.Emit(OpCodes.Ret);
				typeBuilder.DefineMethodOverride(methodBuilder, methodDeclaration);
			}
			methodParameterTypes = _isConstantLength ? new Type[] { _type, SerializerBuilder._byteArrayType, SerializerBuilder._int32Type } : new Type[] { _type, SerializerBuilder._byteArrayType, SerializerBuilder._int32ByRefType };
			methodDeclaration = baseType.GetMethod(nameof(ISerializer<byte>.Serialize), methodParameterTypes);
			methodBuilder = typeBuilder.DefineMethod(nameof(ISerializer<byte>.Serialize), MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final, SerializerBuilder._voidType, methodParameterTypes);
			ilGenerator = methodBuilder.GetILGenerator();
			if (_isConstantLength)
			{
				ilGenerator.Emit(OpCodes.Ldarg, 0x0);
				ilGenerator.Emit(OpCodes.Ldarg, 0x1);
				ilGenerator.Emit(OpCodes.Ldarg, 0x2);
				ilGenerator.Emit(OpCodes.Ldarg, 0x3);
				ilGenerator.Emit(OpCodes.Call, baseType.GetMethod("ValidateSerialize", BindingFlags.NonPublic | BindingFlags.Instance, null, _validateSerializeMethodParameterTypes, null));
			}
			else
			{
				_ = ilGenerator.DeclareLocal(SerializerBuilder._int32Type);
				ilGenerator.Emit(OpCodes.Ldarg, 0x0);
				ilGenerator.Emit(OpCodes.Ldarg, 0x1);
				ilGenerator.Emit(OpCodes.Ldarg, 0x2);
				ilGenerator.Emit(OpCodes.Ldarg, 0x3);
				ilGenerator.Emit(OpCodes.Ldobj, SerializerBuilder._int32Type);
				ilGenerator.Emit(OpCodes.Call, baseType.GetMethod("ValidateSerialize", BindingFlags.NonPublic | BindingFlags.Instance, null, _validateSerializeMethodParameterTypes, null));
			}
			for (int fieldIndex = 0x0; fieldIndex < constantLengthFieldCount; fieldIndex++)
			{
				field = _constantLengthFields[fieldIndex];
				ilGenerator.Emit(OpCodes.Ldarg, 0x0);
				ilGenerator.Emit(OpCodes.Ldfld, serializerFieldBuilders[field._serializerIndex]);
				ilGenerator.Emit(_isValueType ? OpCodes.Ldarga : OpCodes.Ldarg, 0x1);
				ilGenerator.Emit(OpCodes.Ldfld, field._info);
				ilGenerator.Emit(OpCodes.Ldarg, 0x2);
				ilGenerator.Emit(_isConstantLength ? OpCodes.Ldarga : OpCodes.Ldarg, 0x3);
				ilGenerator.Emit(OpCodes.Callvirt, _constantLengthSerializers[field._serializerIndex]._serializeMethodInfo);
			}
			for (int fieldIndex = 0x0; fieldIndex != variableLengthFieldCount; fieldIndex++)
			{
				if (fieldIndex != decrementedVariableLengthFieldCount)
				{
					ilGenerator.Emit(OpCodes.Ldarg, 0x3);
					ilGenerator.Emit(OpCodes.Dup);
					ilGenerator.Emit(OpCodes.Dup);
					ilGenerator.Emit(OpCodes.Ldobj, SerializerBuilder._int32Type);
					ilGenerator.Emit(OpCodes.Ldsfld, SerializerBuilder._int32SerializerBuilderTypeInstanceFieldInfo);
					ilGenerator.Emit(OpCodes.Callvirt, SerializerBuilder._constantLengthSerializerInt32TypeCountPropertyGetMethodInfo);
					ilGenerator.Emit(OpCodes.Add);
					ilGenerator.Emit(OpCodes.Stobj, SerializerBuilder._int32Type);
					ilGenerator.Emit(OpCodes.Ldobj, SerializerBuilder._int32Type);
					ilGenerator.Emit(OpCodes.Stloc, 0x0);
				}
				field = _variableLengthFields[fieldIndex];
				ilGenerator.Emit(OpCodes.Ldarg, 0x0);
				ilGenerator.Emit(OpCodes.Ldfld, serializerFieldBuilders[constantLengthSerializerCount + field._serializerIndex]);
				ilGenerator.Emit(_isValueType ? OpCodes.Ldarga : OpCodes.Ldarg, 0x1);
				ilGenerator.Emit(OpCodes.Ldfld, field._info);
				ilGenerator.Emit(OpCodes.Ldarg, 0x2);
				ilGenerator.Emit(OpCodes.Ldarg, 0x3);
				ilGenerator.Emit(OpCodes.Callvirt, _variableLengthSerializers[field._serializerIndex]._serialzeMethodInfo);
				if (fieldIndex != decrementedVariableLengthFieldCount)
				{
					ilGenerator.Emit(OpCodes.Ldsfld, SerializerBuilder._int32SerializerBuilderTypeInstanceFieldInfo);
					ilGenerator.Emit(OpCodes.Ldarg, 0x3);
					ilGenerator.Emit(OpCodes.Ldobj, SerializerBuilder._int32Type);
					ilGenerator.Emit(OpCodes.Ldloc, 0x0);
					ilGenerator.Emit(OpCodes.Sub);
					ilGenerator.Emit(OpCodes.Ldarg, 0x2);
					ilGenerator.Emit(OpCodes.Ldloc, 0x0);
					ilGenerator.Emit(OpCodes.Ldsfld, SerializerBuilder._int32SerializerBuilderTypeInstanceFieldInfo);
					ilGenerator.Emit(OpCodes.Callvirt, SerializerBuilder._constantLengthSerializerInt32TypeCountPropertyGetMethodInfo);
					ilGenerator.Emit(OpCodes.Sub);
					ilGenerator.Emit(OpCodes.Callvirt, SerializerBuilder._constantLengthSerializerInt32TypeSerializeMethodInfo);
				}
			}
			ilGenerator.Emit(OpCodes.Ret);
			typeBuilder.DefineMethodOverride(methodBuilder, methodDeclaration);
			methodParameterTypes = _isConstantLength ? new Type[] { SerializerBuilder._byteArrayType, SerializerBuilder._int32Type } : new Type[] { SerializerBuilder._int32Type, SerializerBuilder._byteArrayType, SerializerBuilder._int32Type };
			methodDeclaration = baseType.GetMethod(nameof(ISerializer<byte>.Deserialize), methodParameterTypes);
			methodBuilder = typeBuilder.DefineMethod(nameof(ISerializer<byte>.Deserialize), MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final, _type, methodParameterTypes);
			ilGenerator = methodBuilder.GetILGenerator();
			_ = ilGenerator.DeclareLocal(_type);
			if (_isConstantLength)
			{
				ilGenerator.Emit(OpCodes.Ldarg, 0x0);
				ilGenerator.Emit(OpCodes.Ldarg, 0x1);
				ilGenerator.Emit(OpCodes.Ldarg, 0x2);
				ilGenerator.Emit(OpCodes.Call, baseType.GetMethod("ValidateDeserialize", BindingFlags.NonPublic | BindingFlags.Instance));
			}
			else
			{
				_ = ilGenerator.DeclareLocal(SerializerBuilder._int32Type);
				ilGenerator.Emit(OpCodes.Ldarg, 0x0);
				ilGenerator.Emit(OpCodes.Ldarg, 0x1);
				ilGenerator.Emit(OpCodes.Ldarg, 0x2);
				ilGenerator.Emit(OpCodes.Ldarg, 0x3);
				ilGenerator.Emit(OpCodes.Call, baseType.GetMethod("ValidateDeserialize", BindingFlags.NonPublic | BindingFlags.Instance));
				ilGenerator.Emit(OpCodes.Ldarg, 0x3);
				ilGenerator.Emit(OpCodes.Stloc, 0x1);
			}
			if (_isValueType)
			{
				ilGenerator.Emit(OpCodes.Ldloca, 0x0);
				ilGenerator.Emit(OpCodes.Initobj, _type);
			}
			else
			{
				ilGenerator.Emit(OpCodes.Newobj, _constructorInfo);
				ilGenerator.Emit(OpCodes.Stloc, 0x0);
			}
			for (int fieldIndex = 0x0; fieldIndex < constantLengthFieldCount; fieldIndex++)
			{
				field = _constantLengthFields[fieldIndex];
				ilGenerator.Emit(_isValueType ? OpCodes.Ldloca : OpCodes.Ldloc, 0x0);
				ilGenerator.Emit(OpCodes.Ldarg, 0x0);
				ilGenerator.Emit(OpCodes.Ldfld, serializerFieldBuilders[field._serializerIndex]);
				ilGenerator.Emit(OpCodes.Ldarg, _isConstantLength ? 0x1 : 0x2);
				ilGenerator.Emit(OpCodes.Ldarga, _isConstantLength ? 0x2 : 0x3);
				ilGenerator.Emit(OpCodes.Callvirt, _constantLengthSerializers[field._serializerIndex]._deserializeConstantLengthMethodInfo);
				ilGenerator.Emit(OpCodes.Stfld, field._info);
			}
			for (int fieldIndex = 0x0; fieldIndex != variableLengthFieldCount; fieldIndex++)
			{
				field = _variableLengthFields[fieldIndex];
				ilGenerator.Emit(_isValueType ? OpCodes.Ldloca : OpCodes.Ldloc, 0x0);
				ilGenerator.Emit(OpCodes.Ldarg, 0x0);
				ilGenerator.Emit(OpCodes.Ldfld, serializerFieldBuilders[constantLengthSerializerCount + field._serializerIndex]);
				if (fieldIndex != decrementedVariableLengthFieldCount)
				{
					ilGenerator.Emit(OpCodes.Ldsfld, SerializerBuilder._int32SerializerBuilderTypeInstanceFieldInfo);
					ilGenerator.Emit(OpCodes.Ldarg, 0x2);
					ilGenerator.Emit(OpCodes.Ldarga, 0x3);
					ilGenerator.Emit(OpCodes.Callvirt, SerializerBuilder._constantLengthSerializerInt32TypeDeserializeMethodInfo);
				}
				else
				{
					ilGenerator.Emit(OpCodes.Ldarg, 0x1);
					ilGenerator.Emit(OpCodes.Ldarg, 0x3);
					ilGenerator.Emit(OpCodes.Sub);
					ilGenerator.Emit(OpCodes.Ldloc, 0x1);
					ilGenerator.Emit(OpCodes.Add);
				}
				ilGenerator.Emit(OpCodes.Ldarg, 0x2);
				ilGenerator.Emit(OpCodes.Ldarga, 0x3);
				ilGenerator.Emit(OpCodes.Callvirt, _variableLengthSerializers[field._serializerIndex]._deserializeMethodInfo);
				ilGenerator.Emit(OpCodes.Stfld, field._info);
			}
			ilGenerator.Emit(OpCodes.Ldloc, 0x0);
			ilGenerator.Emit(OpCodes.Ret);
			typeBuilder.DefineMethodOverride(methodBuilder, methodDeclaration);
			_lastCreateFieldCount = _fields.Count;
			return _serializer = (ISerializer<T>)Activator.CreateInstance(typeBuilder.CreateTypeInfo(), serializers);
		}
		/// <summary>
		/// Adds a field of the type to code.
		/// </summary>
		/// <param name="fieldInfo">The <see cref="FieldInfo"/> of the field.</param>
		/// <param name="serializer">An <see cref="ISerializer{T}"/> to code the field.</param>
		/// <exception cref="ArgumentNullException"><paramref name="fieldInfo"/> is <see langword="null"/> or <paramref name="serializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The field is static or the field is not a public field of the type or the field is already added to the <see cref="SerializerBuilder{T}"/> or <paramref name="serializer"/> type does not implement the <see cref="ISerializer{T}"/> interface or the type of the field and <paramref name="serializer"/> type are not compatible.</exception>
		public void AddField(FieldInfo fieldInfo, object serializer)
		{
			if (fieldInfo == null)
				throw new ArgumentNullException(nameof(fieldInfo));
			if (serializer == null)
				throw new ArgumentNullException(nameof(serializer));
			Type declaringType = fieldInfo.DeclaringType;
			for (Type type = _type; type != null; type = type.BaseType)
			{
				if (declaringType != type)
					continue;
				declaringType = null;
				break;
			}
			if (declaringType != null)
				throw new ArgumentException("The field is not a field of the type.");
			if (fieldInfo.IsStatic)
				throw new ArgumentException("The field is static.");
			if (!fieldInfo.IsPublic)
				throw new ArgumentException("The field is not public.");
			bool implements = false;
			foreach (Type serializerInterface in serializer.GetType().GetInterfaces())
			{
				if (!serializerInterface.IsGenericType || serializerInterface.GetGenericTypeDefinition() != SerializerBuilder._iSerializerTypeDefinition)
					continue;
				implements = true;
				if (fieldInfo.FieldType != serializerInterface.GetGenericArguments()[0x0])
					throw new ArgumentException(string.Format("The type of the field and {0} type are not compatible.", nameof(serializer)));
				break;
			}
			if (!implements)
				throw new ArgumentException(string.Format("{0} does not implement the {1} interface.", nameof(serializer), SerializerBuilder._iSerializerTypeDefinition.FullName));
			if (!_fields.Add(fieldInfo))
				throw new ArgumentException(string.Format("The field is already added to the {0}.", nameof(SerializerBuilder<T>)));
			int serializerIndex;
			if (SerializerBuilder.IsIConstantLengthSerializer(serializer))
			{
				if (!_constantLengthSerializerIndices.TryGetValue(serializer, out serializerIndex))
				{
					serializerIndex = _constantLengthSerializers.Count;
					_constantLengthSerializers.Add(new Field.ConstantLengthSerializer(fieldInfo.FieldType, serializer));
					_constantLengthSerializerIndices.Add(serializer, serializerIndex);
				}
				_constantLengthFields.Add(new Field(fieldInfo, serializerIndex));
				return;
			}
			if (!_variableLengthSerializerIndices.TryGetValue(serializer, out serializerIndex))
			{
				serializerIndex = _variableLengthSerializers.Count;
				_variableLengthSerializers.Add(new Field.VariableLengthSerializer(fieldInfo.FieldType, serializer));
				_variableLengthSerializerIndices.Add(serializer, serializerIndex);
				_isConstantLength = false;
			}
			_variableLengthFields.Add(new Field(fieldInfo, serializerIndex));
		}
		/// <summary>
		/// Adds a field of the type to code.
		/// </summary>
		/// <typeparam name="TField">The type of the field.</typeparam>
		/// <param name="fieldExpression">The field expression to define the field. Example: t => t.Field.</param>
		/// <param name="serializer">An <see cref="ISerializer{T}"/> to code the field.</param>
		/// <exception cref="ArgumentNullException"><paramref name="fieldExpression"/> is <see langword="null"/> or <paramref name="serializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="fieldExpression"/> does not access to a non-static public field of the type or the field is already added to the <see cref="SerializerBuilder{T}"/>.</exception>
		public void AddField<TField>(Expression<Func<T, TField>> fieldExpression, ISerializer<TField> serializer)
		{
			if (fieldExpression == null)
				throw new ArgumentNullException(nameof(fieldExpression));
			if (!(fieldExpression.Body is MemberExpression memberExpression) || !(memberExpression.Member is FieldInfo fieldInfo))
				throw new ArgumentException(string.Format("{0} does not access to a field.", nameof(fieldExpression)));
			AddField(fieldInfo, serializer);
		}
	}
}