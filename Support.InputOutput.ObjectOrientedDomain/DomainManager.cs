//using System;
//using System.Collections.Generic;
//using System.Diagnostics.Contracts;
//using System.Threading;
//using Noname.BitConversion.System;
//using Noname.IO.ManagedHeap;

//namespace Noname.IO.ObjectOrientedDomain
//{
//	/// <summary>
//	/// Represents a manager of an object-oriented domain.
//	/// </summary>
//	public class DomainManager : IDisposable
//	{
//		internal sealed class SuspectArrayCase
//		{
//			internal readonly ArrayManager _arrayManager;
//			internal int _externalReferenceCount;
//			internal bool _handled;

//			internal SuspectArrayCase(ArrayManager arrayManager)
//			{
//				_arrayManager = arrayManager;
//				_externalReferenceCount = arrayManager._meta._parentCount;
//			}
//		}

//		internal const int _nullObjectPointerIndex = 1;

//		private byte[] _initialElementBuffer;
//		private bool _committing;
//		internal readonly HeapManager _heapManager;
//		internal readonly object _arrayManagerLock;
//		internal readonly Dictionary<PointerManager, WeakReference<ArrayManager>> _arrayManagers;
//		internal readonly HashSet<ArrayManager> _dirtyArrayManagers;
//		internal readonly ArrayManager _nullArrayManager;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="DomainManager"/> class.
//		/// </summary>
//		/// <param name="path">The path to the domain.</param>
//		/// <param name="create">If true the domain creates; otherwise, the <see cref="DomainManager"/> opens the domain.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
//		public DomainManager(string path, bool create)
//		{
//			_heapManager = new HeapManager(path, create);
//			_arrayManagerLock = new object();
//			_arrayManagers = new Dictionary<PointerManager, WeakReference<ArrayManager>>();
//			_dirtyArrayManagers = new HashSet<ArrayManager>();
//			_nullArrayManager = new ArrayManager(this, new ArrayManager.Meta(0, 0));
//			if (create)
//			{
//				_heapManager.StartPointerManager = CreateArray(0, 0, 0)._pointerManager;
//				_heapManager.Commit();
//			}
//		}

