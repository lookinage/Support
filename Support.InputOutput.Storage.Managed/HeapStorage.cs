//using Support.Coding.Serialization;
//using Support.Coding.Serialization.System;
//using Support.Collections.Generic;
//using Support.InputOutput;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Diagnostics.Contracts;
//using System.IO;
//using System.Threading;

//namespace Support.InputOutput.Storage.Managed
//{
//	/// <summary>
//	/// Represents a manager of a managed heap of data.
//	/// </summary>
//	public class HeapStorage : IDisposable
//	{
//		private struct Change
//		{
//			internal sealed class Serializer : ConstantLengthSerializer<Change>
//			{
//				static internal readonly Serializer _instance;

//				static Serializer() => _instance = new Serializer();

//				private Serializer() : base(Int64SerializerBuilder.Default.Count + Int32SerializerBuilder.Default.Count) { }

//				public override sealed void Serialize(Change instance, byte[] bytes, int index)
//				{
//					Int64SerializerBuilder.Default.Serialize(instance._address, bytes, ref index);
//					Int32SerializerBuilder.Default.Serialize(instance._count, bytes, ref index);
//				}
//				public override sealed Change Deserialize(byte[] bytes, int index) => new Change(Int64SerializerBuilder.Default.Deserialize(bytes, ref index), Int32SerializerBuilder.Default.Deserialize(bytes, ref index));
//			}

//			internal readonly long _address;
//			internal readonly int _count;

//			internal Change(long address, int count)
//			{
//				_address = address;
//				_count = count;
//			}
//		}
//		private struct BufferedChange
//		{
//			internal readonly long _position;
//			internal readonly Change _change;

//			internal BufferedChange(long address, Change change)
//			{
//				_position = address;
//				_change = change;
//			}
//		}
//		internal struct Meta
//		{
//			internal sealed class Serializer : ConstantLengthSerializer<Meta>
//			{
//				static internal readonly Serializer _instance;

//				static Serializer() => _instance = new Serializer();

//				internal Serializer() : base(Int64SerializerBuilder.Default.Count + Int32SerializerBuilder.Default.Count + Int32SerializerBuilder.Default.Count + Int32SerializerBuilder.Default.Count + Int32SerializerBuilder.Default.Count + Int32SerializerBuilder.Default.Count) { }

//				public override sealed void Serialize(Meta instance, byte[] bytes, int index)
//				{
//					Int64SerializerBuilder.Default.Serialize(instance._pointerListAddress, bytes, ref index);
//					Int32SerializerBuilder.Default.Serialize(instance._pointerListCapacity, bytes, ref index);
//					Int32SerializerBuilder.Default.Serialize(instance._pointerListCount, bytes, ref index);
//					Int32SerializerBuilder.Default.Serialize(instance._pointerListUsedCount, bytes, ref index);
//					Int32SerializerBuilder.Default.Serialize(instance._pointerListFreeIndex, bytes, ref index);
//					Int32SerializerBuilder.Default.Serialize(instance._startPointerIndex, bytes, ref index);
//				}
//				public override sealed Meta Deserialize(byte[] bytes, int index) => new Meta(Int64SerializerBuilder.Default.Deserialize(bytes, ref index), Int32SerializerBuilder.Default.Deserialize(bytes, ref index), Int32SerializerBuilder.Default.Deserialize(bytes, ref index), Int32SerializerBuilder.Default.Deserialize(bytes, ref index), Int32SerializerBuilder.Default.Deserialize(bytes, ref index), Int32SerializerBuilder.Default.Deserialize(bytes, ref index));
//			}

//			internal long _pointerListAddress;
//			internal int _pointerListCapacity;
//			internal int _pointerListCount;
//			internal int _pointerListUsedCount;
//			internal int _pointerListFreeIndex;
//			internal int _startPointerIndex;

