using System.Collections.Generic;
using Noname.BitConversion;
using Noname.BitConversion.System;

namespace Noname.IO.ObjectOrientedDomain.Collections
{
	internal struct DictionaryEntryVariableLengthConstantLength<TDataConstantLength> where TDataConstantLength : struct
	{
		internal sealed class BitConverter : ConstantLengthBitConverter<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>>
		{
			static private readonly Dictionary<ConstantLengthBitConverter<TDataConstantLength>, BitConverter> _instances;

			static BitConverter() => _instances = new Dictionary<ConstantLengthBitConverter<TDataConstantLength>, BitConverter>();

			static internal BitConverter GetInstance(ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter)
			{
				if (_instances.TryGetValue(dataConstantLengthBitConverter, out BitConverter instance))
					return instance;
				_instances.Add(dataConstantLengthBitConverter, instance = new BitConverter(dataConstantLengthBitConverter));
				return instance;
			}

			internal readonly ConstantLengthBitConverter<TDataConstantLength> _dataConstantLengthBitConverter;

			private BitConverter(ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter) : base(BooleanBitConverterBuilder.Instance.ByteCount + Int32BitConverterBuilder.Instance.ByteCount + dataConstantLengthBitConverter.ByteCount) => _dataConstantLengthBitConverter = dataConstantLengthBitConverter;

			public override sealed void GetBytes(DictionaryEntryVariableLengthConstantLength<TDataConstantLength> instance, byte[] bytes, int index)
			{
				BooleanBitConverterBuilder.Instance.GetBytes(instance._hasItem, bytes, ref index);
				Int32BitConverterBuilder.Instance.GetBytes(instance._next, bytes, ref index);
				_dataConstantLengthBitConverter.GetBytes(instance._dataConstantLength, bytes, ref index);
			}
			public override sealed DictionaryEntryVariableLengthConstantLength<TDataConstantLength> GetInstance(byte[] bytes, int index) => new DictionaryEntryVariableLengthConstantLength<TDataConstantLength>(BooleanBitConverterBuilder.Instance.GetInstance(bytes, ref index), Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index), _dataConstantLengthBitConverter.GetInstance(bytes, ref index));
		}

		internal bool _hasItem;
		internal int _next;
		internal TDataConstantLength _dataConstantLength;

		private DictionaryEntryVariableLengthConstantLength(bool hasItem, int next, TDataConstantLength dataConstantLength)
		{
			_hasItem = hasItem;
			_next = next;
			_dataConstantLength = dataConstantLength;
		}
	}
}