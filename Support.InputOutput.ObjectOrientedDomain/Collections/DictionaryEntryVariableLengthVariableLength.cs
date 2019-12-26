//using System.Collections.Generic;
//using Noname.BitConversion;

//namespace Noname.IO.ObjectOrientedDomain.Collections
//{
//	internal struct DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>
//	{
//		internal sealed class BitConverter : VariableLengthBitConverter<DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>>
//		{
//			private struct Info
//			{
//				internal readonly VariableLengthBitConverter<TKey> _keyBitConverter;
//				internal readonly VariableLengthBitConverter<TDataVariableLength> _dataVariableLengthBitConverter;

//				public Info(VariableLengthBitConverter<TKey> keyBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter)
//				{
//					_keyBitConverter = keyBitConverter;
//					_dataVariableLengthBitConverter = dataVariableLengthBitConverter;
//				}
//			}

//			static private readonly Dictionary<Info, BitConverter> _instances;

//			static BitConverter() => _instances = new Dictionary<Info, BitConverter>();

//			static internal BitConverter GetInstance(VariableLengthBitConverter<TKey> keyBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter)
//			{
//				Info info = new Info(keyBitConverter, DefaultableBitConverterBuilder.GetInstance(dataVariableLengthBitConverter));
//				if (_instances.TryGetValue(info, out BitConverter instance))
//					return instance;
//				_instances.Add(info, instance = new BitConverter(info));
//				return instance;
//			}

//			private readonly Info _info;

//			private BitConverter(Info info) => _info = info;

//			public override sealed int GetByteCount(DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength> instance) => checked(_info._keyBitConverter.GetByteCount(instance._key) + _info._dataVariableLengthBitConverter.GetByteCount(instance._dataVariableLength));
//			public override sealed void GetBytes(DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength> instance, byte[] bytes, ref int index)
//			{
//				_info._keyBitConverter.GetBytes(instance._key, bytes, ref index);
//				_info._dataVariableLengthBitConverter.GetBytes(instance._dataVariableLength, bytes, ref index);
//			}
//			public override sealed DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength> GetInstance(byte[] bytes, int index, int count)
//			{
//				int startIndex = index;
//				return new DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>(_info._keyBitConverter.GetInstance(bytes, ref index), _info._dataVariableLengthBitConverter.GetInstance(bytes, ref index, count - index + startIndex));
//			}
//		}

//		internal TKey _key;
//		internal TDataVariableLength _dataVariableLength;

//		private DictionaryEntryVariableLengthVariableLength(TKey key, TDataVariableLength dataConstantLength)
//		{
//			_key = key;
//			_dataVariableLength = dataConstantLength;
//		}
//	}
//}