//			private Meta(long pointerListAddress, int pointerListCapacity, int pointerListCount, int pointerListUsedCount, int pointerListFreeIndex, int startPointerIndex)
//			{
//				_pointerListAddress = pointerListAddress;
//				_pointerListCapacity = pointerListCapacity;
//				_pointerListCount = pointerListCount;
//				_pointerListUsedCount = pointerListUsedCount;
//				_pointerListFreeIndex = pointerListFreeIndex;
//				_startPointerIndex = startPointerIndex;
//			}
//		}

//		private const int _defaultBufferSize = 0x1000;
//		private const int _defaultListCapacity = 4;
//		private const int _defaultFreeSegmentCountDefragmentBound = 0x10000;
//		internal const int _nullPointerIndex = -1;

//		/// <summary>
//		/// Initializes the <see cref="HeapStorage"/> class from files.
//		/// </summary>
//		/// <param name="storagePath">The path to the storage file.</param>
//		/// <param name="changePath">The path to the change file.</param>
//		/// <param name="initialize">If <see langword="true"/> the heap is initialized as new, otherwise, the heap is used as initialized before.</param>
//		/// <param name="storageBufferSize">The size of the buffer of the storage stream.</param>
//		/// <param name="changeBufferSize">The size of the buffer of the change stream.</param>
//		/// <param name="internalBufferSize">The size of the buffer for the internal operations.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="storagePath"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentNullException"><paramref name="changePath"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentException"><paramref name="initialize"/> is false and the heap is not initialized before.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="storageBufferSize"/> is less than 1.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="changeBufferSize"/> is less than 1.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="internalBufferSize"/> is less than 1.</exception>
//		static public HeapStorage FromFiles(string storagePath, string changePath, bool initialize, int storageBufferSize, int changeBufferSize, int internalBufferSize)
//		{
//			if (storagePath == null)
//				throw new ArgumentNullException(nameof(storagePath));
//			if (changePath == null)
//				throw new ArgumentNullException(nameof(changePath));
//			if (storageBufferSize < 0x1)
//				throw new ArgumentOutOfRangeException(nameof(storageBufferSize));
//			if (changeBufferSize < 0x1)
//				throw new ArgumentOutOfRangeException(nameof(changeBufferSize));
//			if (internalBufferSize < 0x1)
//				throw new ArgumentOutOfRangeException(nameof(internalBufferSize));
//			return new HeapStorage(RandomAccessStream.FromFile(storagePath, storageBufferSize), ReliableStream.FromFile(changePath, initialize, changeBufferSize), initialize, internalBufferSize);
//		}
//		/// <summary>
//		/// Initializes the <see cref="HeapStorage"/> class from files.
//		/// </summary>
//		/// <param name="storagePath">The path to the storage file.</param>
//		/// <param name="changePath">The path to the change file.</param>
//		/// <param name="initialize">If <see langword="true"/> the heap is initialized as new, otherwise, the heap is used as initialized before.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="storagePath"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentNullException"><paramref name="changePath"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentException"><paramref name="initialize"/> is false and the heap is not initialized before.</exception>
//		static public HeapStorage FromFiles(string storagePath, string changePath, bool initialize) => FromFiles(storagePath, changePath, initialize, _defaultBufferSize, _defaultBufferSize, _defaultBufferSize);

//		private readonly RandomAccessStream _storage;
//		private readonly ReliableStream _changes;
//		private readonly ThreadLocal<byte[]> _buffer;
//		private readonly List<BufferedChange> _bufferedChanges;
//		internal readonly PointerManager _nullPointerManager;
//		internal readonly object _pointerManagerLock;
//		internal readonly Dictionary<int, WeakReference<PointerManager>> _pointerManagers;
//		internal readonly List<PointerManager> _changedPointerManagers;
//		internal readonly object _readLock;
//		internal readonly ManagedStorageMap _segmentManager;
//		internal int _freeSegmentCountDefragmentBound;
//		internal Meta _meta;
//		internal bool _disposed;

