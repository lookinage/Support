//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics.Contracts;
//using System.Threading;
//using Noname.BitConversion;
//using Noname.BitConversion.System;
//using Noname.IO.ManagedHeap;

//namespace Noname.IO.ObjectOrientedDomain
//{
//	/// <summary>
//	/// Represents a manager of an array in an object-oriented domain.
//	/// </summary>
//	public sealed class ArrayManager : IList<ValueManager>
//	{
//		internal struct Meta
//		{
//			internal sealed class BitConverter : ConstantLengthBitConverter<Meta>
//			{
//				static internal readonly BitConverter _instance;

//				static BitConverter() => _instance = new BitConverter();

//				internal BitConverter() : base(Int32BitConverterBuilder.Instance.ByteCount + Int32BitConverterBuilder.Instance.ByteCount + Int32BitConverterBuilder.Instance.ByteCount + Int32BitConverterBuilder.Instance.ByteCount + Int32BitConverterBuilder.Instance.ByteCount + Int32BitConverterBuilder.Instance.ByteCount + Int32BitConverterBuilder.Instance.ByteCount) { }

//				public override sealed void GetBytes(Meta instance, byte[] bytes, int index)
//				{
//					Int32BitConverterBuilder.Instance.GetBytes(instance._dataSize, bytes, ref index);
//					Int32BitConverterBuilder.Instance.GetBytes(instance._pointerCount, bytes, ref index);
//					Int32BitConverterBuilder.Instance.GetBytes(instance._parentCapacity, bytes, ref index);
//					Int32BitConverterBuilder.Instance.GetBytes(instance._parentCount, bytes, ref index);
//					Int32BitConverterBuilder.Instance.GetBytes(instance._parentUsedCount, bytes, ref index);
//					Int32BitConverterBuilder.Instance.GetBytes(instance._parentFreeIndex, bytes, ref index);
//					Int32BitConverterBuilder.Instance.GetBytes(instance._parentPointerIndex, bytes, ref index);
//				}
//				public override sealed Meta GetInstance(byte[] bytes, int index) => new Meta(Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index), Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index), Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index), Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index), Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index), Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index), Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index));
//			}

//			internal readonly int _dataSize;
//			internal readonly int _pointerCount;
//			internal int _parentCapacity;
//			internal int _parentCount;
//			internal int _parentUsedCount;
//			internal int _parentFreeIndex;
//			internal int _parentPointerIndex;
			
//			public Meta(int dataSize, int pointerCount, int parentCapacity, int parentCount, int parentUsedCount, int parentFreeIndex, int parentPointerIndex)
//			{
//				Contract.Assert(dataSize >= 0 && dataSize <= 1073741823 && pointerCount >= 0 && pointerCount <= 134217728);
//				_dataSize = dataSize;
//				_pointerCount = pointerCount;
//				_parentCapacity = parentCapacity;
//				_parentCount = parentCount;
//				_parentUsedCount = parentUsedCount;
//				_parentFreeIndex = parentFreeIndex;
//				_parentPointerIndex = parentPointerIndex;
//			}
//			internal Meta(int dataSiteSize, int pointerCount) : this(dataSiteSize, pointerCount, -1, -1, -1, -1, -1) { }

//			internal int ValueSize => _dataSize + Int32BitConverterBuilder.Instance.ByteCount * _pointerCount;
//		}

//		private readonly HashSet<ArrayManager> _addedParents;
//		private readonly HashSet<ArrayManager> _removedParents;
//		private readonly object _valueLock;
//		private readonly Dictionary<int, WeakReference<ValueManager>> _valueManagers;
//		private readonly HashSet<ValueManager> _dirtyValueManagers;
//		internal readonly DomainManager _domainManager;
//		internal Meta _meta;
//		internal PointerManager _pointerManager;

//		internal ArrayManager(DomainManager domainManager, Meta meta)
//		{
//			_domainManager = domainManager;
//			_meta = meta;
//			_addedParents = new HashSet<ArrayManager>();
//			_removedParents = new HashSet<ArrayManager>();
//			_valueLock = new object();
//			_valueManagers = new Dictionary<int, WeakReference<ValueManager>>();
//			_dirtyValueManagers = new HashSet<ValueManager>();
//		}
//		internal ArrayManager(DomainManager domainManager, PointerManager pointerManager) : this(domainManager, pointerManager.Read(0, Meta.BitConverter._instance)) => _pointerManager = pointerManager;

//		/// <summary>
//		/// Releases all resources used by the <see cref="ArrayManager"/>.
//		/// </summary>
//		~ArrayManager() => _domainManager.ReleaseArrayManager(this);

