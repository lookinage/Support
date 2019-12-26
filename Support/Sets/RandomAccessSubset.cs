using System;

namespace Support.Sets
{
	/// <summary>
	/// Represents an <see cref="IRandomAccessSubset{T}"/> realized in the memory.
	/// </summary>
	/// <typeparam name="T">The type of elements of the set.</typeparam>
	public class RandomAccessSubset<T> : IRandomAccessSubset<T> where T : ISetElement<T>, ISurjectiveSetElement<int>
	{
		private struct Slot
		{
			internal int _nextSlotIndex;
			internal int _hash;
			internal T _element;
		}
		/// <summary>
		/// Represents an enumerator of elements of a <see cref="RandomAccessSubset{T}"/>.
		/// </summary>
		public struct Enumerator : IEnumerator<T>
		{
			private readonly Slot[] _slots;
			private readonly int _usedSlotCount;
			private int _slotIndex;
			private T _current;
			private bool _disposed;

			internal Enumerator(RandomAccessSubset<T> instance)
			{
				_slots = instance._slots;
				_usedSlotCount = instance._usedSlotCount;
				_slotIndex = -0x1;
				_current = default;
				_disposed = false;
			}

			/// <summary>
			/// Gets the current element of the <see cref="Enumerator"/>.
			/// </summary>
			/// <exception cref="InvalidOperationException">The <see cref="Enumerator"/> is not started.</exception>
			/// <exception cref="InvalidOperationException">The <see cref="Enumerator"/> is over.</exception>
			public T Current
			{
				get
				{
					if (_slotIndex < 0)
						ThrowEnumeratorIsNotStartedOrEnumerationIsOverException();
					return _current;
				}
			}

			private void ThrowEnumeratorIsNotStartedOrEnumerationIsOverException() => throw new InvalidOperationException(_slotIndex == -0x1 ? EnumeratorHelper.GetEnumeratorIsNotStartedExceptionMessage(this) : EnumeratorHelper.GetEnumeratorIsOverEnumeratingExceptionMessage(this));
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
			/// Sets the next element of the <see cref="RandomAccessSubset{T}"/> as current.
			/// </summary>
			/// <returns><see langword="true"/> whether the next element is set as current; otherwise, <see langword="false"/>.</returns>
			public bool MoveNext()
			{
				if (_slotIndex == -0x2)
					return false;
				int nextSlotIndex;
				for (nextSlotIndex = _slotIndex + 0x1; nextSlotIndex != _usedSlotCount && _slots[nextSlotIndex]._hash == -0x1; nextSlotIndex++) ;
				if (nextSlotIndex != _usedSlotCount)
				{
					_current = _slots[_slotIndex = nextSlotIndex]._element;
					return true;
				}
				_slotIndex = -0x2;
				return false;
			}
		}
		/// <summary>
		/// Represents an editor of a <see cref="RandomAccessSubset{T}"/>.
		/// </summary>
		public struct Editor : IRandomAccessSubsetEditor<T>
		{
			private readonly RandomAccessSubset<T> _instance;

			internal Editor(RandomAccessSubset<T> instance) => _instance = instance;

