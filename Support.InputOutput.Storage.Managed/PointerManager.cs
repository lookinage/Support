//using Support.Coding.Serialization;
//using System;

//namespace Support.InputOutput.Storage.Managed
//{
//	/// <summary>
//	/// Represents a manager of a pointer that points to a segment in a managed heap.
//	/// </summary>
//	public sealed class PointerManager
//	{
//		internal readonly HeapStorage _heapManager;
//		internal readonly int _index;
//		internal Pointer _pointer;
//		internal bool _freed;

//		internal PointerManager(HeapStorage heapManager, int index, Pointer pointer)
//		{
//			_heapManager = heapManager;
//			_index = index;
//			_pointer = pointer;
//		}

//		/// <summary>
//		/// Releases all resources used by the <see cref="PointerManager"/>.
//		/// </summary>
//		~PointerManager() => _heapManager.ReleasePointerManager(this);

//		/// <summary>
//		/// Gets the <see cref="ManagedHeap.HeapStorage"/> of the <see cref="PointerManager"/>.
//		/// </summary>
//		public HeapStorage HeapManager => _heapManager;
//		/// <summary>
//		/// Gets the value meaning whether the pointer does not point to a segment.
//		/// </summary>
//		public bool IsNull => _index == HeapStorage._nullPointerIndex;
//		/// <summary>
//		/// Gets the index of the pointer.
//		/// </summary>
//		public int Index => _index;
//		/// <summary>
//		/// Gets the size of the segment.
//		/// </summary>
//		/// <exception cref="ArgumentException">The pointer does not point to a segment.</exception>
//		public long Size
//		{
//			get
//			{
//				if (IsNull)
//					throw new ArgumentException("The pointer does not point to a segment.");
//				return _pointer._size;
//			}
//		}
//		/// <summary>
//		/// Gets the value meaning whether the segment of the pointer is freed.
//		/// </summary>
//		/// <exception cref="ArgumentException">The pointer does not point to a segment.</exception>
//		public bool Freed
//		{
//			get
//			{
//				if (IsNull)
//					throw new ArgumentException("The pointer does not point to a segment.");
//				return _freed;
//			}
//		}

