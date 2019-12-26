//using System;
//using Noname.BitConversion;
//using Noname.BitConversion.System;

//namespace Noname.IO.ObjectOrientedDomain
//{
//	/// <summary>
//	/// Represents a manager of a value of an element of an array in an object-oriented domain.
//	/// </summary>
//	public sealed class ValueManager : IValueManager
//	{
//		internal readonly ArrayManager _arrayManager;
//		internal readonly int _index;
//		internal byte[] _data;

//		internal ValueManager(ArrayManager arrayManager, int index)
//		{
//			_arrayManager = arrayManager;
//			_index = index;
//		}
//		internal ValueManager(ArrayManager arrayManager, int index, byte[] buffer, int bufferElementIndex) : this(arrayManager, index)
//		{
//			int valueSize = arrayManager._meta.ValueSize;
//			_data = new byte[valueSize];
//			Array.Copy(buffer, valueSize * bufferElementIndex, _data, 0, valueSize);
//		}

//		/// <summary>
//		/// Releases all resources used by the <see cref="ValueManager"/>.
//		/// </summary>
//		~ValueManager() => _arrayManager.ReleaseValueManager(this);

//		/// <summary>
//		/// Gets the <see cref="ObjectOrientedDomain.ArrayManager"/> of the <see cref="ValueManager"/>.
//		/// </summary>
//		public ArrayManager ArrayManager => _arrayManager;
//		/// <summary>
//		/// Gets the index of the element in the array.
//		/// </summary>
//		public int Index => _index;
//		/// <summary>
//		/// Gets or sets a reference to an array at the specified index.
//		/// </summary>
//		/// <param name="index">The zero-based index of the <see cref="ObjectOrientedDomain.ArrayManager"/> of the array to get or set.</param>
//		/// <returns>The <see cref="ObjectOrientedDomain.ArrayManager"/> of the array at the specified index.</returns>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of the pointers of the element.</exception>
//		/// <exception cref="ArgumentNullException">The specified <see cref="ObjectOrientedDomain.ArrayManager"/> is null.</exception>
//		/// <exception cref="ArgumentException">The array is lost or the specified <see cref="ObjectOrientedDomain.ArrayManager"/> is of another <see cref="DomainManager"/> or the array of the specified <see cref="ObjectOrientedDomain.ArrayManager"/> is lost.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public ArrayManager this[int index]
//		{
//			get
//			{
//				ArrayManager arrayManager = _arrayManager;
//				if (index < 0 || index >= arrayManager._meta._pointerCount)
//					throw new ArgumentOutOfRangeException(nameof(index));
//				if (arrayManager.Lost)
//					throw new ArgumentException("The array is lost.");
//				DomainManager domainManager = arrayManager.DomainManager;
//				if (domainManager.Disposed)
//					throw new ObjectDisposedException(string.Empty);
//				Check();
//				return domainManager.GetArrayManager(domainManager._heapManager.Get(Int32BitConverterBuilder.Instance.GetInstance(_data, arrayManager._meta._dataSize + Int32BitConverterBuilder.Instance.ByteCount * index)));
//			}
//			set
//			{
//				ArrayManager arrayManager = _arrayManager;
//				if (index < 0 || index >= arrayManager._meta._pointerCount)
//					throw new ArgumentOutOfRangeException(nameof(index));
//				if (value == null)
//					throw new ArgumentNullException(nameof(value));
//				if (arrayManager.Lost)
//					throw new ArgumentException("The array is lost.");
//				DomainManager domainManager = arrayManager.DomainManager;
//				if (domainManager.Disposed)
//					throw new ObjectDisposedException(string.Empty);
//				if (value._domainManager != arrayManager._domainManager)
//					throw new ArgumentException(string.Format("The specified {0} is of another {1}.", nameof(ObjectOrientedDomain.ArrayManager), nameof(DomainManager)));
//				if (value.Lost)
//					throw new ArgumentException(string.Format("The array of the specified {0} is lost.", nameof(ObjectOrientedDomain.ArrayManager)));
//				Check();
//				ArrayManager target = domainManager.GetArrayManager(domainManager._heapManager.Get(Int32BitConverterBuilder.Instance.GetInstance(_data, arrayManager._meta._dataSize + Int32BitConverterBuilder.Instance.ByteCount * index)));
//				if (value == target)
//					return;
//				domainManager.Release(arrayManager, target);
//				domainManager.Lease(arrayManager, value);
//				Int32BitConverterBuilder.Instance.GetBytes(value._pointerManager.Index, _data, arrayManager._meta._dataSize + Int32BitConverterBuilder.Instance.ByteCount * index);
//				Commit();
//			}
//		}