			/// <summary>
			/// Removes all elements from the <see cref="RandomAccessSubset{T}"/>.
			/// </summary>
			public void Clear() => _instance.Clear();
			/// <summary>
			/// Adds an element to the <see cref="RandomAccessSubset{T}"/> if the <see cref="RandomAccessSubset{T}"/> does not contain the element.
			/// </summary>
			/// <param name="element">The element.</param>
			/// <param name="address">The address of the element in the <see cref="RandomAccessSubset{T}"/>.</param>
			/// <returns><see langword="true"/> whether <paramref name="element"/> is added to the <see cref="RandomAccessSubset{T}"/>; otherwise, <see langword="false"/>.</returns>
			/// <exception cref="InvalidOperationException">The <see cref="RandomAccessSubset{T}"/> would be overflowed.</exception>
			public bool TryAdd(T element, out int address) => _instance.TryAdd(element, out address);
			/// <summary>
			/// Adds an element to the <see cref="RandomAccessSubset{T}"/>.
			/// </summary>
			/// <param name="element">The element.</param>
			/// <returns>The address of the element in the <see cref="RandomAccessSubset{T}"/>.</returns>
			/// <exception cref="InvalidOperationException">The <see cref="RandomAccessSubset{T}"/> would be overflowed.</exception>
			/// <exception cref="ArgumentException">The <see cref="RandomAccessSubset{T}"/> already contains the element.</exception>
			public int Add(T element) => _instance.Add(element);
			/// <summary>
			/// Removes an element from the <see cref="RandomAccessSubset{T}"/> if the <see cref="RandomAccessSubset{T}"/> contains the element.
			/// </summary>
			/// <param name="element">The element.</param>
			/// <returns><see langword="true"/> whether <paramref name="element"/> is removed from the <see cref="RandomAccessSubset{T}"/>; otherwise, <see langword="false"/>.</returns>
			public bool TryRemove(T element) => _instance.TryRemove(element);
			/// <summary>
			/// Removes an element from the <see cref="RandomAccessSubset{T}"/>.
			/// </summary>
			/// <param name="element">The element.</param>
			/// <exception cref="ArgumentException">The <see cref="RandomAccessSubset{T}"/> does not contain <paramref name="element"/>.</exception>
			public void Remove(T element) => _instance.Remove(element);
			/// <summary>
			/// Removes an element at an address from the <see cref="RandomAccessSubset{T}"/> if the <see cref="RandomAccessSubset{T}"/> contains the element.
			/// </summary>
			/// <param name="address">The address.</param>
			/// <param name="element">The element if the <see cref="RandomAccessSubset{T}"/> contains; otherwise, the default value.</param>
			/// <returns><see langword="true"/> whether the element is removed from the <see cref="RandomAccessSubset{T}"/>; otherwise, <see langword="false"/>.</returns>
			public bool TryRemoveAt(int address, out T element) => _instance.TryRemoveAt(address, out element);
			/// <summary>
			/// Removes an element at an address from the <see cref="RandomAccessSubset{T}"/> if the <see cref="RandomAccessSubset{T}"/> contains the element.
			/// </summary>
			/// <param name="address">The address.</param>
			/// <returns><see langword="true"/> whether the element is removed from the <see cref="RandomAccessSubset{T}"/>; otherwise, <see langword="false"/>.</returns>
			public bool TryRemoveAt(int address) => _instance.TryRemoveAt(address, out _);
			/// <summary>
			/// Removes an element at an address from the <see cref="RandomAccessSubset{T}"/>.
			/// </summary>
			/// <param name="address">The address.</param>
			/// <returns>The element.</returns>
			/// <exception cref="ArgumentException">The <see cref="RandomAccessSubset{T}"/> does not contain the element.</exception>
			public T RemoveAt(int address) => _instance.RemoveAt(address);
		}

		private const int _defaultCapacity = 0x4;

		private Slot[] _slots;
		private int[] _bucketStartIndices;
		private int _freeSlotIndex;
		private int _usedSlotCount;
		private int _count;

		/// <summary>
		/// Initializes the <see cref="RandomAccessSubset{T}"/>.
		/// </summary>
		/// <param name="capacity">The minimum number of elements of the <see cref="RandomAccessSubset{T}"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
		public RandomAccessSubset(int capacity)
		{
			if (capacity < 0x0)
				throw new ArgumentOutOfRangeException(nameof(capacity));
			_slots = new Slot[capacity];
			_bucketStartIndices = new int[capacity];
			_freeSlotIndex = -0x1;
		}
		/// <summary>
		/// Initializes the <see cref="RandomAccessSubset{T}"/>.
		/// </summary>
		public RandomAccessSubset() : this(_defaultCapacity) { }

		/// <summary>
		/// Gets the number of elements of the <see cref="RandomAccessSubset{T}"/>.
		/// </summary>
		public int Count => _count;
		/// <summary>
		/// Gets an element at an address in the <see cref="RandomAccessSubset{T}"/>.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <exception cref="ArgumentException">The <see cref="RandomAccessSubset{T}"/> does not contain the element.</exception>
		public T this[int address]
		{
			get
			{
				if (!TryGetAt(address, out T element))
					throw new ArgumentException(SubsetHelper.GetSubsetDoesNotContainElementExceptionMessage(this));
				return element;
			}
		}