//		/// <summary>
//		/// Frees a segment of the pointer of the <see cref="PointerManager"/>.
//		/// </summary>
//		/// <exception cref="ArgumentException">The pointer does not point to a segment or the segment of the pointer is freed.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="ManagedHeap.HeapStorage"/> is disposed.</exception>
//		public void Free()
//		{
//			HeapStorage heapManager = _heapManager;
//			if (heapManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (IsNull)
//				throw new ArgumentException("The pointer does not point to a segment.");
//			if (_freed)
//				throw new ArgumentException("The segment of the pointer is freed.");
//			if (this == heapManager.StartPointerManager)
//				heapManager.StartPointerManager = heapManager.NullPointerManager;
//			lock (heapManager._pointerManagerLock)
//				_ = heapManager._pointerManagers.Remove(_index);
//			lock (heapManager._readLock)
//				_freed = true;
//			heapManager._segmentManager.Free(_pointer);
//			heapManager.WritePointer(_index, new Pointer(heapManager._meta._pointerListFreeIndex, 0));
//			heapManager._meta._pointerListFreeIndex = _index;
//			heapManager._meta._pointerListCount--;
//			heapManager.WriteMeta();
//		}
//		/// <summary>
//		/// Writes bytes to the heap.
//		/// </summary>
//		/// <param name="buffer">The buffer to contain the bytes.</param>
//		/// <param name="index">The index at which the writing from the buffer begins.</param>
//		/// <param name="count">The number of the bytes.</param>
//		/// <param name="offset">The zero-based offset in the segment at which the bytes are to be written.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/> or <paramref name="count"/> is less than 0 or <paramref name="offset"/> is less than 0 or greater than the segment size.</exception>
//		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/> or <paramref name="count"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the segment or the pointer does not point to a segment or the segment of the pointer is freed.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="ManagedHeap.HeapStorage"/> is disposed.</exception>
//		public void Write(byte[] buffer, int index, int count, long offset)
//		{
//			if (_heapManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (buffer == null)
//				throw new ArgumentNullException(nameof(buffer));
//			if (index < 0 || index > buffer.Length)
//				throw new ArgumentOutOfRangeException(nameof(index));
//			if (count < 0)
//				throw new ArgumentOutOfRangeException(nameof(count));
//			if (offset < 0 || offset > _pointer._size)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (count > buffer.Length - index)
//				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from the {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
//			if (count > _pointer._size - offset)
//				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from the {1} to the end of the segment.", nameof(count), nameof(offset)));
//			if (IsNull)
//				throw new ArgumentException("The pointer does not point to a segment.");
//			if (_freed)
//				throw new ArgumentException("The segment of the pointer is freed.");
//			_heapManager._storage.Write(buffer, index, count, _pointer._address + offset);
//		}
//		/// <summary>
//		/// Converts the specified value to bytes and writes the bytes to the heap.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="value">The value.</param>
//		/// <param name="serializer">The <see cref="IConstantLengthSerializer{T}"/> to convert the value to the bytes.</param>
//		/// <param name="offset">The zero-based offset in the segment at which the bytes are to be written.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the segment size.</exception>
//		/// <exception cref="ArgumentException">The number of bytes of <paramref name="value"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the segment or the pointer does not point to a segment or the segment of the pointer is freed.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="ManagedHeap.HeapStorage"/> is disposed.</exception>
//		public void Write<T>(T value, IConstantLengthSerializer<T> serializer, long offset)
//		{
//			if (_heapManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (serializer == null)
//				throw new ArgumentNullException(nameof(serializer));
//			if (offset < 0 || offset > _pointer._size)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (serializer.Count > _pointer._size - offset)
//				throw new ArgumentException(string.Format("The number of bytes of {0} is greater than the number of bytes from the {1} to the end of the segment.", nameof(value), nameof(offset)));
//			if (IsNull)
//				throw new ArgumentException("The pointer does not point to a segment.");
//			if (_freed)
//				throw new ArgumentException("The segment of the pointer is freed.");
//			_heapManager._storage.Write(value, serializer, _pointer._address + offset);
//		}
//		/// <summary>
//		/// Converts the specified value to bytes and writes the bytes to the heap.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="value">The value.</param>
//		/// <param name="bitConverter">The <see cref="ISerializer{T}"/> to convert the value to the bytes.</param>
//		/// <param name="offset">The zero-based offset in the segment at which the bytes are to be written.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="bitConverter"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the segment size.</exception>
//		/// <exception cref="ArgumentException">The number of bytes of <paramref name="value"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the segment or the pointer does not point to a segment or the segment of the pointer is freed.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="ManagedHeap.HeapStorage"/> is disposed.</exception>
//		public void Write<T>(T value, ISerializer<T> bitConverter, long offset)
//		{
//			if (_heapManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (bitConverter == null)
//				throw new ArgumentNullException(nameof(bitConverter));
//			if (offset < 0 || offset > _pointer._size)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (bitConverter.Count(value) > _pointer._size - offset)
//				throw new ArgumentException(string.Format("The number of bytes of {0} is greater than the number of bytes from the {1} to the end of the segment.", nameof(value), nameof(offset)));
//			if (IsNull)
//				throw new ArgumentException("The pointer does not point to a segment.");
//			if (_freed)
//				throw new ArgumentException("The segment of the pointer is freed.");
//			_heapManager._storage.Write(value, bitConverter, _pointer._address + offset);
//		}
//		/// <summary>
//		/// Reads bytes from the heap.
//		/// </summary>
//		/// <param name="offset">The zero-based offset in the segment at which the bytes are to be read.</param>
//		/// <param name="count">The number of the bytes.</param>
//		/// <param name="buffer">The buffer to receive the bytes.</param>
//		/// <param name="index">The index at which the reading to the buffer begins.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/> or <paramref name="count"/> is less than 0 or <paramref name="offset"/> is less than 0 or greater than the segment size.</exception>
//		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/> or <paramref name="count"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the segment or the pointer does not point to a segment or the segment of the pointer is freed.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="ManagedHeap.HeapStorage"/> is disposed.</exception>
//		public void Read(long offset, int count, byte[] buffer, int index)
//		{
//			if (_heapManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (buffer == null)
//				throw new ArgumentNullException(nameof(buffer));
//			if (index < 0 || index > buffer.Length)
//				throw new ArgumentOutOfRangeException(nameof(index));
//			if (count < 0)
//				throw new ArgumentOutOfRangeException(nameof(count));
//			if (offset < 0 || offset > _pointer._size)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (count > buffer.Length - index)
//				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
//			if (offset < 0 || offset > _pointer._size)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (count > _pointer._size - offset)
//				throw new ArgumentException(string.Format("The number of bytes of the value is greater than the number of bytes from the {0} to the end of the segment.", nameof(offset)));
//			if (IsNull)
//				throw new ArgumentException("The pointer does not point to a segment.");
//			lock (_heapManager._readLock)
//			{
//				if (_freed)
//					throw new ArgumentException("The segment of the pointer is freed.");
//				_heapManager._storage.Read(_pointer._address + offset, count, buffer, index);
//			}
//		}
//		/// <summary>
//		/// Reads bytes from the heap and converts it to a value.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="offset">The zero-based offset in the segment at which the bytes are to be read.</param>
//		/// <param name="bitConverter">The <see cref="IConstantLengthSerializer{T}"/> to convert read bytes to the value.</param>
//		/// <returns>A read value.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="bitConverter"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the segment size.</exception>
//		/// <exception cref="ArgumentException">The number of bytes of the value is greater than the number of bytes from <paramref name="offset"/> to the end of the segment or the pointer does not point to a segment or the segment of the pointer is freed.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="ManagedHeap.HeapStorage"/> is disposed.</exception>
//		public T Read<T>(long offset, IConstantLengthSerializer<T> bitConverter)
//		{
//			if (_heapManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (bitConverter == null)
//				throw new ArgumentNullException(nameof(bitConverter));
//			if (offset < 0 || offset > _pointer._size)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (bitConverter.Count > _pointer._size - offset)
//				throw new ArgumentException(string.Format("The number of bytes of the value is greater than the number of bytes from the {0} to the end of the segment.", nameof(offset)));
//			if (IsNull)
//				throw new ArgumentException("The pointer does not point to a segment.");
//			lock (_heapManager._readLock)
//			{
//				if (_freed)
//					throw new ArgumentException("The segment of the pointer is freed.");
//				return _heapManager._storage.Read(_pointer._address + offset, bitConverter);
//			}
//		}
//		/// <summary>
//		/// Reads bytes from the heap and converts it to a value.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="offset">The zero-based offset in the segment at which the bytes are to be read.</param>
//		/// <param name="count">The number of the bytes.</param>
//		/// <param name="serializer">The <see cref="ISerializer{T}"/> to convert read bytes to the value.</param>
//		/// <returns>A read value.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the segment size or <paramref name="count"/> is less than 0 or greater than the segment size.</exception>
//		/// <exception cref="ArgumentException">The number of bytes of the value is greater than the number of bytes from <paramref name="offset"/> to the end of the segment or the pointer does not point to a segment or the segment of the pointer is freed.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="ManagedHeap.HeapStorage"/> is disposed.</exception>
//		public T Read<T>(long offset, int count, ISerializer<T> serializer)
//		{
//			if (_heapManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (serializer == null)
//				throw new ArgumentNullException(nameof(serializer));
//			if (offset < 0 || offset > _pointer._size)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (count < 0 || count > _pointer._size)
//				throw new ArgumentOutOfRangeException(nameof(count));
//			if (count > _pointer._size - offset)
//				throw new ArgumentException(string.Format("The number of bytes of the value is greater than the number of bytes from the {0} to the end of the segment.", nameof(offset)));
//			if (IsNull)
//				throw new ArgumentException("The pointer does not point to a segment.");
//			lock (_heapManager._readLock)
//			{
//				if (_freed)
//					throw new ArgumentException("The segment of the pointer is freed.");
//				return _heapManager._storage.Read(_pointer._address + offset, count, serializer);
//			}
//		}
//	}
//}