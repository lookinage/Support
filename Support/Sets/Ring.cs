using System;

namespace Support.Sets
{
	/// <summary>
	/// Represents an <see cref="IRing{T}"/> realized in the memory.
	/// </summary>
	/// <typeparam name="T">The type of elements of the output set.</typeparam>
	public class Ring<T> : IRing<T> where T : ISetElement<T>
	{
		private struct Inserter
		{
			private readonly T[] _storage;
			private readonly int _capacity;
			private int _index;
			private int _count;

			internal Inserter(T[] storage, int capacity, int index, int count)
			{
				_storage = storage;
				_capacity = capacity;
				_index = index;
				_count = count;
			}

			internal int Count => _count;

			internal bool InsertDirectly(T element)
			{
				if (_count == 0x0)
					throw new ArgumentException(SubsetHelper.GetSubsetDoesNotContainElementsAsManyAsItIsSpecifiedExceptionMessage());
				if (_index == _capacity)
				{
					_storage[0x0] = element;
					_index = 0x1;
					_count--;
					return false;
				}
				_storage[_index++] = element;
				_count--;
				return false;
			}
			internal bool InsertInversely(T element)
			{
				if (_count == 0x0)
					throw new ArgumentException(SubsetHelper.GetSubsetDoesNotContainElementsAsManyAsItIsSpecifiedExceptionMessage());
				if (_index < 0x0)
				{
					int capacity;
					_storage[capacity = _capacity - 0x1] = element;
					_index = capacity - 0x1;
					_count--;
					return false;
				}
				_storage[_index--] = element;
				_count--;
				return false;
			}
		}
		/// <summary>
		/// Represents an enumerator of elements of a <see cref="Ring{T}"/>.
		/// </summary>
		public struct Enumerator : IEnumerator<Relation<Integer, T>>
		{
			private readonly T[] _storage;
			private readonly int _capacity;
			private readonly int _start;
			private readonly int _count;
			private int _index;
			private T _current;
			private bool _disposed;

			internal Enumerator(Ring<T> instance)
			{
				_storage = instance._storage;
				_capacity = _storage.Length;
				_start = instance._startIndex;
				_count = instance._count;
				_index = -0x1;
				_current = default;
				_disposed = false;
			}

			/// <summary>
			/// Gets the current element of the <see cref="Enumerator"/>.
			/// </summary>
			/// <exception cref="InvalidOperationException">The <see cref="Enumerator"/> is not started.</exception>
			/// <exception cref="InvalidOperationException">The <see cref="Enumerator"/> is over.</exception>
			public Relation<Integer, T> Current
			{
				get
				{
					if (_index < 0)
						ThrowEnumeratorIsNotStartedOrEnumerationIsOverException();
					return new Relation<Integer, T>(_index, _current);
				}
			}

			private void ThrowEnumeratorIsNotStartedOrEnumerationIsOverException() => throw new InvalidOperationException(_index == -0x1 ? EnumeratorHelper.GetEnumeratorIsNotStartedExceptionMessage(this) : EnumeratorHelper.GetEnumeratorIsOverEnumeratingExceptionMessage(this));
			/// <summary>
			/// Releases all resources used by the <see cref="Enumerator"/>.
			/// </summary>
			public void Dispose()
			{
				if (_disposed)
					return;
				_disposed = true;
			}
			/// <summary>
			/// Sets the next element of the <see cref="Ring{T}"/> as current.
			/// </summary>
			/// <returns><see langword="true"/> whether the next element is set as current; otherwise, <see langword="false"/>.</returns>
			public bool MoveNext()
			{
				if (_index == -0x2)
					return false;
				int index;
				if ((index = _index + 0x1) == _count)
				{
					_index = -0x2;
					return false;
				}
				_index = index;
				int capacity;
				_current = _storage[(index += _start) >= (capacity = _capacity) ? index - capacity : index];
				return true;
			}
		}
		/// <summary>
		/// Represents an editor of a <see cref="Ring{T}"/>.
		/// </summary>
		public struct Editor : IRingEditor<T>
		{
			private readonly Ring<T> _instance;

			internal Editor(Ring<T> instance) => _instance = instance;