//		private HeapStorage(RandomAccessStream storage, ReliableStream changes, bool initialize, int internalBufferSize)
//		{
//			_storage = storage;
//			_changes = changes;
//			_buffer = new ThreadLocal<byte[]>(() => new byte[internalBufferSize]);
//			_bufferedChanges = new List<BufferedChange>();
//			_nullPointerManager = new PointerManager(this, _nullPointerIndex, default);
//			_pointerManagerLock = new object();
//			_pointerManagers = new Dictionary<int, WeakReference<PointerManager>>();
//			_changedPointerManagers = new List<PointerManager>();
//			_readLock = new object();
//			FreeSegmentCountDefragmentBound = _defaultFreeSegmentCountDefragmentBound;
//			if (initialize)
//			{
//				_meta._pointerListAddress = Meta.Serializer._instance.Count;
//				_meta._pointerListCapacity = _defaultListCapacity;
//				_meta._pointerListCount = 0x0;
//				_meta._pointerListUsedCount = 0x0;
//				_meta._pointerListFreeIndex = -0x1;
//				_meta._startPointerIndex = _nullPointerIndex;
//				WriteMeta();
//				_changes.Commit();
//			}
//			_meta = Read(0x0, Meta.Serializer._instance);
//			long totalAmount = _meta._pointerListAddress + Pointer.Serializer._default.Count * (long)_meta._pointerListCapacity;
//			IEnumerator<Pointer> pointerEnumerator = GetPointerEnumerator();
//			for (int pointerIndex = 0x0; pointerEnumerator.MoveNext(); pointerIndex++)
//			{
//				Pointer pointer = pointerEnumerator.Current;
//				if (pointer._address == 0x0)
//				{
//					Contract.Assert(pointer._size >= -0x1 && pointer._size < _meta._pointerListUsedCount);
//					continue;
//				}
//				if (pointer._address + pointer._size < totalAmount)
//					continue;
//				totalAmount = pointer._address + pointer._size;
//			}
//			ManagedStorageMap inversedSegmentManager = new ManagedStorageMap(totalAmount);
//			inversedSegmentManager.Free(new Pointer(Meta.Serializer._instance.Count, 0x0));
//			inversedSegmentManager.Free(new Pointer(Pointer.Serializer._default.Count * (long)_meta._pointerListCapacity, _meta._pointerListAddress));
//			pointerEnumerator = GetPointerEnumerator();
//			for (int pointerIndex = 0x0; pointerEnumerator.MoveNext(); pointerIndex++)
//			{
//				Pointer pointer = pointerEnumerator.Current;
//				if (pointer._address == 0x0)
//					continue;
//				inversedSegmentManager.Free(pointer);
//			}
//			_segmentManager = new ManagedStorageMap(totalAmount, inversedSegmentManager);
//		}

//		/// <summary>
//		/// Gets the number of free segments.
//		/// </summary>
//		public int FreeSegmentCount => _segmentManager.SegmentCount;
//		/// <summary>
//		/// Gets the number of allocated segments.
//		/// </summary>
//		public int AllocatedSegmentCount => _meta._pointerListCount;
//		/// <summary>
//		/// Gets the total amount of bytes.
//		/// </summary>
//		public long TotalAmount => _segmentManager.Capacity;
//		/// <summary>
//		/// Gets the amount of free bytes.
//		/// </summary>
//		public long FreeAmount => _segmentManager.Freed;
//		/// <summary>
//		/// Gets the amount of allocated bytes.
//		/// </summary>
//		public long AllocatedAmount => _segmentManager.Allocated;
//		/// <summary>
//		/// Gets the <see cref="PointerManager"/> of the null pointer.
//		/// </summary>
//		/// <exception cref="ObjectDisposedException">The <see cref="HeapStorage"/> is disposed.</exception>
//		public PointerManager NullPointerManager => _nullPointerManager;
//		/// <summary>
//		/// Gets or sets the <see cref="PointerManager"/> of the pointer that starts the work.
//		/// </summary>
//		/// <exception cref="ArgumentNullException">The specified <see cref="PointerManager"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentException">The specified <see cref="PointerManager"/> is of another <see cref="HeapStorage"/> or the segment of the pointer is freed.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="HeapStorage"/> is disposed.</exception>
//		public PointerManager StartPointerManager
//		{
//			get => Get(_meta._startPointerIndex);
//			set
//			{
//				if (Disposed)
//					throw new ObjectDisposedException(string.Empty);
//				if (value == null)
//					throw new ArgumentNullException(nameof(value));
//				if (value._heapManager != this)
//					throw new ArgumentException(string.Format("The specified {0} is of another {1}.", nameof(PointerManager), nameof(HeapStorage)));
//				if (value._freed)
//					throw new ArgumentException("The segment of the pointer is freed.");
//				if (value._index == _meta._startPointerIndex)
//					return;
//				_meta._startPointerIndex = value._index;
//				WriteMeta();
//			}
//		}

