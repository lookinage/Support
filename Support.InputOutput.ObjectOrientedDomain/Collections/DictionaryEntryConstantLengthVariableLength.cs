//using System.Collections.Generic;
//using Noname.BitConversion;

//namespace Noname.IO.ObjectOrientedDomain.Collections
//{
//	internal struct DictionaryEntryConstantLengthVariableLength<TDataVariableLength>
//	{
//		internal sealed class BitConverter : VariableLengthBitConverter<DictionaryEntryConstantLengthVariableLength<TDataVariableLength>>
//		{
//			static private readonly Dictionary<VariableLengthBitConverter<TDataVariableLength>, BitConverter> _instances;

//			static BitConverter() => _instances = new Dictionary<VariableLengthBitConverter<TDataVariableLength>, BitConverter>();

//			static internal BitConverter GetInstance(VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter)
//			{
//				if (_instances.TryGetValue(dataVariableLengthBitConverter, out BitConverter instance))
//					return instance;
//				_instances.Add(dataVariableLengthBitConverter, instance = new BitConverter(dataVariableLengthBitConverter));
//				return instance;
//			}

//			private readonly VariableLengthBitConverter<TDataVariableLength> _dataVariableLengthBitConverter;

//			private BitConverter(VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter) => _dataVariableLengthBitConverter = dataVariableLengthBitConverter;

//			public override sealed int GetByteCount(DictionaryEntryConstantLengthVariableLength<TDataVariableLength> instance) => _dataVariableLengthBitConverter.GetByteCount(instance._dataVariableLength);
//			public override sealed void GetBytes(DictionaryEntryConstantLengthVariableLength<TDataVariableLength> instance, byte[] bytes, ref int index) => _dataVariableLengthBitConverter.GetBytes(instance._dataVariableLength, bytes, ref index);
//			public override sealed DictionaryEntryConstantLengthVariableLength<TDataVariableLength> GetInstance(byte[] bytes, int index, int count) => new DictionaryEntryConstantLengthVariableLength<TDataVariableLength>(_dataVariableLengthBitConverter.GetInstance(bytes, ref index, count));
//		}

//		internal TDataVariableLength _dataVariableLength;

//		private DictionaryEntryConstantLengthVariableLength(TDataVariableLength dataConstantLength) => _dataVariableLength = dataConstantLength;
//	}
//}