			/// <summary>
			/// Sets the element that corresponds an index.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <exception cref="ArgumentException">The <see cref="Ring{T}"/> does not contain a relation with <paramref name="index"/>.</exception>
			public T this[Integer index]
			{
				set
				{
					if (!_instance.TrySet(index, value, out _))
						throw new ArgumentException(SurjectionHelper.GetSurjectionDoesNotContainRelationWithInputExceptionMessage(this, nameof(index)));
				}
			}

			/// <summary>
			/// Removes all elements from the <see cref="Ring{T}"/>.
			/// </summary>
			public void Clear() => _instance.Clear();
			/// <summary>
			/// Sets an element that corresponds an index if the <see cref="Ring{T}"/> contains a relation with the index.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="newElement">The new element.</param>
			/// <param name="oldElement">The old element if the <see cref="Ring{T}"/> contains a relation with <paramref name="index"/>; otherwise, the default value.</param>
			/// <returns><see langword="true"/> whether the <see cref="Ring{T}"/> contains a relation with <paramref name="index"/>; otherwise, <see langword="false"/>.</returns>
			public bool TrySet(Integer index, T newElement, out T oldElement) => _instance.TrySet(index, newElement, out oldElement);
			/// <summary>
			/// Sets an element that corresponds an index if the <see cref="Ring{T}"/> contains a relation with the index.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="newElement">The new element.</param>
			/// <returns><see langword="true"/> whether the <see cref="Ring{T}"/> contains a relation with <paramref name="index"/>; otherwise, <see langword="false"/>.</returns>
			public bool TrySet(Integer index, T newElement) => _instance.TrySet(index, newElement, out _);
			/// <summary>
			/// Sets an element that corresponds an index.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="newElement">The new element.</param>
			/// <param name="oldElement">The old element.</param>
			/// <exception cref="ArgumentException">The <see cref="Ring{T}"/> does not contain a relation with <paramref name="index"/>.</exception>
			public void Set(Integer index, T newElement, out T oldElement)
			{
				if (!_instance.TrySet(index, newElement, out oldElement))
					throw new ArgumentException(SurjectionHelper.GetSurjectionDoesNotContainRelationWithInputExceptionMessage(this, nameof(index)));
			}
			/// <summary>
			/// Inserts an element at an index.
			/// </summary>
			/// <param name="index">The index of the element.</param>
			/// <param name="element">The new element at <paramref name="index"/>.</param>
			/// <exception cref="InvalidOperationException">The <see cref="Ring{T}"/> would be overflowed.</exception>
			/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <see cref="Ring{T}"/>.</exception>
			public void Insert(Integer index, T element) => _instance.Insert(index, element);
			/// <summary>
			/// Inserts the elements of an <see cref="ISequence{T}"/> in direct order at an index.
			/// </summary>
			/// <param name="index">The index of the first element of the sequence to insert.</param>
			/// <param name="sequence">The sequence.</param>
			/// <exception cref="InvalidOperationException">The <see cref="Ring{T}"/> would be overflowed.</exception>
			/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <see cref="Ring{T}"/>.</exception>
			/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
			/// <exception cref="ArgumentException"><paramref name="sequence"/> does not contain elements as many as it is specified.</exception>
			public void InsertDirectly(Integer index, ISequence<T> sequence) => _instance.InsertDirectly(index, sequence);
			/// <summary>
			/// Inserts the elements of an <see cref="ISequence{T}"/> in reverse order at an index.
			/// </summary>
			/// <param name="index">The index of the first element of the sequence to insert.</param>
			/// <param name="sequence">The sequence.</param>
			/// <exception cref="InvalidOperationException">The <see cref="Ring{T}"/> would be overflowed.</exception>
			/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <see cref="Ring{T}"/>.</exception>
			/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
			/// <exception cref="ArgumentException"><paramref name="sequence"/> does not contain elements as many as it is specified.</exception>
			public void InsertInversely(Integer index, ISequence<T> sequence) => _instance.InsertInversely(index, sequence);
			/// <summary>
			/// Adds an element to the start of the <see cref="Ring{T}"/>.
			/// </summary>
			/// <param name="element">The element to add to the <see cref="Ring{T}"/>.</param>
			/// <exception cref="InvalidOperationException">The <see cref="Ring{T}"/> would be overflowed.</exception>
			public void AddFirst(T element) => _instance.AddFirst(element);
			/// <summary>
			/// Adds an element to the endIndex of the <see cref="Ring{T}"/>.
			/// </summary>
			/// <param name="element">The element to add to the <see cref="Ring{T}"/>.</param>
			/// <exception cref="InvalidOperationException">The <see cref="Ring{T}"/> would be overflowed.</exception>
			public void AddLast(T element) => _instance.AddLast(element);
			/// <summary>
			/// Adds the element of an <see cref="ISequence{T}"/> to the start of the <see cref="Ring{T}"/>.
			/// </summary>
			/// <param name="sequence">The sequence.</param>
			/// <exception cref="InvalidOperationException">The <see cref="Ring{T}"/> would be overflowed.</exception>
			/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
			/// <exception cref="ArgumentException"><paramref name="sequence"/> contains more elements than it is specified.</exception>
			public void AddFirstDirectly(ISequence<T> sequence) => _instance.AddFirstDirectly(sequence);
			/// <summary>
			/// Adds the element of an <see cref="ISequence{T}"/> to the start of the <see cref="Ring{T}"/>.
			/// </summary>
			/// <param name="sequence">The sequence.</param>
			/// <exception cref="InvalidOperationException">The <see cref="Ring{T}"/> would be overflowed.</exception>
			/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
			/// <exception cref="ArgumentException"><paramref name="sequence"/> contains more elements than it is specified.</exception>
			public void AddFirstInversely(ISequence<T> sequence) => _instance.AddFirstInversely(sequence);
			/// <summary>
			/// Adds the element of an <see cref="ISequence{T}"/> to the endIndex of the <see cref="Ring{T}"/>.
			/// </summary>
			/// <param name="sequence">The sequence.</param>
			/// <exception cref="InvalidOperationException">The <see cref="Ring{T}"/> would be overflowed.</exception>
			/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
			/// <exception cref="ArgumentException"><paramref name="sequence"/> contains more elements than it is specified.</exception>
			public void AddLastDirectly(ISequence<T> sequence) => _instance.AddLastDirectly(sequence);
			/// <summary>
			/// Adds the element of an <see cref="ISequence{T}"/> to the endIndex of the <see cref="Ring{T}"/>.
			/// </summary>
			/// <param name="sequence">The sequence.</param>
			/// <exception cref="InvalidOperationException">The <see cref="Ring{T}"/> would be overflowed.</exception>
			/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
			/// <exception cref="ArgumentException"><paramref name="sequence"/> contains more elements than it is specified.</exception>
			public void AddLastInversely(ISequence<T> sequence) => _instance.AddLastInversely(sequence);
			/// <summary>
			/// Removes the element from the endIndex of the <see cref="Ring{T}"/> if the <see cref="Ring{T}"/> is not empty.
			/// </summary>
			/// <param name="element">The element from the endIndex of the <see cref="Ring{T}"/> whether is removed; otherwise, the default value.</param>
			/// <returns><see langword="true"/> whether the element is removed; otherwise, <see langword="false"/>.</returns>
			public bool TryRemoveFirst(out T element) => _instance.TryRemoveFirst(out element);
			/// <summary>
			/// Removes the element from the start of the <see cref="Ring{T}"/> if the <see cref="Ring{T}"/> is not empty.
			/// </summary>
			/// <param name="element">The element from the start of the <see cref="Ring{T}"/> whether is removed; otherwise, the default value.</param>
			/// <returns><see langword="true"/> whether the element is removed; otherwise, <see langword="false"/>.</returns>
			public bool TryRemoveLast(out T element) => _instance.TryRemoveLast(out element);
			/// <summary>
			/// Removes the element from the start of the <see cref="Ring{T}"/>.
			/// </summary>
			/// <returns>The element from the start of the <see cref="Ring{T}"/>.</returns>
			/// <exception cref="InvalidOperationException">The <see cref="Ring{T}"/> is empty.</exception>
			public T RemoveFirst()
			{
				if (!_instance.TryRemoveFirst(out T element))
					throw new InvalidOperationException(SubsetHelper.GetSubsetIsEmptyExceptionMessage(this));
				return element;
			}
			/// <summary>
			/// Removes the element from the endIndex of the <see cref="Ring{T}"/>.
			/// </summary>
			/// <returns>The element from the endIndex of the <see cref="Ring{T}"/>.</returns>
			/// <exception cref="InvalidOperationException">The <see cref="Ring{T}"/> is empty.</exception>
			public T RemoveLast()
			{
				if (!_instance.TryRemoveLast(out T element))
					throw new InvalidOperationException(SubsetHelper.GetSubsetIsEmptyExceptionMessage(this));
				return element;
			}
		}