//		/// <summary>
//		/// Gets the number of free segments.
//		/// </summary>
//		public int FreeSegmentCount => _heapManager.FreeSegmentCount;
//		/// <summary>
//		/// Gets the number of allocated segments.
//		/// </summary>
//		public int AllocatedSegmentCount => _heapManager.AllocatedSegmentCount;
//		/// <summary>
//		/// Gets the total amount of bytes.
//		/// </summary>
//		public long TotalAmount => _heapManager.TotalAmount;
//		/// <summary>
//		/// Gets the amount of free bytes.
//		/// </summary>
//		public long FreeAmount => _heapManager.FreeAmount;
//		/// <summary>
//		/// Gets the amount of allocated bytes.
//		/// </summary>
//		public long AllocatedAmount => _heapManager.AllocatedAmount;
//		/// <summary>
//		/// Gets the <see cref="ArrayManager"/> of the null object.
//		/// </summary>
//		public ArrayManager NullArrayManager => GetArrayManager(_heapManager.Get(_nullObjectPointerIndex));
//		/// <summary>
//		/// Gets the <see cref="ArrayManager"/> of the object that starts the work.
//		/// </summary>
//		/// <exception cref="ArgumentNullException">The specified <see cref="ArrayManager"/> is null.</exception>
//		/// <exception cref="ArgumentException">The specified <see cref="ArrayManager"/> is of another <see cref="DomainManager"/> or the object is lost.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public ArrayManager StartArrayManager
//		{
//			get => GetArrayManager(_heapManager.StartPointerManager);
//			set
//			{
//				if (Disposed)
//					throw new ObjectDisposedException(string.Empty);
//				if (value == null)
//					throw new ArgumentNullException(nameof(value));
//				if (value._domainManager != this)
//					throw new ArgumentException(string.Format("The specified {0} is of another {1}.", nameof(PointerManager), nameof(DomainManager)));
//				if (value.Lost)
//					throw new ArgumentException("The object is lost.");
//				ArrayManager nullArrayManager = NullArrayManager;
//				Release(nullArrayManager, StartArrayManager);
//				_heapManager.StartPointerManager = value._pointerManager;
//				Lease(nullArrayManager, value);
//			}
//		}
//		/// <summary>
//		/// Gets or sets the bound of number of free segments of the heap for the auto defragmentation.
//		/// </summary>
//		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 1.</exception>
//		public int FreeSegmentCountDefragmentBound { get => _heapManager.FreeSegmentCountDefragmentBound; set => _heapManager.FreeSegmentCountDefragmentBound = value; }
//		/// <summary>
//		/// Gets the number of pieces of the buffer.
//		/// </summary>
//		public int BufferPieceCount => _heapManager.BufferPieceCount;
//		/// <summary>
//		/// Gets the extreme address of the buffer.
//		/// </summary>
//		public long BufferExtremeOffset => _heapManager.BufferExtremeOffset;
//		/// <summary>
//		/// Gets the number of bytes of the buffer.
//		/// </summary>
//		public long BufferByteCount => _heapManager.BufferByteCount;
//		/// <summary>
//		/// Gets the number of occupied bytes of the buffer.
//		/// </summary>
//		public long BufferOccupiedByteCount => _heapManager.BufferOccupiedByteCount;
//		/// <summary>
//		/// Gets or sets the max number of bytes in a piece of the buffer.
//		/// </summary>
//		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 1.</exception>
//		public int MaxBufferPieceByteCount { get => _heapManager.MaxBufferPieceByteCount; set => _heapManager.MaxBufferPieceByteCount = value; }
//		/// <summary>
//		/// Gets the value meaning whether all the changes are committed.
//		/// </summary>
//		public bool Committed => _heapManager.Committed;
//		/// <summary>
//		/// Gets the value meaning whether the <see cref="StorageManager"/> is disposed.
//		/// </summary>
//		public bool Disposed => _heapManager.Disposed;
//		/// <summary>
//		/// Gets or sets the max number of bytes to read at a time.
//		/// </summary>
//		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 2.</exception>
//		public int MaxReadWriteByteCount { get => _heapManager.MaxReadWriteByteCount; set => _heapManager.MaxReadWriteByteCount = value; }
//		/// <summary>
//		/// Gets or sets the max number of pieces of the buffer.
//		/// </summary>
//		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 1.</exception>
//		public int MaxBufferPieceCount { get => _heapManager.MaxBufferPieceCount; set => _heapManager.MaxBufferPieceCount = value; }
//		/// <summary>
//		/// Gets or sets the max number of buffered bytes.
//		/// </summary>
//		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 1.</exception>
//		public long MaxBufferByteCount { get => _heapManager.MaxBufferByteCount; set => _heapManager.MaxBufferByteCount = value; }

