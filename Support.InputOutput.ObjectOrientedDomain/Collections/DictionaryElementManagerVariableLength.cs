//using System;

//namespace Noname.IO.ObjectOrientedDomain.Collections
//{
//	internal sealed class DictionaryElementManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength> : DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> where TDataConstantLength : struct where TDataVariableLength : struct
//	{
//		private readonly DictionaryManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength> _dictionaryManager;
//		private ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> _entryManager;

//		internal DictionaryElementManagerVariableLength(int index, DictionaryManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength> dictionaryManager, ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> entryManager) : base(index, dictionaryManager)
//		{
//			_dictionaryManager = dictionaryManager;
//			_entryManager = entryManager;
//		}

//		public override sealed TDataVariableLength DataVariableLength
//		{
//			get
//			{
//				Check();
//				return _entryManager.DataVariableLength._dataVariableLength;
//			}
//			set
//			{
//				Check();
//				_dictionaryManager.IncreaseVersion();
//				DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength> entry = _entryManager.DataVariableLength;
//				entry._dataVariableLength = value;
//				_entryManager.DataVariableLength = entry;
//			}
//		}
//		public override sealed TDataConstantLength DataConstantLength
//		{
//			get
//			{
//				Check();
//				return _entryManager.DataConstantLength._dataConstantLength;
//			}
//			set
//			{
//				Check();
//				_dictionaryManager.IncreaseVersion();
//				DictionaryEntryVariableLengthConstantLength<TDataConstantLength> entry = _entryManager.DataConstantLength;
//				entry._dataConstantLength = value;
//				_entryManager.DataConstantLength = entry;
//			}
//		}
//		public override sealed PointerManager this[int pointerIndex]
//		{
//			get
//			{
//				Check();
//				return _entryManager[pointerIndex];
//			}
//			set
//			{
//				Check();
//				_dictionaryManager.IncreaseVersion();
//				_entryManager[pointerIndex] = value;
//			}
//		}
//		public override sealed TKey Key => _entryManager.DataVariableLength._key;
//		public override sealed bool HasItem => _entryManager.DataConstantLength._hasItem;

//		private void Check()
//		{
//			if (!_entryManager.DataConstantLength._hasItem)
//				throw new InvalidOperationException("The element does not contain an item.");
//			if (_dictionaryManager.CheckEntryShard(_index, _entryManager._arrayManager))
//				return;
//			_entryManager = _dictionaryManager.GetEntryManager(_index);
//		}
//	}
//}