		private const int _defaultCapacity = 0x4;

		private T[] _storage;
		private int _startIndex;
		private int _endIndex;
		private int _count;

		/// <summary>
		/// Initializes the <see cref="Ring{T}"/>.
		/// </summary>
		/// <param name="capacity">The minimum number of indices that is to be related to elements of the set.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
		public Ring(int capacity)
		{
			if (capacity < 0x0)
				throw new ArgumentOutOfRangeException(nameof(capacity));
			_storage = new T[capacity];
		}
		/// <summary>
		/// Initializes the <see cref="Ring{T}"/>.
		/// </summary>
		public Ring() : this(_defaultCapacity) { }

		/// <summary>
		/// Gets the number of elements of the <see cref="Ring{T}"/>.
		/// </summary>
		public int Count => _count;
		/// <summary>
		/// Gets or sets the element that corresponds an index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <exception cref="ArgumentException">The <see cref="Ring{T}"/> does not contain a relation with <paramref name="index"/>.</exception>
		public T this[Integer index]
		{
			get
			{
				if (!TryGet(index, out T element))
					throw new ArgumentException(SurjectionHelper.GetSurjectionDoesNotContainRelationWithInputExceptionMessage(this, nameof(index)));
				return element;
			}
		}
		/// <summary>
		/// Gets the first element of the <see cref="Ring{T}"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> is empty.</exception>
		public T First
		{
			get
			{
				if (!TryGetFirst(out T element))
					throw new InvalidOperationException(SubsetHelper.GetSubsetIsEmptyExceptionMessage(this));
				return element;
			}
		}
		/// <summary>
		/// Gets the last element of the <see cref="Ring{T}"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> is empty.</exception>
		public T Last
		{
			get
			{
				if (!TryGetLast(out T element))
					throw new InvalidOperationException(SubsetHelper.GetSubsetIsEmptyExceptionMessage(this));
				return element;
			}
		}

