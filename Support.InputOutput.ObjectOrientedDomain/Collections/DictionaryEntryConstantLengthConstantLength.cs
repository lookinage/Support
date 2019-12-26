using System.Collections.Generic;
using Noname.BitConversion;
using Noname.BitConversion.System;

namespace Noname.IO.ObjectOrientedDomain.Collections
{
	internal struct DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength> where TKey : struct where TDataConstantLength : struct
	{
		internal sealed class BitConverter : ConstantLengthBitConverter<DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength>>
		{
			internal struct Info
			{
				internal readonly ConstantLengthBitConverter<TKey> _keyBitConverter;
				internal readonly ConstantLengthBitConverter<TDataConstantLength> _dataConstantLengthBitConverter;

				public Info(ConstantLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter)
				{
					_keyBitConverter = keyBitConverter;
					_dataConstantLengthBitConverter = dataConstantLengthBitConverter;
				}
			}

			static private readonly Dictionary<Info, BitConverter> _instances;

			static BitConverter() => _instances = new Dictionary<Info, BitConverter>();

			static internal BitConverter GetInstance(ConstantLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter)
			{
				Info info = new Info(keyBitConverter, dataConstantLengthBitConverter);
				if (_instances.TryGetValue(info, out BitConverter instance))
					return instance;
				_instances.Add(info, instance = new BitConverter(info));
				return instance;
			}

			private readonly Info _info;

			private BitConverter(Info info) : base(BooleanBitConverterBuilder.Instance.ByteCount + Int32BitConverterBuilder.Instance.ByteCount + info._keyBitConverter.ByteCount + info._dataConstantLengthBitConverter.ByteCount) => _info = info;

			public override sealed void GetBytes(DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength> instance, byte[] bytes, int index)
			{
				BooleanBitConverterBuilder.Instance.GetBytes(instance._hasItem, bytes, ref index);
				Int32BitConverterBuilder.Instance.GetBytes(instance._next, bytes, ref index);
				_info._keyBitConverter.GetBytes(instance._key, bytes, ref index);
				_info._dataConstantLengthBitConverter.GetBytes(instance._dataConstantLength, bytes, ref index);
			}
			public override sealed DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength> GetInstance(byte[] bytes, int index) => new DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength>(BooleanBitConverterBuilder.Instance.GetInstance(bytes, ref index), Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index), _info._keyBitConverter.GetInstance(bytes, ref index), _info._dataConstantLengthBitConverter.GetInstance(bytes, ref index));
		}

		internal bool _hasItem;
		internal int _next;
		internal TKey _key;
		internal TDataConstantLength _dataConstantLength;

		private DictionaryEntryConstantLengthConstantLength(bool hasItem, int next, TKey key, TDataConstantLength dataConstantLength)
		{
			_hasItem = hasItem;
			_next = next;
			_key = key;
			_dataConstantLength = dataConstantLength;
		}
	}
}