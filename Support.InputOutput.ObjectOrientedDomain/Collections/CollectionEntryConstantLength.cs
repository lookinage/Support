using System.Collections.Generic;
using Noname.BitConversion;
using Noname.BitConversion.System;

namespace Noname.IO.ObjectOrientedDomain.Collections
{
	internal struct CollectionEntryConstantLength<TDataConstantLength> where TDataConstantLength : struct
	{
		internal sealed class BitConverter : ConstantLengthBitConverter<CollectionEntryConstantLength<TDataConstantLength>>
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

			private BitConverter(ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter) : base(BooleanBitConverterBuilder.Instance.ByteCount + (dataConstantLengthBitConverter.ByteCount > Int32BitConverterBuilder.Instance.ByteCount ? dataConstantLengthBitConverter.ByteCount : Int32BitConverterBuilder.Instance.ByteCount)) => _dataConstantLengthBitConverter = dataConstantLengthBitConverter;

			public override sealed void GetBytes(CollectionEntryConstantLength<TDataConstantLength> instance, byte[] bytes, int index)
			{
				BooleanBitConverterBuilder.Instance.GetBytes(instance.HasItem, bytes, ref index);
				if (instance.HasItem)
					_dataConstantLengthBitConverter.GetBytes(instance._dataConstantLength, bytes, ref index);
				else
					Int32BitConverterBuilder.Instance.GetBytes(instance._nextFreeIndex, bytes, ref index);
			}
			public override sealed CollectionEntryConstantLength<TDataConstantLength> GetInstance(byte[] bytes, int index) => BooleanBitConverterBuilder.Instance.GetInstance(bytes, ref index) ? new CollectionEntryConstantLength<TDataConstantLength>(_dataConstantLengthBitConverter.GetInstance(bytes, ref index)) : new CollectionEntryConstantLength<TDataConstantLength>(Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index));
		}

		internal int _nextFreeIndex;
		internal TDataConstantLength _dataConstantLength;

		private CollectionEntryConstantLength(int nextFreeIndex)
		{
			_nextFreeIndex = nextFreeIndex;
			_dataConstantLength = default;
		}
		private CollectionEntryConstantLength(TDataConstantLength dataConstantLength)
		{
			_nextFreeIndex = -2;
			_dataConstantLength = dataConstantLength;
		}

		internal bool HasItem => _nextFreeIndex == -2;

		internal void SetHasItem() => _nextFreeIndex = -2;
	}
}