		private void IncreaseCapacity(T[] storage, int capacity, int size)
		{
			if (size < 0x0)
				throw new InvalidOperationException(SubsetHelper.GetSubsetWouldBeOverflowed(this));
			int previousCapacity = capacity;
			do
			{
				if ((capacity <<= 0x1) > 0x0)
					continue;
				capacity = int.MaxValue;
				break;
			}
			while (capacity < size);
			int startIndex;
			int endIndex;
			int count = _count;
			T[] newStorage = new T[capacity];
			if ((startIndex = _startIndex) < (endIndex = _endIndex))
				Array.Copy(storage, startIndex, newStorage, 0x0, count);
			else
			{
				Array.Copy(storage, startIndex, newStorage, 0x0, previousCapacity - startIndex);
				Array.Copy(storage, 0x0, newStorage, previousCapacity - startIndex, endIndex);
			}
			_storage = newStorage;
			_startIndex = 0x0;
			_endIndex = count;
		}
		private void Clear()
		{
			int startIndex;
			int endIndex;
			if ((startIndex = _startIndex) > (endIndex = _endIndex))
			{
				T[] storage;
				Array.Clear(storage = _storage, startIndex, storage.Length - startIndex);
				Array.Clear(storage, 0x0, endIndex);
			}
			else
				Array.Clear(_storage, startIndex, _count);
			_startIndex = 0x0;
			_endIndex = 0x0;
			_count = 0x0;
		}
		private bool TrySet(Integer index, T newElement, out T oldElement)
		{
			if (index < 0x0 || index >= _count)
			{
				oldElement = default;
				return false;
			}
			int startIndex;
			T[] storage;
			int storageIndex;
			if ((startIndex = _startIndex) < _endIndex)
			{
				oldElement = (storage = _storage)[storageIndex = startIndex + index];
				storage[storageIndex] = newElement;
				return true;
			}
			int capacity;
			if ((storageIndex = startIndex + index) < (capacity = (storage = _storage).Length))
			{
				oldElement = storage[storageIndex];
				storage[storageIndex] = newElement;
				return true;
			}
			oldElement = storage[storageIndex = index - capacity + startIndex];
			storage[storageIndex] = newElement;
			return true;
		}
		private void Insert(Integer index, T element) => throw new NotImplementedException();
		private void InsertDirectly(Integer index, ISequence<T> sequence)
		{
			int count;
			if (index == (count = _count))
			{
				AddLastDirectly(sequence);
				return;
			}
			if (index < 0x0 || index > count)
				throw new ArgumentException(OrderHelper.GetIndexIsOutsideRangeOfValidIndices(this, nameof(index)));
			int sequenceCount;
			if ((sequenceCount = sequence.Count) == 0x0)
				return;
			T[] storage;
			int capacity;
			if (count + sequenceCount > (capacity = (storage = _storage).Length))
			{
				IncreaseCapacity(storage, capacity, count + sequenceCount);
				capacity = (storage = _storage).Length;
			}
			_count = count + sequenceCount;
			int sourceStartIndex;
			int destinationStartIndex;
			int sourceEndIndex;
			int destinationEndIndex;
			int moveCount;
			Inserter inserter;
			if ((sourceEndIndex = (sourceStartIndex = _startIndex + index) + (moveCount = count - index)) > capacity)
			{
				if ((destinationStartIndex = sourceStartIndex + sequenceCount) >= capacity)
				{
					_endIndex = (destinationStartIndex -= capacity) + moveCount;
					Array.Copy(storage, 0x0, storage, destinationStartIndex + (moveCount -= sourceEndIndex -= capacity), sourceEndIndex);
					Array.Copy(storage, sourceStartIndex, storage, destinationStartIndex, moveCount);
					try
					{
						sequence.Handle((inserter = new Inserter(storage, capacity, sourceStartIndex, sequenceCount)).InsertDirectly);
						if (inserter.Count != 0x0)
							throw new ArgumentException(SubsetHelper.GetSubsetDoesNotContainElementsAsManyAsItIsSpecifiedExceptionMessage(nameof(sequence)));
					}
					catch
					{
						Array.Copy(storage, destinationStartIndex, storage, sourceStartIndex, moveCount);
						Array.Copy(storage, destinationStartIndex + moveCount, storage, 0x0, sourceEndIndex);
						Array.Clear(storage, _endIndex - sequenceCount, sequenceCount);
						_count = count;
						_endIndex = _startIndex + count - capacity;
						throw;
					}
					return;
				}
				_endIndex = destinationStartIndex + moveCount - capacity;
				Array.Copy(storage, 0x0, storage, destinationStartIndex + (moveCount -= sourceEndIndex -= capacity) - capacity, sourceEndIndex);
				Array.Copy(storage, capacity - sequenceCount, storage, 0x0, sequenceCount);
				Array.Copy(storage, sourceStartIndex, storage, destinationStartIndex, moveCount - sequenceCount);
				try
				{
					sequence.Handle((inserter = new Inserter(storage, capacity, sourceStartIndex, sequenceCount)).InsertDirectly);
					if (inserter.Count != 0x0)
						throw new ArgumentException(SubsetHelper.GetSubsetDoesNotContainElementsAsManyAsItIsSpecifiedExceptionMessage(nameof(sequence)));
				}
				catch
				{
					Array.Copy(storage, destinationStartIndex, storage, sourceStartIndex, moveCount - sequenceCount);
					Array.Copy(storage, 0x0, storage, capacity - sequenceCount, sequenceCount);
					Array.Copy(storage, destinationStartIndex + moveCount - capacity, storage, 0x0, sourceEndIndex);
					Array.Clear(storage, _endIndex - sequenceCount, sequenceCount);
					_count = count;
					_endIndex = _startIndex + count - capacity;
					throw;
				}
				return;
			}
			if ((destinationStartIndex = sourceStartIndex + sequenceCount) >= capacity)
			{
				Array.Copy(storage, sourceStartIndex, storage, destinationStartIndex = sourceStartIndex + sequenceCount - capacity, moveCount);
				_endIndex = destinationStartIndex + moveCount;
				try
				{
					sequence.Handle((inserter = new Inserter(storage, capacity, sourceStartIndex, sequenceCount)).InsertDirectly);
					if (inserter.Count != 0x0)
						throw new ArgumentException(SubsetHelper.GetSubsetDoesNotContainElementsAsManyAsItIsSpecifiedExceptionMessage(nameof(sequence)));
				}
				catch
				{
					Array.Copy(storage, destinationStartIndex, storage, sourceStartIndex, moveCount);
					Array.Clear(storage, capacity - (sequenceCount -= _endIndex), sequenceCount);
					Array.Clear(storage, 0x0, _endIndex);
					_count = count;
					_endIndex = _startIndex + count;
					throw;
				}
				return;
			}
			if ((destinationEndIndex = destinationStartIndex + (moveCount = count - index)) > capacity)
			{
				Array.Copy(storage, sourceStartIndex + (moveCount -= _endIndex = destinationEndIndex -= capacity), storage, 0x0, destinationEndIndex);
				Array.Copy(storage, sourceStartIndex, storage, destinationStartIndex, moveCount);
				try
				{
					sequence.Handle((inserter = new Inserter(storage, capacity, sourceStartIndex, sequenceCount)).InsertDirectly);
					if (inserter.Count != 0x0)
						throw new ArgumentException(SubsetHelper.GetSubsetDoesNotContainElementsAsManyAsItIsSpecifiedExceptionMessage(nameof(sequence)));
				}
				catch
				{
					Array.Copy(storage, destinationStartIndex, storage, sourceStartIndex, moveCount);
					Array.Copy(storage, 0x0, storage, sourceStartIndex + moveCount, destinationEndIndex);
					Array.Clear(storage, capacity - (sequenceCount -= _endIndex), sequenceCount);
					Array.Clear(storage, 0x0, _endIndex);
					_count = count;
					_endIndex = _startIndex + count;
					throw;
				}
				return;
			}
			_endIndex = destinationEndIndex != capacity ? destinationEndIndex : 0x0;
			Array.Copy(storage, sourceStartIndex, storage, destinationStartIndex, moveCount);
			try
			{
				sequence.Handle((inserter = new Inserter(storage, capacity, sourceStartIndex, sequenceCount)).InsertDirectly);
				if (inserter.Count != 0x0)
					throw new ArgumentException(SubsetHelper.GetSubsetDoesNotContainElementsAsManyAsItIsSpecifiedExceptionMessage(nameof(sequence)));
			}
			catch
			{
				Array.Copy(storage, destinationStartIndex, storage, sourceStartIndex, moveCount);
				Array.Clear(storage, destinationEndIndex - sequenceCount, sequenceCount);
				_count = count;
				_endIndex = _startIndex + count;
				throw;
			}
			return;
		}
		private void InsertInversely(Integer index, ISequence<T> sequence) => throw new NotImplementedException();
		private void AddFirst(T element)
		{
			T[] storage;
			int count;
			int capacity;
			if ((count = _count) == (capacity = (storage = _storage).Length))
			{
				IncreaseCapacity(storage, capacity, count + 0x1);
				capacity = (storage = _storage).Length;
			}
			int startIndex;
			storage[_startIndex = (startIndex = _startIndex - 0x1) < 0x0 ? capacity - 0x1 : startIndex] = element;
			_count = count + 0x1;
		}
		private void AddLast(T element)
		{
			T[] storage;
			int count;
			int capacity;
			if ((count = _count) == (capacity = (storage = _storage).Length))
			{
				IncreaseCapacity(storage, capacity, count + 0x1);
				capacity = (storage = _storage).Length;
			}
			int endIndex;
			storage[endIndex = _endIndex] = element;
			_endIndex = ++endIndex != capacity ? endIndex : 0x0;
			_count = count + 0x1;
		}
		private void AddFirstDirectly(ISequence<T> sequence)
		{
			int sequenceCount;
			if ((sequenceCount = sequence.Count) == 0x0)
				return;
			T[] storage;
			int count;
			int capacity;
			if ((count = _count) + sequenceCount > (capacity = (storage = _storage).Length))
			{
				IncreaseCapacity(storage, capacity, count + sequenceCount);
				capacity = (storage = _storage).Length;
			}
			_count = count + sequenceCount;
			int index;
			int startIndex;
			_startIndex = (startIndex = (index = _startIndex) - sequenceCount) >= 0x0 ? startIndex : startIndex + capacity;
			try { sequence.Handle(new Inserter(storage, capacity, index, sequenceCount).InsertInversely); }
			catch
			{
				_count = count;
				_startIndex = (startIndex = _endIndex - count) >= 0x0 ? startIndex : startIndex + capacity;
				throw;
			}
		}
		private void AddFirstInversely(ISequence<T> sequence)
		{
			int sequenceCount;
			if ((sequenceCount = sequence.Count) == 0x0)
				return;
			T[] storage;
			int count;
			int capacity;
			if ((count = _count) + sequenceCount > (capacity = (storage = _storage).Length))
			{
				IncreaseCapacity(storage, capacity, count + sequenceCount);
				capacity = (storage = _storage).Length;
			}
			_count = count + sequenceCount;
			int startIndex;
			_startIndex = (startIndex = _startIndex - sequenceCount) >= 0x0 ? startIndex : startIndex += capacity;
			try { sequence.Handle(new Inserter(storage, capacity, startIndex, sequenceCount).InsertDirectly); }
			catch
			{
				_count = count;
				_startIndex = (startIndex = _endIndex - count) >= 0x0 ? startIndex : startIndex + capacity;
				throw;
			}
		}
		private void AddLastDirectly(ISequence<T> sequence)
		{
			int sequenceCount;
			if ((sequenceCount = sequence.Count) == 0x0)
				return;
			T[] storage;
			int count;
			int capacity;
			if ((count = _count) + sequenceCount > (capacity = (storage = _storage).Length))
			{
				IncreaseCapacity(storage, capacity, count + sequenceCount);
				capacity = (storage = _storage).Length;
			}
			_count = count + sequenceCount;
			int index;
			int endIndex;
			_endIndex = (endIndex = (index = _endIndex) + sequenceCount) < capacity ? endIndex : endIndex - capacity;
			try { sequence.Handle(new Inserter(storage, capacity, index, sequenceCount).InsertDirectly); }
			catch
			{
				_count = count;
				_endIndex = (endIndex = _startIndex + count) < capacity ? endIndex : endIndex - capacity;
				throw;
			}
		}
		private void AddLastInversely(ISequence<T> sequence)
		{
			int sequenceCount;
			if ((sequenceCount = sequence.Count) == 0x0)
				return;
			T[] storage;
			int count;
			int capacity;
			if ((count = _count) + sequenceCount > (capacity = (storage = _storage).Length))
			{
				IncreaseCapacity(storage, capacity, count + sequenceCount);
				capacity = (storage = _storage).Length;
			}
			_count = count + sequenceCount;
			int endIndex;
			_endIndex = (endIndex = _endIndex + sequenceCount) < capacity ? endIndex : endIndex -= capacity;
			try { sequence.Handle(new Inserter(storage, capacity, endIndex - 0x1, sequenceCount).InsertInversely); }
			catch
			{
				_count = count;
				_endIndex = (endIndex = _startIndex + count) < capacity ? endIndex : endIndex - capacity;
				throw;
			}
		}
		private bool TryRemoveFirst(out T element)
		{
			int count;
			if ((count = _count) == 0x0)
			{
				element = default;
				return false;
			}
			T[] storage = _storage;
			int startIndex = _startIndex;
			element = storage[startIndex];
			storage[startIndex] = default;
			_startIndex = ++startIndex != storage.Length ? startIndex : 0x0;
			_count = count - 0x1;
			return true;
		}
		private bool TryRemoveLast(out T element)
		{
			int count;
			if ((count = _count) == 0x0)
			{
				element = default;
				return false;
			}
			T[] storage = _storage;
			int endIndex;
			element = storage[(endIndex = _endIndex - 0x1) < 0x0 ? storage.Length - 0x1 : endIndex];
			storage[_endIndex = endIndex] = default;
			_count = count - 0x1;
			return true;
		}
		/// <summary>
		/// Gets an <see cref="Enumerator"/> of the <see cref="Ring{T}"/>.
		/// </summary>
		/// <returns>An <see cref="Enumerator"/> of the <see cref="Ring{T}"/>.</returns>
		public Enumerator GetEnumerator() => new Enumerator(this);
		/// <summary>
		/// Handles elements of the <see cref="Ring{T}"/> with an <see cref="ElementHandler{T}"/>.
		/// </summary>
		/// <param name="handler">The handler of the elements.</param>
		/// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
		public void Handle(ElementHandler<Relation<Integer, T>> handler)
		{
			if (handler == null)
				throw new ArgumentNullException(nameof(handler));
			if (_count == 0x0)
				return;
			int startIndex;
			int endIndex;
			T[] storage;
			int storageIndex;
			int index = 0x0;
			if ((startIndex = _startIndex) < (endIndex = _endIndex))
			{
				storage = _storage;
				for (storageIndex = startIndex; storageIndex != endIndex; storageIndex++)
					if (handler(new Relation<Integer, T>(index++, storage[storageIndex])))
						return;
				return;
			}
			int capacity = (storage = _storage).Length;
			for (storageIndex = startIndex; storageIndex != capacity; storageIndex++)
				if (handler(new Relation<Integer, T>(index++, storage[storageIndex])))
					return;
			for (storageIndex = 0x0; storageIndex != endIndex; storageIndex++)
				if (handler(new Relation<Integer, T>(index++, storage[storageIndex])))
					return;
		}
		/// <summary>
		/// Determines whether the <see cref="Ring{T}"/> contains a relation with an index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns><see langword="true"/> whether the <see cref="Ring{T}"/> contains a relation with <paramref name="index"/>; otherwise, <see langword="false"/>.</returns>
		public bool Contains(Integer index) => index >= 0x0 && index < _count;
		/// <summary>
		/// Gets the element that corresponds an index if the <see cref="Ring{T}"/> contains a relation with the index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="element">The element if the <see cref="Ring{T}"/> contains a relation with <paramref name="index"/>; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the <see cref="Ring{T}"/> contains a relation with <paramref name="index"/>; otherwise, <see langword="false"/>.</returns>
		public bool TryGet(Integer index, out T element)
		{
			if (index < 0x0 || index >= _count)
			{
				element = default;
				return false;
			}
			int startIndex;
			if ((startIndex = _startIndex) < _endIndex)
			{
				element = _storage[startIndex + index];
				return true;
			}
			T[] storage;
			int capacity;
			int storageIndex;
			element = (storageIndex = startIndex + index) < (capacity = (storage = _storage).Length) ? storage[storageIndex] : storage[index - capacity + startIndex];
			return true;
		}
		/// <summary>
		/// Gets the first element of the <see cref="Ring{T}"/> if the <see cref="Ring{T}"/> is not empty.
		/// </summary>
		/// <param name="element">The element if the <see cref="Ring{T}"/> is not empty; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the element is gotten; otherwise, <see langword="false"/>.</returns>
		public bool TryGetFirst(out T element)
		{
			if (_count == 0x0)
			{
				element = default;
				return false;
			}
			element = _storage[_startIndex];
			return true;
		}
		/// <summary>
		/// Gets the last element of the <see cref="Ring{T}"/> if the <see cref="Ring{T}"/> is not empty.
		/// </summary>
		/// <param name="element">The element if the <see cref="Ring{T}"/> is not empty; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the element is gotten; otherwise, <see langword="false"/>.</returns>
		public bool TryGetLast(out T element)
		{
			if (_count == 0x0)
			{
				element = default;
				return false;
			}
			int endIndex;
			element = _storage[(endIndex = _endIndex - 0x1) < 0x0 ? _storage.Length - 0x1 : endIndex];
			return true;
		}
		IEnumerator<Relation<Integer, T>> ISequence<Relation<Integer, T>>.GetEnumerator() => GetEnumerator();
		bool ISubset<Relation<Integer, T>>.Contains(Relation<Integer, T> element) => Contains(element.Input);
	}
}