//		/// <summary>
//		/// Gets the <see cref="ObjectOrientedDomain.DomainManager"/> of the <see cref="ArrayManager"/>.
//		/// </summary>
//		public DomainManager DomainManager => _domainManager;
//		/// <summary>
//		/// Gets the value meaning whether the array is null.
//		/// </summary>
//		public bool IsNull => _pointerManager.Index == DomainManager._nullObjectPointerIndex;
//		/// <summary>
//		/// Gets the value meaning whether the array is lost.
//		/// </summary>
//		/// <exception cref="ArgumentException">The <see cref="ArrayManager"/> is of the null array.</exception>
//		public bool Lost
//		{
//			get
//			{
//				if (IsNull)
//					throw new ArgumentException(string.Format("The {0} is of the null array.", nameof(ArrayManager)));
//				return _pointerManager.Freed;
//			}
//		}
//		/// <summary>
//		/// Gets the size of the data of values of elements of the array.
//		/// </summary>
//		/// <exception cref="ArgumentException">The <see cref="ArrayManager"/> is of the null array.</exception>
//		public int DataSize
//		{
//			get
//			{
//				if (IsNull)
//					throw new ArgumentException(string.Format("The {0} is of the null array.", nameof(ArrayManager)));
//				return _meta._dataSize;
//			}
//		}
//		/// <summary>
//		/// Gets the number of pointers of values of elements of the array.
//		/// </summary>
//		/// <exception cref="ArgumentException">The <see cref="ArrayManager"/> is of the null array.</exception>
//		public int PointerCount
//		{
//			get
//			{
//				if (IsNull)
//					throw new ArgumentException(string.Format("The {0} is of the null array.", nameof(ArrayManager)));
//				return _meta._pointerCount;
//			}
//		}
//		/// <summary>
//		/// Gets the number of elements in the array.
//		/// </summary>
//		/// <exception cref="ArgumentException">The <see cref="ArrayManager"/> is of the null array.</exception>
//		public int Length
//		{
//			get
//			{
//				if (IsNull)
//					throw new ArgumentException(string.Format("The {0} is of the null array.", nameof(ArrayManager)));
//				int valueSize = _meta.ValueSize;
//				return valueSize == 0 ? 0 : (int)((_pointerManager.Size - Meta.BitConverter._instance.ByteCount) / valueSize);
//			}
//		}
//		/// <summary>
//		/// Gets the number of references to the array.
//		/// </summary>
//		/// <exception cref="ArgumentException">The <see cref="ArrayManager"/> is of the null array.</exception>
//		public int ReferenceCount
//		{
//			get
//			{
//				if (IsNull)
//					throw new ArgumentException(string.Format("The {0} is of the null array.", nameof(ArrayManager)));
//				return _meta._parentCount;
//			}
//		}
//		/// <summary>
//		/// Gets an <see cref="ValueManager"/> of a value of an element at the specified index.
//		/// </summary>
//		/// <param name="index">The zero-based index of an element to get or set.</param>
//		/// <returns>An <see cref="ValueManager"/>  of a value of an element at the specified index.</returns>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the array.</exception>
//		/// <exception cref="ArgumentException">The <see cref="ArrayManager"/> is of the null array.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="ObjectOrientedDomain.DomainManager"/> is disposed.</exception>
//		public ValueManager this[int index]
//		{
//			get
//			{
//				if (_domainManager.Disposed)
//					throw new ObjectDisposedException(string.Empty);
//				if (IsNull)
//					throw new ArgumentException(string.Format("The {0} is of the null array.", nameof(ArrayManager)));
//				if (index < 0 || index >= Length)
//					throw new ArgumentOutOfRangeException(nameof(index));
//				Find:
//				bool wait = false;
//				Monitor.Enter(_valueLock);
//				try
//				{
//					ValueManager valueManager;
//					if (_valueManagers.TryGetValue(index, out WeakReference<ValueManager> reference))
//					{
//						if (reference.TryGetTarget(out valueManager))
//							return valueManager;
//						wait = true;
//						goto Find;
//					}
//					valueManager = new ValueManager(this, index);
//					_valueManagers.Add(index, new WeakReference<ValueManager>(valueManager));
//					return valueManager;
//				}
//				finally
//				{
//					Monitor.Exit(_valueLock);
//					if (wait)
//						GC.WaitForPendingFinalizers();
//				}
//			}
//		}

//		int ICollection<ValueManager>.Count => Length;
//		bool ICollection<ValueManager>.IsReadOnly => false;

//		ValueManager IList<ValueManager>.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

//		internal void ReleaseValueManager(ValueManager valueManager)
//		{
//			lock (_valueLock)
//				_valueManagers.Remove(valueManager._index);
//		}
//		internal void SetDirty()=>
//		internal void Free()
//		{
//			_domainManager.Release(this, 0, Length);
//			_pointerManager.Free();
//		}
//		internal void Lease()
//		{
//			checked { _meta._parentCount++; }
//			_pointerManager.Write(_meta, Meta.BitConverter._instance, 0);
//		}
//		internal void Release()
//		{
//			_meta._parentCount--;
//			_pointerManager.Write(_meta, Meta.BitConverter._instance, 0);
//		}
//		internal void ChangeReferenceCount(int referenceCount)
//		{
//			_meta._parentCount = referenceCount;
//			_pointerManager.Write(_meta, Meta.BitConverter._instance, 0);
//		}
//		internal void Commit()
//		{