//		private void Check()
//		{
//			if (_data != null)
//				return;
//			int valueSize = _arrayManager._meta.ValueSize;
//			_arrayManager._pointerManager.Read(ArrayManager.Meta.BitConverter._instance.ByteCount + valueSize * (long)_index, valueSize, _data = new byte[valueSize], 0);
//		}
//		private void Commit()
//		{
//			int valueSize = _arrayManager._meta.ValueSize;
//			_arrayManager._pointerManager.Write(_data, 0, valueSize, ArrayManager.Meta.BitConverter._instance.ByteCount + valueSize * (long)_index);
//		}
//		/// <summary>
//		/// Writes bytes to the data of the value.
//		/// </summary>
//		/// <param name="buffer">The buffer to contain the bytes.</param>
//		/// <param name="index">The index at which the writing from the buffer begins.</param>
//		/// <param name="count">The number of the bytes.</param>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be written.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/> or <paramref name="count"/> is less than 0 or <paramref name="offset"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The array is lost or <paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/> or <paramref name="count"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public void Write(byte[] buffer, int index, int count, int offset)
//		{
//			ArrayManager arrayManager = _arrayManager;
//			if (arrayManager._domainManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (arrayManager.Lost)
//				throw new ArgumentException("The array is lost.");
//			if (buffer == null)
//				throw new ArgumentNullException(nameof(buffer));
//			if (index < 0 || index > buffer.Length)
//				throw new ArgumentOutOfRangeException(nameof(index));
//			if (count < 0)
//				throw new ArgumentOutOfRangeException(nameof(count));
//			if (offset < 0 || offset > arrayManager._meta._dataSize)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (count > buffer.Length - index)
//				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from the {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
//			if (count > arrayManager._meta._dataSize - offset)
//				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from the {1} to the end of the data.", nameof(count), nameof(offset)));
//			Check();
//			Array.Copy(buffer, index, _data, offset, count);
//			Commit();
//		}
//		/// <summary>
//		/// Converts the specified value to bytes and writes the bytes to the data of the value.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="value">The value.</param>
//		/// <param name="bitConverter">The <see cref="ConstantLengthBitConverter{T}"/> to convert the value to the bytes.</param>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be written.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="bitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The array is lost or the number of bytes of <paramref name="value"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public void Write<T>(T value, ConstantLengthBitConverter<T> bitConverter, int offset)
//		{
//			ArrayManager arrayManager = _arrayManager;
//			if (arrayManager._domainManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (arrayManager.Lost)
//				throw new ArgumentException("The array is lost.");
//			if (bitConverter == null)
//				throw new ArgumentNullException(nameof(bitConverter));
//			if (offset < 0 || offset > arrayManager._meta._dataSize)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (bitConverter.ByteCount > arrayManager._meta._dataSize - offset)
//				throw new ArgumentException(string.Format("The number of bytes of {0} is greater than the number of bytes from the {1} to the end of the data.", nameof(value), nameof(offset)));
//			Check();
//			bitConverter.GetBytes(value, _data, offset);
//			Commit();
//		}
//		/// <summary>
//		/// Converts the specified value to bytes and writes the bytes to the data of the value.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="value">The value.</param>
//		/// <param name="bitConverter">The <see cref="VariableLengthBitConverter{T}"/> to convert the value to the bytes.</param>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be written.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="bitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The array is lost or the number of bytes of <paramref name="value"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public void Write<T>(T value, VariableLengthBitConverter<T> bitConverter, int offset)
//		{
//			ArrayManager arrayManager = _arrayManager;
//			if (arrayManager._domainManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (arrayManager.Lost)
//				throw new ArgumentException("The array is lost.");
//			if (bitConverter == null)
//				throw new ArgumentNullException(nameof(bitConverter));
//			if (offset < 0 || offset > arrayManager._meta._dataSize)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (bitConverter.GetByteCount(value) > arrayManager._meta._dataSize - offset)
//				throw new ArgumentException(string.Format("The number of bytes of {0} is greater than the number of bytes from the {1} to the end of the data.", nameof(value), nameof(offset)));
//			Check();
//			bitConverter.GetBytes(value, _data, offset);
//			Commit();
//		}
//		/// <summary>
//		/// Reads bytes from the data of the value.
//		/// </summary>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be read.</param>
//		/// <param name="count">The number of the bytes.</param>
//		/// <param name="buffer">The buffer to receive the bytes.</param>
//		/// <param name="index">The index at which the reading to the buffer begins.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/> or <paramref name="count"/> is less than 0 or <paramref name="offset"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The array is lost or <paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/> or <paramref name="count"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public void Read(int offset, int count, byte[] buffer, int index)
//		{
//			ArrayManager arrayManager = _arrayManager;
//			if (arrayManager._domainManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (arrayManager.Lost)
//				throw new ArgumentException("The array is lost.");
//			if (buffer == null)
//				throw new ArgumentNullException(nameof(buffer));
//			if (index < 0 || index > buffer.Length)
//				throw new ArgumentOutOfRangeException(nameof(index));
//			if (count < 0)
//				throw new ArgumentOutOfRangeException(nameof(count));
//			if (offset < 0 || offset > arrayManager._meta._dataSize)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (count > buffer.Length - index)
//				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
//			if (offset < 0 || offset > arrayManager._meta._dataSize)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (count > arrayManager._meta._dataSize - offset)
//				throw new ArgumentException(string.Format("The number of bytes of the value is greater than the number of bytes from the {0} to the end of the data.", nameof(offset)));
//			Check();
//			Array.Copy(_data, offset, buffer, index, count);
//		}
//		/// <summary>
//		/// Reads bytes from the data of the value and converts it to a value.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be read.</param>
//		/// <param name="bitConverter">The <see cref="ConstantLengthBitConverter{T}"/> to convert read bytes to the value.</param>
//		/// <returns>A read value.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="bitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The number of bytes of the value is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public T Read<T>(int offset, ConstantLengthBitConverter<T> bitConverter)
//		{
//			ArrayManager arrayManager = _arrayManager;
//			if (arrayManager._domainManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (arrayManager.Lost)
//				throw new ArgumentException("The array is lost.");
//			if (bitConverter == null)
//				throw new ArgumentNullException(nameof(bitConverter));
//			if (offset < 0 || offset > arrayManager._meta._dataSize)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (bitConverter.ByteCount > arrayManager._meta._dataSize)
//				throw new ArgumentException(string.Format("The number of bytes of the value is greater than the number of bytes from the {0} to the end of the data.", nameof(offset)));
//			Check();
//			return bitConverter.GetInstance(_data, offset);
//		}
//		/// <summary>
//		/// Reads bytes from the data of the value and converts it to a value.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be read.</param>
//		/// <param name="count">The number of the bytes.</param>
//		/// <param name="bitConverter">The <see cref="ConstantLengthBitConverter{T}"/> to convert read bytes to the value.</param>
//		/// <returns>A read value.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="bitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the size of the data or <paramref name="count"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The number of bytes of the value is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public T Read<T>(int offset, int count, VariableLengthBitConverter<T> bitConverter)
//		{
//			ArrayManager arrayManager = _arrayManager;
//			if (arrayManager._domainManager.Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (arrayManager.Lost)
//				throw new ArgumentException("The array is lost.");
//			if (bitConverter == null)
//				throw new ArgumentNullException(nameof(bitConverter));
//			if (offset < 0 || offset > arrayManager._meta._dataSize)
//				throw new ArgumentOutOfRangeException(nameof(offset));
//			if (count < 0 || count > arrayManager._meta._dataSize)
//				throw new ArgumentOutOfRangeException(nameof(count));
//			if (count > arrayManager._meta._dataSize)
//				throw new ArgumentException(string.Format("The number of bytes of the value is greater than the number of bytes from the {0} to the end of the data.", nameof(offset)));
//			Check();
//			return bitConverter.GetInstance(_data, offset, count);
//		}
//	}
//}