//		/// <summary>
//		/// Gets or sets the bound of number of free segments of the heap for the auto defragmentation.
//		/// </summary>
//		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 1.</exception>
//		public int FreeSegmentCountDefragmentBound { get => _freeSegmentCountDefragmentBound; set => _freeSegmentCountDefragmentBound = value < 1 ? throw new ArgumentOutOfRangeException(nameof(value)) : value; }
//		/// <summary>
//		/// Gets or sets the max number of pieces of the buffer.
//		/// </summary>
//		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 1.</exception>
//		public int MaxBufferPieceCount { get => _storage.MaxBufferPieceCount; set => _storage.MaxBufferPieceCount = value; }
//		/// <summary>
//		/// Gets or sets the max number of buffered bytes.
//		/// </summary>
//		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 1.</exception>
//		public long MaxBufferByteCount { get => _storage.MaxBufferByteCount; set => _storage.MaxBufferByteCount = value; }
//		/// <summary>
//		/// Gets the value meaning whether all the changes are committed.
//		/// </summary>
//		public bool Committed => _changes.Committed;
//		/// <summary>
//		/// Gets the value meaning whether the <see cref="HeapStorage"/> is disposed.
//		/// </summary>
//		public bool Disposed => _disposed;

//		internal void Read(long address, int count, byte[] buffer, int index)
//		{
//			lock (_storage)
//				_storage.Read(address, count, buffer, index);
//			foreach (BufferedChange bufferedChange in _bufferedChanges)
//			{
//				if (bufferedChange._change._address + bufferedChange._change._count <= address || bufferedChange._change._address >= address + count)
//					continue;
//				int offsetDifference = (int)(bufferedChange._change._address - address);
//				int copyCount;
//				if (offsetDifference > 0x0)
//				{
//					copyCount = count - offsetDifference;
//					_changes.Read(bufferedChange._position, copyCount < bufferedChange._change._count ? copyCount : bufferedChange._change._count, buffer, index + offsetDifference);
//					continue;
//				}
//				copyCount = bufferedChange._change._count + offsetDifference;
//				_changes.Read(bufferedChange._position - offsetDifference, copyCount < count ? copyCount : count, buffer, index);
//			}
//		}
//		internal T Read<T>(long address, int count, ISerializer<T> serializer)
//		{
//			//if (count > long.MaxValue - address)
//			//	throw new ArgumentException(string.Format("The number of bytes of the value is greater than the number of bytes from {0} to the end of the storage.", nameof(address)));
//			byte[] buffer = _buffer.Value;
//			if (ArrayHelper.EnsureLength(ref buffer, count))
//				_buffer.Value = buffer;
//			Read(address, count, buffer, 0x0);
//			return serializer.Deserialize(count, buffer, 0x0);
//		}
//		internal T Read<T>(long address, IConstantLengthSerializer<T> serializer)
//		{
//			int count = serializer.Count;
//			//if (count > long.MaxValue - address)
//			//	throw new ArgumentException(string.Format("The number of bytes of the value is greater than the number of bytes from {0} to the end of the storage.", nameof(address)));
//			byte[] buffer = _buffer.Value;
//			if (ArrayHelper.EnsureLength(ref buffer, count))
//				_buffer.Value = buffer;
//			Read(address, count, buffer, 0x0);
//			return serializer.Deserialize(buffer, 0x0);
//		}
//		internal void Write(byte[] buffer, int index, int count, long address)
//		{
//			if (count == 0x0)
//				return;
//			Change change = new Change(address, count);
//			int changeCount = Change.Serializer._instance.Count;
//			byte[] changeBuffer = _buffer.Value;
//			if (ArrayHelper.EnsureLength(ref changeBuffer, changeCount))
//				_buffer.Value = changeBuffer;
//			Change.Serializer._instance.Serialize(change, changeBuffer, 0x0);
//			_changes.Write(changeBuffer, 0x0, changeCount);
//			_bufferedChanges.Add(new BufferedChange(_changes.Length, change));
//			_changes.Write(buffer, index, count);
//		}
//		internal void Write<T>(T value, ISerializer<T> serializer, long address)
//		{
//			int count = serializer.Count(value);
//			//if (count > long.MaxValue - address)
//			//	throw new ArgumentException(string.Format("The number of bytes of {0} is greater than the number of bytes from the {1} to the end of the storage.", nameof(value), nameof(address)));
//			byte[] buffer = _buffer.Value;
//			if (ArrayHelper.EnsureLength(ref buffer, count))
//				_buffer.Value = buffer;
//			serializer.Serialize(value, buffer, 0x0);
//			Write(buffer, 0x0, count, address);
//		}
//		internal void Write<T>(T instance, IConstantLengthSerializer<T> serializer, long address)
//		{
//			int count = serializer.Count;
//			//if (count > long.MaxValue - address)
//			//	throw new ArgumentException(string.Format("The number of bytes of {0} is greater than the number of bytes from the {1} to the end of the storage.", nameof(instance), nameof(address)));
//			byte[] buffer = _buffer.Value;
//			if (ArrayHelper.EnsureLength(ref buffer, count))
//				_buffer.Value = buffer;
//			serializer.Serialize(instance, buffer, 0x0);
//			Write(buffer, 0x0, count, address);
//		}
//		internal void Copy(long sourcePosition, long count, long destinationPosition)
//		{
//			if (destinationPosition == sourcePosition)
//				return;
//			if (count == 0x0)
//				return;
//			byte[] buffer = _buffer.Value;
//			int bufferSize = buffer.Length;
//			if (count <= bufferSize)
//			{
//				buffer = new byte[count];
//				Read(sourcePosition, (int)count, buffer, 0x0);
//				Write(buffer, 0x0, (int)count, destinationPosition);
//				return;
//			}
//			int copyCount;
//			if (destinationPosition < sourcePosition)
//			{
//				for (; count != 0x0; count -= copyCount)
//				{
//					copyCount = count > bufferSize ? bufferSize : (int)count;
//					Read(sourcePosition, copyCount, buffer, 0x0);
//					Write(buffer, 0x0, copyCount, destinationPosition);
//					sourcePosition += copyCount;
//					destinationPosition += copyCount;
//				}
//				return;
//			}
//			for (sourcePosition += count, destinationPosition += count; count != 0x0; count -= copyCount)
//			{
//				copyCount = count > bufferSize ? bufferSize : (int)count;
//				sourcePosition -= copyCount;
//				destinationPosition -= copyCount;
//				Read(sourcePosition, copyCount, buffer, 0x0);
//				Write(buffer, 0x0, copyCount, destinationPosition);
//			}
//		}
//		internal void WriteMeta() => Write(_meta, Meta.Serializer._instance, 0x0);
//		internal Pointer ReadPointer(int index) => Read(_meta._pointerListAddress + Pointer.Serializer._default.Count * (long)index, Pointer.Serializer._default);
//		internal void WritePointer(int index, Pointer pointer) => Write(pointer, Pointer.Serializer._default, _meta._pointerListAddress + Pointer.Serializer._default.Count * (long)index);
//		internal void IncreasePointerListCapacity()
//		{
//			int newListCapacity = _meta._pointerListCapacity << 0x1;
//			if (newListCapacity < 0x0)
//				newListCapacity = int.MaxValue;
//			Pointer listPointer = new Pointer(Pointer.Serializer._default.Count * (long)_meta._pointerListCapacity, _meta._pointerListAddress);
//			Pointer newlistPointer;
//			newlistPointer._size = Pointer.Serializer._default.Count * (long)newListCapacity;
//			newlistPointer._address = _segmentManager.Allocate(newlistPointer._size);
//			Copy(listPointer._address, listPointer._size, newlistPointer._address);
//			_meta._pointerListCapacity = newListCapacity;
//			_meta._pointerListAddress = newlistPointer._address;
//			_segmentManager.Free(listPointer);
//		}
//		internal int TakeFreePointerListIndex()
//		{
//			int index;
//			if (_meta._pointerListFreeIndex != -0x1)
//			{
//				Debug.Assert(_meta._pointerListCount < _meta._pointerListUsedCount);
//				index = _meta._pointerListFreeIndex;
//				_meta._pointerListFreeIndex = (int)ReadPointer(index)._size;
//			}
//			else
//			{
//				Debug.Assert(_meta._pointerListCount == _meta._pointerListUsedCount);
//				if (_meta._pointerListUsedCount == _meta._pointerListCapacity)
//					IncreasePointerListCapacity();
//				index = _meta._pointerListUsedCount++;
//			}
//			_meta._pointerListCount++;
//			WriteMeta();
//			return index;
//		}
//		internal IEnumerator<Pointer> GetPointerEnumerator()
//		{
//			long pointerListAddress = _meta._pointerListAddress;
//			int pointerListUsedCount = _meta._pointerListUsedCount;
//			byte[] buffer = null;
//			for (int index = 0; index < pointerListUsedCount;)
//			{
//				int readLength = GetReadWriteLength(Pointer.Serializer._default.Count, pointerListUsedCount - index);
//				int readSize = Pointer.Serializer._default.Count * readLength;
//				_ = ArrayHelper.EnsureLength(ref buffer, readSize);
//				lock (_storage)
//					_storage.Read(pointerListAddress + Pointer.Serializer._default.Count * (long)index, readSize, buffer, 0);
//				for (int floor = index, roof = index + readLength; index < roof; index++)
//					yield return Pointer.Serializer._default.Deserialize(buffer, Pointer.Serializer._default.Count * (index - floor));
//			}
//		}
//		internal void ReleasePointerManager(PointerManager pointerManager)
//		{
//			if (pointerManager._freed)
//				return;
//			lock (_pointerManagerLock)
//				_ = _pointerManagers.Remove(pointerManager._index);
//		}
//		/// <summary>
//		/// Returns a number of elements of a specified array to read or write at a time.
//		/// </summary>
//		/// <param name="size">The size of the array element.</param>
//		/// <param name="length">The length of the array.</param>
//		/// <returns>A number of elements of a specified array to read or write at a time.</returns>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is less than 0.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than 0.</exception>
//		public int GetReadWriteLength(int size, int length)
//		{
//			if (size < 0x0)
//				throw new ArgumentOutOfRangeException(nameof(size));
//			if (length < 0x0)
//				throw new ArgumentOutOfRangeException(nameof(length));
//			int bufferSize = _buffer.Value.Length;
//			if (size > bufferSize)
//				return 0x1;
//			else if (size * length > bufferSize << 0x1)
//				return bufferSize / size;
//			return length;
//		}
//		/// <summary>
//		/// Allocates a segment in the heap and returns the <see cref="PointerManager"/> of the pointer of the segment.
//		/// </summary>
//		/// <param name="size">The size of the segment to allocate.</param>
//		/// <returns>The <see cref="PointerManager"/> of the pointer of the allocated segment.</returns>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is less than or equal to 0.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="HeapStorage"/> is disposed.</exception>
//		public PointerManager Create(long size)
//		{
//			if (Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (size <= 0x0)
//				throw new ArgumentOutOfRangeException(nameof(size));
//			int index;
//			Pointer pointer;
//			WritePointer(index = TakeFreePointerListIndex(), pointer = new Pointer(size, _segmentManager.Allocate(size)));
//			PointerManager pointerManager = new PointerManager(this, index, pointer);
//			lock (_pointerManagerLock)
//				_pointerManagers.Add(index, new WeakReference<PointerManager>(pointerManager));
//			_changedPointerManagers.Add(pointerManager);
//			return pointerManager;
//		}
//		/// <summary>
//		/// Returns a <see cref="PointerManager"/> of the specified pointer.
//		/// </summary>
//		/// <param name="index">The index of the pointer.</param>
//		/// <returns>A <see cref="PointerManager"/> of the specified pointer.</returns>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than 0.</exception>
//		/// <exception cref="ArgumentException">There is no pointer at the specified index.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="HeapStorage"/> is disposed.</exception>
//		public PointerManager Get(int index)
//		{
//			if (Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (index == _nullPointerIndex)
//				return _nullPointerManager;
//			if (index < 0x0)
//				throw new ArgumentOutOfRangeException(nameof(index));
//			if (index >= _meta._pointerListUsedCount)
//				throw new ArgumentException("There is no pointer at the specified index.");
//			Find:
//			bool wait = false;
//			Monitor.Enter(_pointerManagerLock);
//			try
//			{
//				PointerManager pointerManager;
//				if (_pointerManagers.TryGetValue(index, out WeakReference<PointerManager> reference))
//				{
//					if (reference.TryGetTarget(out pointerManager))
//						return pointerManager;
//					wait = true;
//					goto Find;
//				}
//				Pointer pointer = ReadPointer(index);
//				if (pointer._address == 0x0)
//					throw new ArgumentException("There is no pointer at the specified index.");
//				pointerManager = new PointerManager(this, index, pointer);
//				_pointerManagers.Add(index, new WeakReference<PointerManager>(pointerManager));
//				return pointerManager;
//			}
//			finally
//			{
//				Monitor.Exit(_pointerManagerLock);
//				if (wait)
//					GC.WaitForPendingFinalizers();
//			}
//		}
//		/// <summary>
//		/// Defragments the heap.
//		/// </summary>
//		/// <exception cref="InvalidOperationException">The non-committed changes exist.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="HeapStorage"/> is disposed.</exception>
//		public void Defragment()
//		{
//			if (!Committed)
//				throw new InvalidOperationException("The non-committed changes exist.");
//			if (Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (_segmentManager.SegmentCount == 0x0)
//				return;
//			long totalAmount = _segmentManager.Capacity;
//			RedBlackTreeDictionary<long, long> shiftByAddress = _segmentManager.Defragment();
//			lock (_pointerManagerLock)
//			{
//				long shift;
//				IEnumerator<Pointer> pointerEnumerator = GetPointerEnumerator();
//				for (int pointerIndex = 0x0; pointerEnumerator.MoveNext(); pointerIndex++)
//				{
//					Pointer pointer = pointerEnumerator.Current;
//					if (pointer._address == 0x0 || !shiftByAddress.TryGetFirstOnRay(true, pointer._address, out _, out shift))
//						continue;
//					pointer._address -= shift;
//					WritePointer(pointerIndex, pointer);
//				}
//				if (shiftByAddress.TryGetFirstOnRay(true, _meta._pointerListAddress, out _, out shift))
//					_meta._pointerListAddress -= shift;
//				WriteMeta();
//				lock (_readLock)
//				{
//					foreach (WeakReference<PointerManager> reference in _pointerManagers.Values)
//					{
//						if (!reference.TryGetTarget(out PointerManager pointerManager) || !shiftByAddress.TryGetFirstOnRay(true, pointerManager._pointer._address, out _, out shift))
//							continue;
//						pointerManager._pointer._address -= shift;
//					}
//					long address = 0;
//					shift = 0;
//					foreach (KeyValuePair<long, long> pair in shiftByAddress)
//					{
//						long nextAddress = pair.Key;
//						long nextShift = pair.Value;
//						if (shift != 0)
//							Copy(address, address - shift, nextAddress - address - nextShift + shift);
//						address = nextAddress;
//						shift = nextShift;
//					}
//					Copy(address, address - shift, totalAmount - address);
//				}
//			}
//		}
//		/// <summary>
//		/// Reads bytes at the specified source offset of the specified source segment and writes it at the specified destination offset of the specified destination segment.
//		/// </summary>
//		/// <param name="sourcePointerManager">The <see cref="PointerManager"/> of the pointer of the source segment.</param>
//		/// <param name="sourceOffset">The offset of the source segment.</param>
//		/// <param name="destinationPointerManager">The <see cref="PointerManager"/> of the pointer of the destination segment.</param>
//		/// <param name="destinationOffset">The offset of the destination segment.</param>
//		/// <param name="count">The number of the bytes.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="sourcePointerManager"/> is <see langword="null"/> or <paramref name="destinationPointerManager"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="sourceOffset"/> is less than 0 or greater than the source segment size or <paramref name="destinationOffset"/> is less than 0 or greater than the destination segment size or <paramref name="count"/> is less than 0 or greater than the source or destination segment size.</exception>
//		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="sourceOffset"/> to the end of the source segment or <paramref name="count"/> is greater than the number of bytes from <paramref name="destinationOffset"/> to the end of the destination segment or <paramref name="sourcePointerManager"/> or <paramref name="destinationPointerManager"/> is of another <see cref="HeapStorage"/> or the source or destination pointer does not point to a segment or the source or destination segment is freed.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="HeapStorage"/> is disposed.</exception>
//		public void Copy(PointerManager sourcePointerManager, long sourceOffset, PointerManager destinationPointerManager, long destinationOffset, long count)
//		{
//			if (Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (sourcePointerManager == null)
//				throw new ArgumentNullException(nameof(sourcePointerManager));
//			if (destinationPointerManager == null)
//				throw new ArgumentNullException(nameof(destinationPointerManager));
//			if (sourceOffset < 0 || sourceOffset > sourcePointerManager._pointer._size)
//				throw new ArgumentOutOfRangeException(nameof(sourceOffset));
//			if (destinationOffset < 0 || destinationOffset > destinationPointerManager._pointer._size)
//				throw new ArgumentOutOfRangeException(nameof(destinationOffset));
//			if (count < 0 || count > sourcePointerManager._pointer._size || count > destinationPointerManager._pointer._size)
//				throw new ArgumentOutOfRangeException(nameof(count));
//			if (count > sourcePointerManager._pointer._size - sourceOffset)
//				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from the {1} to the end of the source segment.", nameof(count), nameof(sourceOffset)));
//			if (count > destinationPointerManager._pointer._size - destinationOffset)
//				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from the {1} to the end of the destination segment.", nameof(count), nameof(destinationOffset)));
//			if (sourcePointerManager._heapManager != this)
//				throw new ArgumentException(string.Format("{0} is of another {1}.", nameof(sourcePointerManager), nameof(HeapStorage)));
//			if (destinationPointerManager._heapManager != this)
//				throw new ArgumentException(string.Format("{0} is of another {1}.", nameof(destinationPointerManager), nameof(HeapStorage)));
//			if (sourcePointerManager.IsNull)
//				throw new ArgumentException("The source pointer does not point to a segment.");
//			if (destinationPointerManager.IsNull)
//				throw new ArgumentException("The destination pointer does not point to a segment.");
//			if (sourcePointerManager._freed)
//				throw new ArgumentException("The source segment is freed.");
//			if (destinationPointerManager._freed)
//				throw new ArgumentException("The destination segment is freed.");
//			Copy(sourcePointerManager._pointer._address + sourceOffset, count, destinationPointerManager._pointer._address + destinationOffset);
//		}
//		/// <summary>
//		/// Commits the made changes.
//		/// </summary>
//		/// <exception cref="ObjectDisposedException">The <see cref="HeapStorage"/> is disposed.</exception>
//		public void Commit()
//		{
//			if (Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			_changes.Commit();
//			_changedPointerManagers.Clear();
//			if (_segmentManager.SegmentCount < _freeSegmentCountDefragmentBound)
//				return;
//			Defragment();
//		}
//		/// <summary>
//		/// Releases all resources used by the <see cref="HeapStorage"/>.
//		/// </summary>
//		public void Close()
//		{
//			_storage.Close();
//			_changes.Close();
//		}
//		void IDisposable.Dispose() => Close();
//	}
//}