//		internal void ReleaseArrayManager(ArrayManager arrayManager)
//		{
//			lock (_arrayManagerLock)
//				_arrayManagers.Remove(arrayManager._pointerManager);
//		}
//		internal ArrayManager GetArrayManager(PointerManager pointerManager)
//		{
//		Find:
//			bool wait = false;
//			Monitor.Enter(_arrayManagerLock);
//			try
//			{
//				ArrayManager arrayManager;
//				if (_arrayManagers.TryGetValue(pointerManager, out WeakReference<ArrayManager> reference))
//				{
//					if (reference.TryGetTarget(out arrayManager))
//						return arrayManager;
//					wait = true;
//					goto Find;
//				}
//				arrayManager = new ArrayManager(this, pointerManager);
//				_arrayManagers.Add(pointerManager, new WeakReference<ArrayManager>(arrayManager));
//				return arrayManager;
//			}
//			finally
//			{
//				Monitor.Exit(_arrayManagerLock);
//				if (wait)
//					GC.WaitForPendingFinalizers();
//			}
//		}
//		internal IEnumerator<ArrayManager> GetArrayManagers(ArrayManager arrayManager, int index, int length)
//		{
//			if (arrayManager._meta._pointerCount == 0)
//				yield break;
//			int valuesSize = arrayManager._meta.ValueSize;
//			byte[] buffer = null;
//			for (int roof = index + length; index < roof;)
//			{
//				int readLength = _heapManager.GetReadLength(valuesSize, roof - index);
//				int readSize = valuesSize * readLength;
//				ArrayHelper.CheckSize(ref buffer, readSize);
//				arrayManager._pointerManager.Read(ArrayManager.Meta.BitConverter._instance.ByteCount + valuesSize * (long)index, readSize, buffer, 0);
//				for (int localFloor = index, localRoof = localFloor + readLength; index < localRoof; index++)
//					for (int pointerIndex = 0; pointerIndex < arrayManager._meta._pointerCount; pointerIndex++)
//						yield return GetArrayManager(_heapManager.Get(Int32BitConverterBuilder.Instance.GetInstance(buffer, valuesSize * (index - localFloor) + arrayManager._meta._dataSize + Int32BitConverterBuilder.Instance.ByteCount * pointerIndex)));
//			}
//		}
//		internal SuspectArrayCase CollectSuspectObjects(ArrayManager arrayManager)
//		{
//			if (_suspectArrayCases.TryGetValue(arrayManager, out SuspectArrayCase suspectObjectCase))
//				return suspectObjectCase;
//			suspectObjectCase = new SuspectArrayCase(arrayManager);
//			_suspectArrayCases.Add(arrayManager, suspectObjectCase);
//			IEnumerator<ArrayManager> arrayManagers = GetArrayManagers(arrayManager, 0, arrayManager.Length);
//			while (arrayManagers.MoveNext())
//			{
//				ArrayManager target = arrayManagers.Current;
//				if (target.IsNull)
//					continue;
//				CollectSuspectObjects(target)._externalReferenceCount--;
//			}
//			return suspectObjectCase;
//		}
//		internal void HandleSuspectObjectCase(SuspectArrayCase suspectArrayCase)
//		{
//			if (suspectArrayCase._externalReferenceCount == 0 || suspectArrayCase._handled)
//				return;
//			suspectArrayCase._handled = true;
//			IEnumerator<ArrayManager> arrayManagers = GetArrayManagers(suspectArrayCase._arrayManager, 0, suspectArrayCase._arrayManager.Length);
//			while (arrayManagers.MoveNext())
//			{
//				ArrayManager arrayManager = arrayManagers.Current;
//				if (arrayManager.IsNull)
//					continue;
//				SuspectArrayCase targetSuspectObjectCase = _suspectArrayCases[arrayManager];
//				targetSuspectObjectCase._externalReferenceCount++;
//				HandleSuspectObjectCase(targetSuspectObjectCase);
//			}
//		}
//		internal void Lease(ArrayManager owner, ArrayManager target)
//		{
//			if (target.IsNull)
//				return;
//			if (_dirtyArrayManagers.Remove(target))
//			{
//				if (target != owner && _dirtyArrayManagers.Contains(owner))
//					return;
//				_suspectArrayManagers.Add(target);
//				return;
//			}
//			target.Lease();
//			if (!_dirtyArrayManagers.Contains(owner))
//				return;
//			_suspectArrayManagers.Remove(target);
//		}
//		internal void Release(ArrayManager owner, ArrayManager target)
//		{
//			if (target.IsNull)
//				return;
//			if (target._meta._parentCount == 1)
//			{
//				_suspectArrayManagers.Remove(target);
//				if (_committing)
//					target.Free();
//				else
//					_dirtyArrayManagers.Add(target);
//				return;
//			}
//			target.Release();
//			_suspectArrayManagers.Add(target);
//		}
//		internal void Lease(ArrayManager owner, int index, int length)
//		{
//			IEnumerator<ArrayManager> arrayManagers = GetArrayManagers(owner, index, length);
//			while (arrayManagers.MoveNext())
//				Lease(owner, arrayManagers.Current);
//		}
//		internal void Release(ArrayManager owner, int index, int length)
//		{
//			IEnumerator<ArrayManager> arrayManagers = GetArrayManagers(owner, index, length);
//			while (arrayManagers.MoveNext())
//				Release(owner, arrayManagers.Current);
//		}
//		/// <summary>
//		/// Creates a new array in the domain.
//		/// </summary>
//		/// <param name="dataSize">The size of the data of values of elements of the array.</param>
//		/// <param name="pointerCount">The number of pointers of values of elements of the array.</param>
//		/// <param name="length">The number of elements of the array.</param>
//		/// <returns>An <see cref="ArrayManager"/> of the array.</returns>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="dataSize"/> is less than 0 or greater than 1073741823 or <paramref name="pointerCount"/> is less than 0 or greater than 134217728 or <paramref name="length"/> is less than 0.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public ArrayManager CreateArray(int dataSize, int pointerCount, int length)
//		{
//			if (Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (dataSize < 0 || dataSize > 1073741823)
//				throw new ArgumentOutOfRangeException(nameof(dataSize));
//			if (pointerCount < 0 || pointerCount > 134217728)
//				throw new ArgumentOutOfRangeException(nameof(pointerCount));
//			if (length < 0)
//				throw new ArgumentOutOfRangeException(nameof(length));
//			ArrayManager.Meta meta = new ArrayManager.Meta(dataSize, pointerCount);
//			PointerManager pointerManager = _heapManager.Create(ArrayManager.Meta.BitConverter._instance.ByteCount + meta.ValueSize * (long)length);
//			pointerManager.Write(meta, ArrayManager.Meta.BitConverter._instance, 0);
//			int valueSize = meta.ValueSize;
//			ArrayHelper.CheckSize(ref _initialElementBuffer, valueSize);
//			ArrayHelper.FillWithZeros(_initialElementBuffer, 0, meta._dataSize);
//			ArrayManager nullArrayManager = NullArrayManager;
//			for (int pointerIndex = 0; pointerIndex < meta._pointerCount; pointerIndex++)
//				Int32BitConverterBuilder.Instance.GetBytes(nullArrayManager._pointerManager.Index, _initialElementBuffer, meta._dataSize + Int32BitConverterBuilder.Instance.ByteCount * pointerIndex);
//			for (int index = 0; index < length; index++)
//				pointerManager.Write(_initialElementBuffer, 0, valueSize, ArrayManager.Meta.BitConverter._instance.ByteCount + valueSize * (long)index);
//			ArrayManager arrayManager = GetArrayManager(pointerManager);
//			Release(nullArrayManager, arrayManager);
//			return arrayManager;
//		}
//		/// <summary>
//		/// Copies a range of elements from an array starting at the specified source index and pastes them to another array starting at the specified destination index.
//		/// </summary>
//		/// <param name="sourceArrayManager">The <see cref="ArrayManager"/> of the array that contains the data to copy.</param>
//		/// <param name="sourceIndex">The index in the <paramref name="sourceArrayManager"/> at which copying begins.</param>
//		/// <param name="destinationArrayManager">The <see cref="ArrayManager"/> of the array that receives the data.</param>
//		/// <param name="destinationIndex">The index in the <paramref name="destinationArrayManager"/> at which storing begins.</param>
//		/// <param name="length">The number of elements to copy.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="sourceArrayManager"/> is null or <paramref name="destinationArrayManager"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="sourceIndex"/> is outside the range of valid indices of an array of <paramref name="sourceArrayManager"/> or <paramref name="destinationIndex"/> is outside the range of valid indices of an array of <paramref name="destinationArrayManager"/> or <paramref name="length"/> is less than 0.</exception>
//		/// <exception cref="ArgumentException">The array of <paramref name="sourceArrayManager"/> is lost or the array of <paramref name="destinationArrayManager"/> is lost or <paramref name="sourceArrayManager"/> is not of the <see cref="DomainManager"/> or <paramref name="destinationArrayManager"/> is not of the <see cref="DomainManager"/> or arrays of <paramref name="destinationArrayManager"/> and <paramref name="sourceArrayManager"/> have different element types or <paramref name="length"/> is greater than the number of elements from <paramref name="sourceIndex"/> to the end of an array of <paramref name="sourceArrayManager"/> or <paramref name="length"/> is greater than the number of elements from <paramref name="destinationIndex"/> to the end of an array of <paramref name="destinationArrayManager"/>.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public void Copy(ArrayManager sourceArrayManager, int sourceIndex, ArrayManager destinationArrayManager, int destinationIndex, int length)
//		{
//			if (Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			if (sourceArrayManager == null)
//				throw new ArgumentNullException(nameof(sourceArrayManager));
//			if (destinationArrayManager == null)
//				throw new ArgumentNullException(nameof(destinationArrayManager));
//			if (sourceArrayManager.Lost)
//				throw new ArgumentException(string.Format("The array of {0} is lost.", nameof(sourceArrayManager)));
//			if (destinationArrayManager.Lost)
//				throw new ArgumentException(string.Format("The array of {0} is lost.", nameof(destinationArrayManager)));
//			if (sourceArrayManager._domainManager != this)
//				throw new ArgumentException(string.Format("{0} is not of the {1}.", nameof(sourceArrayManager), nameof(DomainManager)));
//			if (destinationArrayManager._domainManager != this)
//				throw new ArgumentException(string.Format("{0} is not of the {1}.", nameof(destinationArrayManager), nameof(DomainManager)));
//			if (sourceArrayManager._meta._dataSize != destinationArrayManager._meta._dataSize || sourceArrayManager._meta._pointerCount != destinationArrayManager._meta._pointerCount)
//				throw new ArgumentException(string.Format("Objects of {0} and {1} have different element types.", nameof(sourceArrayManager), nameof(destinationArrayManager)));
//			if (length < 0)
//				throw new ArgumentOutOfRangeException(nameof(length));
//			if (sourceIndex < 0 || sourceIndex > sourceArrayManager.Length)
//				throw new ArgumentOutOfRangeException(nameof(sourceIndex));
//			if (destinationIndex < 0 || destinationIndex > destinationArrayManager.Length)
//				throw new ArgumentOutOfRangeException(nameof(destinationIndex));
//			if (length > sourceArrayManager.Length - sourceIndex)
//				throw new ArgumentException(string.Format("{0} is greater than the number of elements from {1} to the end of an array of {2}.", nameof(length), nameof(sourceIndex), nameof(sourceArrayManager)));
//			if (length > destinationArrayManager.Length - destinationIndex)
//				throw new ArgumentException(string.Format("{0} is greater than the number of elements from {1} to the end of an array of {2}.", nameof(length), nameof(destinationIndex), nameof(destinationArrayManager)));
//			if (length == 0)
//				return;
//			Release(destinationArrayManager, destinationIndex, length);
//			int valueSize = sourceArrayManager._meta.ValueSize;
//			_heapManager.Copy(sourceArrayManager._pointerManager, ArrayManager.Meta.BitConverter._instance.ByteCount + valueSize * (long)sourceIndex, destinationArrayManager._pointerManager, ArrayManager.Meta.BitConverter._instance.ByteCount + valueSize * (long)destinationIndex, valueSize * (long)length);
//			Lease(destinationArrayManager, destinationIndex, length);
//		}
//		/// <summary>
//		/// Defragments the domain.
//		/// </summary>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		/// <exception cref="InvalidOperationException">The non-committed changes exist.</exception>
//		public void Defragment()
//		{
//			if (!Committed)
//				throw new InvalidOperationException("The non-committed changes exist.");
//			if (Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			_heapManager.Defragment();
//		}
//		/// <summary>
//		/// Commits the made changes.
//		/// </summary>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public void Commit()
//		{
//			if (Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			_committing = true;
//			foreach (ArrayManager arrayManager in _dirtyArrayManagers)
//				arrayManager.Free();
//			_dirtyArrayManagers.Clear();
//			foreach (ArrayManager suspectArrayManager in _suspectArrayManagers)
//			{
//				if (suspectArrayManager.Lost)
//					continue;
//				_suspectArrayCases.Clear();
//				SuspectArrayCase mainSuspectObjectCase = CollectSuspectObjects(suspectArrayManager);
//				if (mainSuspectObjectCase._externalReferenceCount > 0)
//					continue;
//				foreach (SuspectArrayCase suspectObjectCase in _suspectArrayCases.Values)
//					HandleSuspectObjectCase(suspectObjectCase);
//				if (mainSuspectObjectCase._externalReferenceCount > 0)
//					continue;
//				foreach (SuspectArrayCase suspectObjectCase in _suspectArrayCases.Values)
//				{
//					if (suspectObjectCase._externalReferenceCount == 0)
//					{
//						suspectObjectCase._arrayManager._pointerManager.Free();
//						continue;
//					}
//					Contract.Assert(suspectObjectCase._externalReferenceCount > 0);
//					if (suspectObjectCase._externalReferenceCount == suspectObjectCase._arrayManager._meta._parentCount)
//						continue;
//					suspectObjectCase._arrayManager.ChangeReferenceCount(suspectObjectCase._externalReferenceCount);
//				}
//			}
//			_suspectArrayManagers.Clear();
//			_committing = false;
//			_heapManager.Commit();
//		}
//		/// <summary>
//		/// Makes backup of the domain.
//		/// </summary>
//		/// <param name="directory">The directory for the backup file.</param>
//		/// <exception cref="InvalidOperationException">The non-committed changes exist.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public void Backup(string directory)
//		{
//			if (!Committed)
//				throw new InvalidOperationException("The non-committed changes exist.");
//			if (Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			_heapManager.Backup(directory);
//		}
//		/// <summary>
//		/// Releases all resources used by the <see cref="DomainManager"/>.
//		/// </summary>
//		/// <exception cref="InvalidOperationException">The non-committed changes exist.</exception>
//		public void Dispose()
//		{
//			if (!Committed)
//				throw new InvalidOperationException("The non-committed changes exist.");
//			if (Disposed)
//				throw new ObjectDisposedException(string.Empty);
//			_heapManager.Dispose();
//		}
//	}
//}