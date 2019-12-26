//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using Noname.BitConversion;
//using Noname.BitConversion.System;

//namespace Noname.IO.ObjectOrientedDomain.Collections
//{
//	internal sealed class DictionaryManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength> : DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> where TDataConstantLength : struct where TDataVariableLength : struct
//	{
//		private struct Enumerator : IEnumerator<DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength>>
//		{
//			private readonly DictionaryManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength> _dictionaryManager;
//			private readonly int _version;
//			private int _current;
//			private DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> _currentItem;

//			internal Enumerator(DictionaryManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength> dictionaryManager)
//			{
//				_dictionaryManager = dictionaryManager;
//				_version = dictionaryManager._version;
//				_current = -1;
//				_currentItem = null;
//			}

//			object IEnumerator.Current => _currentItem;

//			DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> IEnumerator<DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength>>.Current => _currentItem;

//			void IDisposable.Dispose() { }

//			bool IEnumerator.MoveNext()
//			{
//				if (_version != _dictionaryManager._version)
//					throw new InvalidOperationException();
//				for (; ; )
//				{
//					if (++_current >= _dictionaryManager._metaManager.DataConstantLength._usedCount)
//						return false;
//					ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> entryManager = _dictionaryManager.GetEntryManager(_current);
//					if (!entryManager.DataConstantLength._hasItem)
//						continue;
//					_currentItem = new DictionaryElementManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength>(_current, _dictionaryManager, entryManager);
//					return true;
//				}
//			}
//			void IEnumerator.Reset()
//			{
//				_current = -1;
//				_currentItem = null;
//			}
//		}

//		private readonly DictionaryEntryVariableLengthConstantLength<TDataConstantLength>.BitConverter _entryConstantLengthBitConverter;
//		private readonly DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>.BitConverter _entryVariableLengthBitConverter;
//		private readonly int _pointerCount;
//		private readonly IEqualityComparer<TKey> _comparer;
//		private ArrayManager<int, EmptyData>[] _bucketShards;
//		private ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>>[] _entryShards;
//		private int _version;

//		internal DictionaryManagerVariableLength(ValueManager<DictionaryMeta, EmptyData> metaManager, DictionaryEntryVariableLengthConstantLength<TDataConstantLength>.BitConverter entryConstantLengthBitConverter, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>.BitConverter entryVariableLengthBitConverter, int pointerCount, IEqualityComparer<TKey> comparer, ArrayManager<int, EmptyData>[] bucketShards, ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>>[] entryShards) : base(metaManager)
//		{
//			_entryConstantLengthBitConverter = entryConstantLengthBitConverter;
//			_entryVariableLengthBitConverter = entryVariableLengthBitConverter;
//			_pointerCount = pointerCount;
//			_comparer = comparer;
//			_bucketShards = bucketShards;
//			_entryShards = entryShards;
//			_version = 0;
//		}

//		public override sealed int PointerCount => _pointerCount;
//		public override sealed DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> this[TKey key]
//		{
//			get
//			{
//				ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> entryManager = FindEntry(key, out int index);
//				if (entryManager == null)
//					throw new ArgumentException("The dictionary does not contain an item with the specified key.");
//				return new DictionaryElementManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength>(index, this, entryManager);
//			}
//		}