		private void IncreaseCapacity(int capacity)
		{
			int previousCapacity;
			Slot[] newStorage;
			if ((previousCapacity = capacity) == int.MaxValue)
				throw new InvalidOperationException(SubsetHelper.GetSubsetWouldBeOverflowed(this));
			if ((capacity <<= 0x1) < 0x0)
				capacity = int.MaxValue;
			Array.Copy(_slots, newStorage = new Slot[capacity], previousCapacity);
			int[] newBuckets = new int[capacity];
			for (int slotIndex = 0x0; slotIndex != previousCapacity; slotIndex++)
			{
				int bucket = _slots[slotIndex]._hash % capacity;
				newStorage[slotIndex]._nextSlotIndex = newBuckets[bucket] - 0x1;
				newBuckets[bucket] = slotIndex + 0x1;
			}
			_slots = newStorage;
			_bucketStartIndices = newBuckets;
		}
		private void Clear()
		{
			int capacity;
			Array.Clear(_slots, 0x0, capacity = _slots.Length);
			Array.Clear(_bucketStartIndices, 0x0, capacity);
			_freeSlotIndex = -0x1;
			_usedSlotCount = 0x0;
			_count = 0x0;
		}
		private bool TryAdd(T element, out int address)
		{
			Slot[] slots;
			int count;
			int capacity;
			if ((count = _count) == (capacity = (slots = _slots).Length))
			{
				IncreaseCapacity(capacity);
				capacity = (slots = _slots).Length;
			}
			int relatedElement;
			int bucket = (relatedElement = element.RelatedElement) % capacity;
			int[] buckets;
			int slotIndex;
			for (slotIndex = (buckets = _bucketStartIndices)[bucket] - 0x1; slotIndex != -0x1; slotIndex = slots[slotIndex]._nextSlotIndex)
			{
				if (element == null ? slots[slotIndex]._element != null : !element.Compare(slots[slotIndex]._element))
					continue;
				address = slotIndex;
				return false;
			}
			int freeIndex;
			if ((freeIndex = _freeSlotIndex) != -0x1)
				_freeSlotIndex = _slots[slotIndex = freeIndex]._nextSlotIndex;
			else
				slotIndex = _usedSlotCount++;
			slots[slotIndex]._hash = relatedElement;
			slots[slotIndex]._nextSlotIndex = buckets[bucket] - 0x1;
			slots[slotIndex]._element = element;
			buckets[bucket] = slotIndex + 0x1;
			_count = count + 0x1;
			address = slotIndex;
			return true;
		}
		private int Add(T element)
		{
			if (!TryAdd(element, out int address))
				throw new ArgumentException(SubsetHelper.GetSubsetAlreadyContainsElementExceptionMessage(this, nameof(element)));
			return address;
		}
		private bool TryRemove(T element)
		{
			int[] buckets;
			int bucket = element.RelatedElement % (buckets = _bucketStartIndices).Length;
			Slot[] slots = _slots;
			int last = -0x1;
			for (int slotIndex = buckets[bucket] - 0x1; slotIndex != -0x1; last = slotIndex, slotIndex = slots[slotIndex]._nextSlotIndex)
			{
				if (element == null ? slots[slotIndex]._element != null : !element.Compare(slots[slotIndex]._element))
					continue;
				if (last == -0x1)
					buckets[bucket] = slots[slotIndex]._nextSlotIndex + 0x1;
				else
					slots[last]._nextSlotIndex = slots[slotIndex]._nextSlotIndex;
				slots[slotIndex]._hash = -0x1;
				slots[slotIndex]._nextSlotIndex = _freeSlotIndex;
				slots[slotIndex]._element = default;
				_freeSlotIndex = slotIndex;
				_count--;
				return true;
			}
			return false;
		}
		private void Remove(T element)
		{
			if (!TryRemove(element))
				throw new ArgumentException(SubsetHelper.GetSubsetDoesNotContainElementExceptionMessage(this, nameof(element)));
		}
		private bool TryRemoveAt(int address, out T element)
		{
			Slot[] slots;
			int relatedElement;
			if (address < 0x0 || address >= _usedSlotCount || (relatedElement = (slots = _slots)[address]._hash) == -0x1)
			{
				element = default;
				return false;
			}
			T elementBuffer = slots[address]._element;
			int[] buckets;
			int bucket = relatedElement % (buckets = _bucketStartIndices).Length;
			int last = -0x1;
			for (int slotIndex = buckets[bucket] - 0x1; ; last = slotIndex, slotIndex = slots[slotIndex]._nextSlotIndex)
			{
				if (elementBuffer == null ? slots[slotIndex]._element != null : !elementBuffer.Compare(slots[slotIndex]._element))
					continue;
				if (last == -0x1)
					buckets[bucket] = slots[slotIndex]._nextSlotIndex + 0x1;
				else
					slots[last]._nextSlotIndex = slots[slotIndex]._nextSlotIndex;
				slots[slotIndex]._hash = -0x1;
				slots[slotIndex]._nextSlotIndex = _freeSlotIndex;
				slots[slotIndex]._element = default;
				_freeSlotIndex = slotIndex;
				_count--;
				element = elementBuffer;
				return true;
			}
		}
		private T RemoveAt(int address)
		{
			if (!TryRemoveAt(address, out T element))
				throw new ArgumentException(SubsetHelper.GetSubsetDoesNotContainElementExceptionMessage(this));
			return element;
		}
		/// <summary>
		/// Gets an <see cref="Enumerator"/> of the <see cref="RandomAccessSubset{T}"/>.
		/// </summary>
		/// <returns>An <see cref="Enumerator"/> of the <see cref="RandomAccessSubset{T}"/>.</returns>
		public Enumerator GetEnumerator() => new Enumerator(this);
		/// <summary>
		/// Handles elements of the <see cref="RandomAccessSubset{T}"/> with an <see cref="ElementHandler{T}"/>.
		/// </summary>
		/// <param name="handler">The handler of the elements.</param>
		/// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
		public void Handle(ElementHandler<T> handler)
		{
			if (handler == null)
				throw new ArgumentNullException(nameof(handler));
			Slot[] slots = _slots;
			int usedSlotCount = _usedSlotCount;
			for (int slotIndex = 0x0; slotIndex != usedSlotCount; slotIndex++)
				if (slots[slotIndex]._hash != -0x1 && handler(slots[slotIndex]._element))
					return;
		}
		/// <summary>
		/// Determines whether the <see cref="RandomAccessSubset{T}"/> contains an element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><see langword="true"/> whether the <see cref="RandomAccessSubset{T}"/> contains <paramref name="element"/>; otherwise, <see langword="false"/>.</returns>
		public bool Contains(T element) => TryGetAddress(element, out _);
		/// <summary>
		/// Determines whether the <see cref="RandomAccessSubset{T}"/> contains an element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="address">The address of <paramref name="element"/> if the <see cref="RandomAccessSubset{T}"/> contains <paramref name="element"/>; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the <see cref="RandomAccessSubset{T}"/> contains <paramref name="element"/>; otherwise, <see langword="false"/>.</returns>
		public bool TryGetAddress(T element, out int address)
		{
			int relatedElement = element.RelatedElement;
			Slot[] slots = _slots;
			for (int slotIndex = _bucketStartIndices[relatedElement % slots.Length] - 0x1; slotIndex != -0x1; slotIndex = slots[slotIndex]._nextSlotIndex)
			{
				if (element == null ? slots[slotIndex]._element != null : !element.Compare(slots[slotIndex]._element))
					continue;
				address = slotIndex;
				return true;
			}
			address = default;
			return false;
		}
		/// <summary>
		/// Determines whether the <see cref="RandomAccessSubset{T}"/> contains an element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The address of the element.</returns>
		/// <exception cref="ArgumentException">The <see cref="RandomAccessSubset{T}"/> does not contain <paramref name="element"/>.</exception>
		public int GetAddress(T element)
		{
			if (!TryGetAddress(element, out int address))
				throw new ArgumentException(SubsetHelper.GetSubsetDoesNotContainElementExceptionMessage(this, nameof(element)));
			return address;
		}
		/// <summary>
		/// Determines whether the <see cref="RandomAccessSubset{T}"/> contains an element at an address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns><see langword="true"/> whether the <see cref="RandomAccessSubset{T}"/> contains the element; otherwise, <see langword="false"/>.</returns>
		public bool ContainsAt(int address) => address >= 0x0 && address < _usedSlotCount && _slots[address]._hash != -0x1;
		/// <summary>
		/// Determines whether the <see cref="RandomAccessSubset{T}"/> contains an element at an address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="element">The element if the <see cref="RandomAccessSubset{T}"/> contains; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the <see cref="RandomAccessSubset{T}"/> contains the element; otherwise, <see langword="false"/>.</returns>
		public bool TryGetAt(int address, out T element)
		{
			Slot[] slots;
			if (address < 0x0 || address >= _usedSlotCount || (slots = _slots)[address]._hash == -0x1)
			{
				element = default;
				return false;
			}
			element = slots[address]._element;
			return true;
		}
		IEnumerator<T> ISequence<T>.GetEnumerator() => GetEnumerator();
	}
}