//		}
//		/// <summary>
//		/// Returns an enumerator that iterates through the array.
//		/// </summary>
//		/// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the array.</returns>
//		public IEnumerator<ValueManager> GetEnumerator()
//		{
//			int length = Length;
//			for (int index = 0; index < length; )
//			{
//				int readLength = _domainManager._heapManager.GetReadLength(_meta.ValueSize, length - index);
//				ValueManager[] valueManagers = new ValueManager[readLength];
//				CopyTo(valueManagers, 0, index, readLength);
//				for (int localFloor = index, localRoof = localFloor + readLength; index < localRoof; index++)
//					yield return valueManagers[index - localFloor];
//			}
//		}
//		/// <summary>
//		/// Copies managers of values of the elements of the array to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
//		/// </summary>
//		/// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from the array.</param>
//		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
//		/// <param name="index">The index of the start copying element.</param>
//		/// <param name="length">The number of elements to copy.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is not a valid index of <paramref name="array"/> or <paramref name="index"/> is not a valid index of the source array.</exception>
//		/// <exception cref="ArgumentException"><paramref name="length"/> is greater than the number of elements from <paramref name="index"/> to the end of the source array or <paramref name="length"/> is greater than the number of elements from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
//		public void CopyTo(ValueManager[] array, int arrayIndex, int index, int length)
//		{
//			if (array == null)
//				throw new ArgumentNullException(nameof(array));
//			if (arrayIndex < 0 || arrayIndex > array.Length)
//				throw new ArgumentOutOfRangeException(nameof(arrayIndex));
//			if (index < 0 || index > Length)
//				throw new ArgumentOutOfRangeException(nameof(index));
//			if (length < 0)
//				throw new ArgumentOutOfRangeException(nameof(length));
//			if (length > Length - index)
//				throw new ArgumentException(string.Format("{0} is greater than the number of elements from {1} to the end of the source array.", nameof(length), nameof(index)));
//			if (length > array.Length - arrayIndex)
//				throw new ArgumentException(string.Format("{0} is greater than the number of elements from {1} to the end of the destination array.", nameof(length), nameof(arrayIndex)));
//			int valueSize = _meta.ValueSize;
//			byte[] buffer = null;
//			for (int roof = index + length; index < roof;)
//			{
//				int readLength = _domainManager._heapManager.GetReadLength(valueSize, roof - index);
//				int readSize = valueSize * readLength;
//				ArrayHelper.CheckSize(ref buffer, readSize);
//				_pointerManager.Read(Meta.BitConverter._instance.ByteCount + valueSize * (long)index, readSize, buffer, 0);
//				int localFloor = index;
//				int localRoof = localFloor + readLength;
//			Find:
//				bool wait = false;
//				Monitor.Enter(_valueLock);
//				try
//				{
//					for (; index < localRoof; index++)
//					{
//						if (_valueManagers.TryGetValue(index, out WeakReference<ValueManager> reference))
//						{
//							if (reference.TryGetTarget(out ValueManager valueManager))
//							{
//								array[arrayIndex + index] = valueManager;
//								continue;
//							}
//							wait = true;
//							goto Find;
//						}
//						_valueManagers.Add(index, new WeakReference<ValueManager>(array[arrayIndex + index] = new ValueManager(this, index, buffer, index - localFloor)));
//					}
//				}
//				finally
//				{
//					Monitor.Exit(_valueLock);
//					if (wait)
//						GC.WaitForPendingFinalizers();
//				}
//			}
//		}
//		/// <summary>
//		/// Copies managers of values of the elements of the array to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
//		/// </summary>
//		/// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from the array.</param>
//		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is not a valid index of <paramref name="array"/>.</exception>
//		/// <exception cref="ArgumentException">The number of elements in the source array is greater than the available space from the <paramref name="arrayIndex"/> to the end of the destination array.</exception>
//		public void CopyTo(ValueManager[] array, int arrayIndex) => CopyTo(array, arrayIndex, 0, Length);

//		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//		void ICollection<ValueManager>.Add(ValueManager item) => throw new NotSupportedException();
//		bool ICollection<ValueManager>.Remove(ValueManager item) => throw new NotSupportedException();
//		void ICollection<ValueManager>.Clear() => throw new NotSupportedException();
//		bool ICollection<ValueManager>.Contains(ValueManager item) => throw new NotSupportedException();

//		int IList<ValueManager>.IndexOf(ValueManager item) => throw new NotSupportedException();
//		void IList<ValueManager>.Insert(int index, ValueManager item) => throw new NotSupportedException();
//		void IList<ValueManager>.RemoveAt(int index) => throw new NotSupportedException();
//	}
//}