//		private unsafe void IncreaseShardCapacity(int shardIndex)
//		{
//			DomainManager domainManager = _metaManager.Pointer.Domain;
//			ArrayManager<int, EmptyData> bucketShard = _bucketShards[shardIndex];
//			ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> entryShard = _entryShards[shardIndex];
//			int shardCapacity = bucketShard.Length << 1;
//			if (shardCapacity > 0x10000)
//				shardCapacity = 0x10000;
//			ArrayManager<int, EmptyData> newBucketShard = ArrayManagerActivator.Create(domainManager, Int32BitConverterBuilder.Instance, EmptyData.VariableLengthBitConverterInstance, 0, shardCapacity);
//			for (int i = 0; i < shardCapacity; i++)
//				newBucketShard[i].DataConstantLength = -1;
//			ArrayManagerActivator.Open(_metaManager[0], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance)[shardIndex][0] = newBucketShard.Pointer;
//			_bucketShards[shardIndex] = newBucketShard;
//			ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> newEntryShard = ArrayManagerActivator.Create(domainManager, _entryConstantLengthBitConverter, _entryVariableLengthBitConverter, _pointerCount, shardCapacity);
//			ArrayManagerActivator.Copy(entryShard, newEntryShard, entryShard.Length);
//			int usedShardCount = entryShard.Length;
//			for (int entryIndex = 0; entryIndex < usedShardCount; entryIndex++)
//			{
//				ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> entryManager = newEntryShard[entryIndex];
//				DictionaryEntryVariableLengthConstantLength<TDataConstantLength> entry = entryManager.DataConstantLength;
//				if (!entry._hasItem)
//					continue;
//				ArrayElementValueManager<int, EmptyData> bucketManager = newBucketShard[(_comparer.GetHashCode(entryManager.DataVariableLength._key) & 0x7FFFFFFF) % shardCapacity];
//				entry._next = bucketManager.DataConstantLength;
//				entryManager.DataConstantLength = entry;
//				bucketManager.DataConstantLength = shardIndex << 0x10 | entryIndex;
//			}
//			ArrayManagerActivator.Open(_metaManager[1], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance)[shardIndex][0] = newEntryShard.Pointer;
//			_entryShards[shardIndex] = newEntryShard;
//		}
//		private unsafe void IncreaseShardCount(int shardIndex)
//		{
//			DomainManager domainManager = _metaManager.Pointer.Domain;
//			ArrayManager<EmptyData, EmptyData> bucketShardManager = ArrayManagerActivator.Open(_metaManager[0], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance);
//			ArrayManager<EmptyData, EmptyData> entryShardManager = ArrayManagerActivator.Open(_metaManager[1], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance);
//			short shardCount = (short)bucketShardManager.Length;
//			shardCount = shardCount == 0 ? (short)1 : (short)(shardCount << 1);
//			if (shardCount < 0)
//				shardCount = short.MaxValue;
//			ArrayManager<EmptyData, EmptyData> newBucketShardManager = ArrayManagerActivator.Create(domainManager, EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance, 1, shardCount);
//			ArrayManagerActivator.Copy(bucketShardManager, newBucketShardManager, bucketShardManager.Length);
//			ArrayManager<int, EmptyData>[] newBucketShards = new ArrayManager<int, EmptyData>[shardCount];
//			Array.Copy(_bucketShards, newBucketShards, _bucketShards.Length);
//			ArrayManager<EmptyData, EmptyData> newEntryShardManager = ArrayManagerActivator.Create(domainManager, EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance, 1, shardCount);
//			ArrayManagerActivator.Copy(entryShardManager, newEntryShardManager, entryShardManager.Length);
//			ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>>[] newEntryShards = new ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>>[shardCount];
//			Array.Copy(_entryShards, newEntryShards, _entryShards.Length);
//			_metaManager[0] = newBucketShardManager.Pointer;
//			_metaManager[1] = newEntryShardManager.Pointer;
//			_bucketShards = newBucketShards;
//			_entryShards = newEntryShards;
//		}
//		private unsafe void EnsureCapacity(int index)
//		{
//			int shardIndex = *((short*)&index + 1);
//			if (shardIndex == _entryShards.Length)
//				IncreaseShardCount(shardIndex);
//			ArrayManager<int, EmptyData> bucketShard = _bucketShards[shardIndex];
//			if (bucketShard == null)
//			{
//				DomainManager domainManager = _metaManager.Pointer.Domain;
//				bucketShard = ArrayManagerActivator.Create(domainManager, Int32BitConverterBuilder.Instance, EmptyData.VariableLengthBitConverterInstance, 0, CollectionManagerActivator._defaultShardCapacity);
//				for (int i = 0; i < CollectionManagerActivator._defaultShardCapacity; i++)
//					bucketShard[i].DataConstantLength = -1;
//				ArrayManagerActivator.Open(_metaManager[0], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance)[shardIndex][0] = bucketShard.Pointer;
//				_bucketShards[shardIndex] = bucketShard;
//				_entryShards[shardIndex] = ArrayManagerActivator.Create(domainManager, _entryConstantLengthBitConverter, _entryVariableLengthBitConverter, _pointerCount, CollectionManagerActivator._defaultShardCapacity);
//				ArrayManagerActivator.Open(_metaManager[1], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance)[shardIndex][0] = _entryShards[shardIndex].Pointer;
//			}
//			if (*(ushort*)&index < bucketShard.Length)
//				return;
//			IncreaseShardCapacity(shardIndex);
//		}
//		private ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> FindEntry(TKey key, out int index)
//		{
//			int hashCode = _comparer.GetHashCode(key) & 0x7FFFFFFF;
//			foreach (ArrayManager<int, EmptyData> bucketShard in _bucketShards)
//			{
//				if (bucketShard == null)
//					break;
//				ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> entryManager;
//				for (index = bucketShard[hashCode % bucketShard.Length].DataConstantLength; index != -1; index = entryManager.DataConstantLength._next)
//				{
//					entryManager = GetEntryManager(index);
//					if (!_comparer.Equals(key, entryManager.DataVariableLength._key))
//						continue;
//					return entryManager;
//				}
//			}
//			index = 0;
//			return null;
//		}
//		internal unsafe bool CheckEntryShard(int index, ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> entryShard) => entryShard == _entryShards[*((short*)&index + 1)];
//		internal unsafe ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> GetEntryManager(int index) => _entryShards[*((short*)&index + 1)][*(ushort*)&index];
//		internal override sealed void IncreaseVersion() => _version++;
//		public override sealed IEnumerator<DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength>> GetEnumerator() => new Enumerator(this);
//		public override sealed bool ContainsKey(TKey key)
//		{
//			if (key == null)
//				throw new ArgumentNullException(nameof(key));
//			return FindEntry(key, out int index) != null;
//		}
//		public override sealed void CopyTo(DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength>[] array, int arrayIndex)
//		{
//			if (array == null)
//				throw new ArgumentNullException(nameof(array));
//			if (arrayIndex < 0 || arrayIndex > array.Length)
//				throw new ArgumentOutOfRangeException(nameof(arrayIndex));
//			if (Count > array.Length - arrayIndex)
//				throw new ArgumentException(string.Format("The number of elements in the dictionary is greater than the available space from the {0} to the end of the destination array.", nameof(arrayIndex)));
//			int usedCount = _metaManager.DataConstantLength._usedCount;
//			if (usedCount == 0)
//				return;
//			int shardCount = _entryShards.Length;
//			ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>>[] entryManagers = new ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>>[usedCount > 0x10000 ? 0x10000 : usedCount];
//			for (int index = 0, shardIndex = 0; shardIndex < shardCount; shardIndex++)
//			{
//				ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> entryShard = _entryShards[shardIndex];
//				int entryShardEntryCount = usedCount - index;
//				entryShardEntryCount = entryShardEntryCount > 0x10000 ? 0x10000 : entryShardEntryCount;
//				entryShard.CopyTo(entryManagers, 0, entryShardEntryCount);
//				for (int entryShardEntryIndex = 0; entryShardEntryIndex < entryShardEntryCount; entryShardEntryIndex++, index++)
//				{
//					if (index == usedCount)
//						return;
//					if (!entryManagers[entryShardEntryIndex].DataConstantLength._hasItem)
//						continue;
//					array[arrayIndex++] = new DictionaryElementManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength>(index, this, entryManagers[entryShardEntryIndex]);
//				}
//			}
//		}
//		public override sealed bool TryGetValue(TKey key, out DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> dictionaryElementManager)
//		{
//			if (key == null)
//				throw new ArgumentNullException(nameof(key));
//			ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> entryManager = FindEntry(key, out int index);
//			if (entryManager == null)
//			{
//				dictionaryElementManager = null;
//				return false;
//			}
//			dictionaryElementManager = new DictionaryElementManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength>(index, this, entryManager);
//			return true;
//		}
//		public override sealed unsafe DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> Add(TKey key, TDataConstantLength dataConstantLength, TDataVariableLength dataVariableLength, params PointerManager[] pointers)
//		{
//			_version++;
//			if (key == null)
//				throw new ArgumentNullException(nameof(key));
//			if (ContainsKey(key))
//				throw new ArgumentException("The dictionary already contains an item with the specified key.");
//			if (pointers == null)
//				throw new ArgumentNullException(nameof(pointers));
//			if (pointers.Length != _pointerCount)
//				throw new ArgumentException("The number of the specified pointers is not equal to the number of pointers of an element of the collection.");
//			DictionaryMeta meta = _metaManager.DataConstantLength;
//			int index;
//			if (meta._freeIndex != -1)
//			{
//				Debug.Assert(meta._count < meta._usedCount);
//				index = meta._freeIndex;
//				meta._freeIndex = GetEntryManager(meta._freeIndex).DataConstantLength._next;
//			}
//			else
//			{
//				Debug.Assert(meta._count == meta._usedCount);
//				index = meta._usedCount++;
//				EnsureCapacity(index);
//			}
//			ArrayManager<int, EmptyData> bucketShard = _bucketShards[*((short*)&index + 1)];
//			ArrayElementValueManager<int, EmptyData> bucketManager = bucketShard[(_comparer.GetHashCode(key) & 0x7FFFFFFF) % bucketShard.Length];
//			ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> entryManager = GetEntryManager(index);
//			DictionaryEntryVariableLengthConstantLength<TDataConstantLength> entryConstantLength = entryManager.DataConstantLength;
//			entryConstantLength._hasItem = true;
//			entryConstantLength._next = bucketManager.DataConstantLength;
//			entryConstantLength._dataConstantLength = dataConstantLength;
//			entryManager.DataConstantLength = entryConstantLength;
//			DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength> entryVariableLength = entryManager.DataVariableLength;
//			entryVariableLength._key = key;
//			entryVariableLength._dataVariableLength = dataVariableLength;
//			entryManager.DataVariableLength = entryVariableLength;
//			for (int i = 0; i < _pointerCount; i++)
//				entryManager[i] = pointers[i];
//			bucketManager.DataConstantLength = index;
//			meta._count++;
//			_metaManager.DataConstantLength = meta;
//			return new DictionaryElementManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength>(index, this, entryManager);
//		}
//		public override sealed bool Remove(TKey key)
//		{
//			_version++;
//			if (key == null)
//				throw new ArgumentNullException(nameof(key));
//			int hashCode = _comparer.GetHashCode(key) & 0x7FFFFFFF;
//			foreach (ArrayManager<int, EmptyData> bucketShard in _bucketShards)
//			{
//				ArrayElementValueManager<int, EmptyData> bucketManager = bucketShard[hashCode % bucketShard.Length];
//				ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> lastEntryManager = null;
//				ArrayElementValueManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>> entryManager;
//				for (int entryIndex = bucketManager.DataConstantLength; entryIndex != -1; lastEntryManager = entryManager, entryIndex = entryManager.DataConstantLength._next)
//				{
//					entryManager = GetEntryManager(entryIndex);
//					if (!_comparer.Equals(key, entryManager.DataVariableLength._key))
//						continue;
//					DictionaryEntryVariableLengthConstantLength<TDataConstantLength> entry = entryManager.DataConstantLength;
//					if (lastEntryManager == null)
//						bucketManager.DataConstantLength = entry._next;
//					else
//					{
//						DictionaryEntryVariableLengthConstantLength<TDataConstantLength> lastEntry = lastEntryManager.DataConstantLength;
//						lastEntry._next = entry._next;
//						lastEntryManager.DataConstantLength = lastEntry;
//					}
//					DictionaryMeta meta = _metaManager.DataConstantLength;
//					entryManager.Reset();
//					entry._hasItem = false;
//					entry._next = meta._freeIndex;
//					entryManager.DataConstantLength = entry;
//					meta._freeIndex = entryIndex;
//					meta._count--;
//					_metaManager.DataConstantLength = meta;
//					return true;
//				}
//			}
//			return false;
//		}
//		public override sealed void Clear()
//		{
//			_version++;
//			DictionaryMeta meta = _metaManager.DataConstantLength;
//			meta._count = 0;
//			meta._usedCount = 0;
//			meta._freeIndex = -1;
//			_metaManager.DataConstantLength = meta;
//		}
//	}
//}