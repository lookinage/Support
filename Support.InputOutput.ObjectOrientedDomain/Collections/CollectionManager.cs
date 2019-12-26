//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using Noname.BitConversion;

//namespace Noname.IO.ObjectOrientedDomain.Collections
//{
//	/// <summary>
//	/// Represents a manager of a collection of elements that is in a domain.
//	/// </summary>
//	public sealed class CollectionManager : ICollection<CollectionValueManager>
//	{
//		private struct Enumerator : IEnumerator<CollectionElementManager>
//		{
//			private readonly CollectionManager _collectionManager;
//			private readonly int _version;
//			private int _current;
//			private CollectionElementManager _currentItem;

//			internal Enumerator(CollectionManager collectionManager)
//			{
//				_collectionManager = collectionManager;
//				_version = collectionManager._version;
//				_current = -1;
//				_currentItem = null;
//			}

//			object IEnumerator.Current => _currentItem;

//			CollectionElementManager IEnumerator<CollectionElementManager>.Current => _currentItem;

//			void IDisposable.Dispose() { }

//			bool IEnumerator.MoveNext()
//			{
//				if (_version != _collectionManager._version)
//					throw new InvalidOperationException();
//				for (; ; )
//				{
//					if (++_current == _collectionManager._metaManager.DataConstantLength._usedCount)
//						return false;
//					ArrayElementManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> entryManager = _collectionManager.GetEntryManager(_current);
//					if (!entryManager.DataConstantLength.HasItem)
//						continue;
//					_currentItem = new CollectionElementManager(_collectionManager, _current, entryManager);
//					return true;
//				}
//			}
//			void IEnumerator.Reset()
//			{
//				_current = -1;
//				_currentItem = null;
//			}
//		}

//		private readonly ValueManager _metaManager;
//		private readonly int _dataSize;
//		private readonly int _pointerCount;
//		private ArrayManager[] _entryShards;
//		private int _version;

//		internal CollectionManager(ValueManager metaManager, int dataSize, int pointerCount, ArrayManager[] entryShards)
//		{
//			_metaManager = metaManager;
//			_dataSize = dataSize;
//			_pointerCount = pointerCount;
//			_entryShards = entryShards;
//			_version = 0;
//		}

//		bool ICollection<CollectionValueManager>.IsReadOnly => false;

//		/// <summary>
//		/// Gets the <see cref="ArrayManager"/> of the collection.
//		/// </summary>
//		public ArrayManager Pointer => _metaManager.ArrayManager;
//		/// <summary>
//		/// Gets the size of the data of values of elements of the collection.
//		/// </summary>
//		public int DataSize => _dataSize;
//		/// <summary>
//		/// Gets the number of pointers of values of elements of the collection.
//		/// </summary>
//		public int PointerCount => _pointerCount;
//		/// <summary>
//		/// Gets the number of elements contained in the collection.
//		/// </summary>
//		public int Count => _metaManager.DataConstantLength._count;
//		/// <summary>
//		/// Gets a <see cref="CollectionValueManager"/> of an item with the specified key.
//		/// </summary>
//		/// <param name="key">The key of the item to get.</param>
//		/// <returns>A <see cref="CollectionValueManager"/> of an item with the specified key.</returns>
//		/// <exception cref="ArgumentException">The collection does not contain an item with the specified key.</exception>
//		public CollectionValueManager this[int key]
//		{
//			get
//			{
//				ValueManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> entryManager = FindEntry(key);
//				if (entryManager == null)
//					throw new ArgumentException("The collection does not contain an item with the specified key.");
//				return new CollectionValueManager(this, key, entryManager);
//			}
//		}

//		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//		void ICollection<CollectionValueManager>.Add(CollectionValueManager item) => throw new NotSupportedException();
//		bool ICollection<CollectionValueManager>.Remove(CollectionValueManager item) => throw new NotSupportedException();
//		bool ICollection<CollectionValueManager>.Contains(CollectionValueManager item) => throw new NotSupportedException();

//		private unsafe void IncreaseShardCapacity(int shardIndex)
//		{
//			ArrayManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> entryShard = _entryShards[shardIndex];
//			int shardCapacity = entryShard.Length << 1;
//			if (shardCapacity > 0x10000)
//				shardCapacity = 0x10000;
//			ArrayManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> newEntryShard = ArrayManagerActivator.Create(_metaManager.Pointer.Domain, _entryConstantLengthBitConverter, _entryVariableLengthBitConverter, _pointerCount, shardCapacity);
//			ArrayManagerActivator.Copy(entryShard, newEntryShard, entryShard.Length);
//			ArrayManagerActivator.Open(_metaManager[0], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance)[shardIndex][0] = newEntryShard.Pointer;
//			_entryShards[shardIndex] = newEntryShard;
//		}
//		private unsafe void IncreaseShardCount(int shardIndex)
//		{
//			ArrayManager<EmptyData, EmptyData> entryShardManager = ArrayManagerActivator.Open(_metaManager[0], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance);
//			short shardCount = (short)entryShardManager.Length;
//			shardCount = shardCount == 0 ? (short)1 : (short)(shardCount << 1);
//			if (shardCount < 0)
//				shardCount = short.MaxValue;
//			ArrayManager<EmptyData, EmptyData> newEntryShardManager = ArrayManagerActivator.Create(_metaManager.Pointer.Domain, EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance, 1, shardCount);
//			ArrayManagerActivator.Copy(entryShardManager, newEntryShardManager, entryShardManager.Length);
//			ArrayManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>>[] newEntryShards = new ArrayManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>>[shardCount];
//			Array.Copy(_entryShards, newEntryShards, _entryShards.Length);
//			_metaManager[0] = newEntryShardManager.Pointer;
//			_entryShards = newEntryShards;
//		}
//		private unsafe void EnsureCapacity(int index)
//		{
//			int shardIndex = *((short*)&index + 1);
//			if (shardIndex == _entryShards.Length)
//				IncreaseShardCount(shardIndex);
//			ArrayManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> entryShard = _entryShards[shardIndex];
//			if (entryShard == null)
//			{
//				entryShard = ArrayManagerActivator.Create(_metaManager.Pointer.Domain, _entryConstantLengthBitConverter, _entryVariableLengthBitConverter, _pointerCount, CollectionManagerActivator._defaultShardCapacity);
//				ArrayManagerActivator.Open(_metaManager[0], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance)[shardIndex][0] = entryShard.Pointer;
//				_entryShards[shardIndex] = entryShard;
//			}
//			if (*(ushort*)&index < entryShard.Length)
//				return;
//			IncreaseShardCapacity(shardIndex);
//		}
//		private ValueManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> FindEntry(int key)
//		{
//			ValueManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> entryManager;
//			return key >= 0 && key < _metaManager.DataConstantLength._usedCount && (entryManager = GetEntryManager(key)).DataConstantLength.HasItem ? entryManager : null;
//		}
//		internal unsafe bool CheckEntryShard(int index, ArrayManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> entryShard) => entryShard == _entryShards[*((short*)&index + 1)];
//		internal unsafe ValueManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> GetEntryManager(int index) => _entryShards[*((short*)&index + 1)][*(ushort*)&index];
//		internal void IncreaseVersion() => _version++;
//		/// <summary>
//		/// Returns an enumerator that iterates through the collection.
//		/// </summary>
//		/// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.</returns>
//		public IEnumerator<CollectionValueManager> GetEnumerator() => new Enumerator(this);
//		/// <summary>
//		/// Determines whether the collection contains an item with the specified key.
//		/// </summary>
//		/// <param name="key">The key to locate in the collection.</param>
//		/// <returns>true if the collection contains an element with the key; otherwise, false.</returns>
//		public bool ContainsKey(int key) => FindEntry(key) != null;
//		/// <summary>
//		/// Copies the elements of the collection to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
//		/// </summary>
//		/// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from the collection. The <see cref="Array"/> must have zero-based indexing.</param>
//		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is not a valid index of <paramref name="array"/>.</exception>
//		/// <exception cref="ArgumentException">The number of elements in the collection is greater than the available space from the <paramref name="arrayIndex"/> to the end of the array.</exception>
//		public void CopyTo(CollectionValueManager[] array, int arrayIndex)
//		{
//			if (array == null)
//				throw new ArgumentNullException(nameof(array));
//			if (arrayIndex < 0 || arrayIndex > array.Length)
//				throw new ArgumentOutOfRangeException(nameof(arrayIndex));
//			if (Count > array.Length - arrayIndex)
//				throw new ArgumentException(string.Format("The number of elements in the collection is greater than the available space from the {0} to the end of the array.", nameof(arrayIndex)));
//			int usedCount = _metaManager.DataConstantLength._usedCount;
//			if (usedCount == 0)
//				return;
//			int shardCount = _entryShards.Length;
//			ValueManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>>[] entryManagers = new ValueManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>>[usedCount > 0x10000 ? 0x10000 : usedCount];
//			for (int index = 0, shardIndex = 0; shardIndex < shardCount; shardIndex++)
//			{
//				ArrayManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> entryShard = _entryShards[shardIndex];
//				int entryShardEntryCount = usedCount - index;
//				entryShardEntryCount = entryShardEntryCount > 0x10000 ? 0x10000 : entryShardEntryCount;
//				entryShard.CopyTo(entryManagers, 0, entryShardEntryCount);
//				for (int entryShardEntryIndex = 0; entryShardEntryIndex < entryShardEntryCount; entryShardEntryIndex++, index++)
//				{
//					if (index == usedCount)
//						return;
//					if (!entryManagers[entryShardEntryIndex].DataConstantLength.HasItem)
//						continue;
//					array[arrayIndex++] = new CollectionValueManager(this, index, entryManagers[entryShardEntryIndex]);
//				}
//			}
//		}
//		/// <summary>
//		/// Adds an item into the collection.
//		/// </summary>
//		/// <param name="dataConstantLength">The <typeparamref name="TDataConstantLength"/> of the item to add.</param>
//		/// <param name="dataVariableLength">The <typeparamref name="TDataVariableLength"/> of the item to add.</param>
//		/// <param name="pointers">The pointers of the item to add.</param>
//		/// <returns>A <see cref="CollectionValueManager"/> of the element of the added item.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="pointers"/> is null.</exception>
//		/// <exception cref="ArgumentException">The number of the specified pointers is not equal to the number of pointers of an element of the collection.</exception>
//		public CollectionValueManager Add(TDataConstantLength dataConstantLength, TDataVariableLength dataVariableLength, params PointerManager[] pointers)
//		{
//			_version++;
//			if (pointers == null)
//				throw new ArgumentNullException(nameof(pointers));
//			if (pointers.Length != _pointerCount)
//				throw new ArgumentException("The number of the specified pointers is not equal to the number of pointers of an element of the collection.");
//			CollectionMeta meta = _metaManager.DataConstantLength;
//			int index;
//			if (meta._freeIndex != -1)
//			{
//				Debug.Assert(meta._count < meta._usedCount);
//				index = meta._freeIndex;
//				meta._freeIndex = GetEntryManager(meta._freeIndex).DataConstantLength._nextFreeIndex;
//			}
//			else
//			{
//				Debug.Assert(meta._count == meta._usedCount);
//				index = meta._usedCount++;
//				EnsureCapacity(index);
//			}
//			ValueManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> entryManager = GetEntryManager(index);
//			CollectionEntryConstantLength<TDataConstantLength> entryConstantLength = entryManager.DataConstantLength;
//			entryConstantLength.SetHasItem();
//			entryConstantLength._dataConstantLength = dataConstantLength;
//			entryManager.DataConstantLength = entryConstantLength;
//			CollectionEntryVariableLength<TDataVariableLength> entryVariableLength = entryManager.DataVariableLength;
//			entryVariableLength._dataVariableLength = dataVariableLength;
//			entryManager.DataVariableLength = entryVariableLength;
//			for (int i = 0; i < _pointerCount; i++)
//				entryManager[i] = pointers[i];
//			meta._count++;
//			_metaManager.DataConstantLength = meta;
//			return new CollectionValueManager(this, index, entryManager);
//		}
//		/// <summary>
//		/// Removes an item with the specified key.
//		/// </summary>
//		/// <param name="key">The key of the item to remove.</param>
//		/// <returns>true if the item was found and removed from the collection; otherwise, false.</returns>
//		public bool Remove(int key)
//		{
//			_version++;
//			CollectionMeta meta = _metaManager.DataConstantLength;
//			ValueManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> entryManager;
//			if (key < 0 || key >= meta._usedCount || !(entryManager = GetEntryManager(key)).DataConstantLength.HasItem)
//				return false;
//			entryManager.Reset();
//			CollectionEntryConstantLength<TDataConstantLength> entryConstantLength = entryManager.DataConstantLength;
//			entryConstantLength._nextFreeIndex = meta._freeIndex;
//			entryManager.DataConstantLength = entryConstantLength;
//			meta._freeIndex = key;
//			meta._count--;
//			_metaManager.DataConstantLength = meta;
//			return true;
//		}
//		/// <summary>
//		/// Gets the <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> of the element associated with the specified key.
//		/// </summary>
//		/// <param name="key">The key whose <see cref="CollectionValueManager"/> to get.</param>
//		/// <param name="collectionElementManager">The <see cref="CollectionValueManager"/> of the element associated with the specified key.</param>
//		/// <returns>true if the collection contains an element with the specified key; otherwise, false.</returns>
//		public bool TryGetValue(int key, out CollectionValueManager collectionElementManager)
//		{
//			ValueManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>> entryManager = FindEntry(key);
//			if (entryManager == null)
//			{
//				collectionElementManager = null;
//				return false;
//			}
//			collectionElementManager = new CollectionValueManager(this, key, entryManager);
//			return true;
//		}
//		/// <summary>
//		/// Removes all items from the collection.
//		/// </summary>
//		public void Clear()
//		{
//			_version++;
//			CollectionMeta meta = _metaManager.DataConstantLength;
//			meta._count = 0;
//			meta._usedCount = 0;
//			meta._freeIndex = -1;
//			_metaManager.DataConstantLength = meta;
//		}
//	}
//}