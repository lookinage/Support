using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Sets;
using System;

namespace Test.Support.Sets
{
	/// <summary>
	/// Represents the tester of <see cref="IRandomAccessSubset{T}"/> instances.
	/// </summary>
	static public class IRandomAccessSubsetTester
	{
		private struct GetAddressTester<T> where T : ISetElement<T>
		{
			private readonly IRandomAccessSubset<T> _instance;
			private T _element;
			private int _address;

			internal GetAddressTester(IRandomAccessSubset<T> instance, T element)
			{
				_instance = instance;
				_element = element;
				_address = default;
			}
			internal GetAddressTester(IRandomAccessSubset<T> instance) : this(instance, default) { }

			public T Element { get => _element; set => _element = value; }
			public int Address => _address;

			internal void Invoke() => _address = _instance.GetAddress(_element);
		}
		private struct GetAtTester<T> where T : ISetElement<T>
		{
			private readonly IRandomAccessSubset<T> _instance;
			private int _address;
			private T _element;

			internal GetAtTester(IRandomAccessSubset<T> instance, int address)
			{
				_instance = instance;
				_address = address;
				_element = default;
			}

			public int Address { get => _address; set => _address = value; }
			public T Element => _element;

			internal void Invoke() => _element = _instance[_address];
		}
		private struct AddTester<T> where T : ISetElement<T>
		{
			private readonly IRandomAccessSubsetEditor<T> _instance;
			private T _element;
			private int _address;

			internal AddTester(IRandomAccessSubsetEditor<T> instance, T element)
			{
				_instance = instance;
				_element = element;
				_address = default;
			}
			internal AddTester(IRandomAccessSubsetEditor<T> instance) : this(instance, default) { }

			public T Element { get => _element; set => _element = value; }
			public int Address => _address;

			internal void Invoke() => _address = _instance.Add(_element);
		}
		private struct RemoveTester<T> where T : ISetElement<T>
		{
			private readonly IRandomAccessSubsetEditor<T> _instance;
			private T _element;

			internal RemoveTester(IRandomAccessSubsetEditor<T> instance, T element)
			{
				_instance = instance;
				_element = element;
			}
			internal RemoveTester(IRandomAccessSubsetEditor<T> instance) : this(instance, default) { }

			public T Element { get => _element; set => _element = value; }

			internal void Invoke() => _instance.Remove(_element);
		}
		private struct RemoveAtTester<T> where T : ISetElement<T>
		{
			private readonly IRandomAccessSubsetEditor<T> _instance;
			private int _address;

			internal RemoveAtTester(IRandomAccessSubsetEditor<T> instance, int address)
			{
				_instance = instance;
				_address = address;
			}

			public int Address { get => _address; set => _address = value; }

			internal void Invoke() => _instance.RemoveAt(_address);
		}
		private struct ContentSequence : ISequence<Integer>
		{
			private struct Enumerator : IEnumerator<Integer>
			{
				private readonly ContentSequence _sequence;
				private readonly int _count;
				private int _index;
				private bool _disposed;

				internal Enumerator(ContentSequence sequence)
				{
					_sequence = sequence;
					_count = _sequence._addresses.Length;
					_index = -0x1;
					_disposed = default;
				}

				public Integer Current
				{
					get
					{
						if (_index < 0)
							ThrowEnumeratorIsNotStartedOrEnumerationIsOverException();
						return _index;
					}
				}

				private void ThrowEnumeratorIsNotStartedOrEnumerationIsOverException() => throw new InvalidOperationException(_index == -0x1 ? string.Format("The {0} is not started.", this) : string.Format("The {0} is over.", this));
				public void Dispose()
				{
					if (_disposed)
						return;
					_disposed = true;
				}
				public bool MoveNext()
				{
					if (_index == -0x2)
						return false;
					int?[] addresses = _sequence._addresses;
					int usedSlotCount = _count;
					int index;
					for (index = _index + 0x1; index != usedSlotCount && addresses[index] == null; index++) ;
					if (index == usedSlotCount)
					{
						_index = -0x2;
						return false;
					}
					_index = index;
					return true;
				}
			}

			private readonly int?[] _addresses;

			internal ContentSequence(int?[] addresses) => _addresses = addresses;

			public int Count
			{
				get
				{
					int count = 0x0;
					foreach (Integer element in this)
						count++;
					return count;
				}
			}

			public IEnumerator<Integer> GetEnumerator() => new Enumerator(this);
			public void Handle(ElementHandler<Integer> handler)
			{
				int?[] addresses = _addresses;
				for (int index = 0x0, count = _addresses.Length; index != count; index++)
					if (addresses[index] != null && handler(index))
						break;
			}
		}
		private struct ComplementSequence : ISequence<Integer>
		{
			private struct Enumerator : IEnumerator<Integer>
			{
				private readonly ComplementSequence _sequence;
				private readonly int _count;
				private int _index;
				private bool _disposed;

				internal Enumerator(ComplementSequence sequence)
				{
					_sequence = sequence;
					_count = _sequence._addresses.Length;
					_index = -0x1;
					_disposed = default;
				}

				public Integer Current
				{
					get
					{
						if (_index < 0)
							ThrowEnumeratorIsNotStartedOrEnumerationIsOverException();
						return _index;
					}
				}

				private void ThrowEnumeratorIsNotStartedOrEnumerationIsOverException() => throw new InvalidOperationException(_index == -0x1 ? string.Format("The {0} is not started.", this) : string.Format("The {0} is over.", this));
				public void Dispose()
				{
					if (_disposed)
						return;
					_disposed = true;
				}
				public bool MoveNext()
				{
					if (_index == -0x2)
						return false;
					int?[] addresses = _sequence._addresses;
					int usedSlotCount = _count;
					int index;
					for (index = _index + 0x1; index != usedSlotCount && addresses[index] != null; index++) ;
					if (index == usedSlotCount)
					{
						_index = -0x2;
						return false;
					}
					_index = index;
					return true;
				}
			}

			private readonly int?[] _addresses;

			internal ComplementSequence(int?[] addresses) => _addresses = addresses;

			public int Count
			{
				get
				{
					int count = 0x0;
					foreach (Integer element in this)
						count++;
					return count;
				}
			}

			public IEnumerator<Integer> GetEnumerator() => new Enumerator(this);
			public void Handle(ElementHandler<Integer> handler)
			{
				int?[] addresses = _addresses;
				for (int index = 0x0, count = _addresses.Length; index != count; index++)
					if (addresses[index] == null && handler(index))
						break;
			}
		}
		private struct EmptyAddressSequence : ISequence<int>
		{
			private struct Enumerator : IEnumerator<int>
			{
				private readonly EmptyAddressSequence _sequence;
				private readonly int _count;
				private int _index;
				private int _current;
				private bool _disposed;

				internal Enumerator(EmptyAddressSequence sequence)
				{
					_sequence = sequence;
					_count = _sequence._addresses.Length;
					_index = -0x1;
					_current = default;
					_disposed = default;
				}

				public int Current
				{
					get
					{
						if (_index < 0)
							ThrowEnumeratorIsNotStartedOrEnumerationIsOverException();
						return _current;
					}
				}

				private void ThrowEnumeratorIsNotStartedOrEnumerationIsOverException() => throw new InvalidOperationException(_index == -0x1 ? string.Format("The {0} is not started.", this) : string.Format("The {0} is over.", this));
				public void Dispose()
				{
					if (_disposed)
						return;
					_disposed = true;
				}
				public bool MoveNext()
				{
					if (_index == -0x2)
						return false;
					int?[] addresses = _sequence._addresses;
					int usedSlotCount = _count;
					int index;
					for (index = _index + 0x1; index != usedSlotCount && addresses[index] != null; index++) ;
					if (index == usedSlotCount)
					{
						_index = -0x2;
						return false;
					}
					_current = addresses[_index = index].Value;
					return true;
				}
			}

			private readonly int?[] _addresses;

			internal EmptyAddressSequence(int?[] addresses) => _addresses = addresses;

			public int Count
			{
				get
				{
					int count = 0x0;
					foreach (int element in this)
						count++;
					return count;
				}
			}

			public IEnumerator<int> GetEnumerator() => new Enumerator(this);
			public void Handle(ElementHandler<int> handler)
			{
				int?[] addresses = _addresses;
				for (int index = 0x0, count = _addresses.Length; index != count; index++)
					if (addresses[index] == null && handler(addresses[index].Value))
						break;
			}
		}

		/// <summary>
		/// Tests an <see cref="IRandomAccessSubset{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of elements of the <see cref="IRandomAccessSubsetEditor{T}"/>.</typeparam>
		/// <param name="instance">The instance of the <see cref="IRandomAccessSubset{T}"/>.</param>
		/// <param name="content">An <see cref="ISequence{T}"/> of elements that <paramref name="instance"/> contains.</param>
		/// <param name="complement">An <see cref="ISequence{T}"/> of elements that <paramref name="instance"/> does not contain.</param>
		/// <param name="emptyAddresses">An <see cref="ISequence{T}"/> of addresses of <paramref name="instance"/> that are empty.</param>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
		static public void TestIRandomAccessSubset<T>(this IRandomAccessSubset<T> instance, ISequence<T> content, ISequence<T> complement, ISequence<int> emptyAddresses) where T : ISetElement<T>
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			foreach (T element in content)
			{
				Assert.IsTrue(instance.TryGetAddress(element, out int gottenAddress));
				Assert.IsTrue(instance.ContainsAt(gottenAddress));
				Assert.IsTrue(instance.TryGetAt(gottenAddress, out T gottenElement));
				Assert.IsTrue(element == null ? gottenElement == null : element.Compare(gottenElement));
				Assert.IsTrue(element == null ? instance[gottenAddress] == null : element.Compare(instance[gottenAddress]));
				Assert.IsTrue(instance.ContainsAt(gottenAddress = instance.GetAddress(element)));
				Assert.IsTrue(instance.TryGetAt(gottenAddress, out gottenElement));
				Assert.IsTrue(element == null ? gottenElement == null : element.Compare(gottenElement));
				Assert.IsTrue(element == null ? instance[gottenAddress] == null : element.Compare(instance[gottenAddress]));
			}
			foreach (T element in complement)
			{
				Assert.IsFalse(instance.TryGetAddress(element, out int gottenAddress));
				_ = Assert.ThrowsException<ArgumentException>(new GetAddressTester<T>(instance, element).Invoke);
			}
			foreach (int address in emptyAddresses)
			{
				Assert.IsFalse(instance.ContainsAt(address));
				Assert.IsFalse(instance.TryGetAt(address, out T gottenElement));
				_ = Assert.ThrowsException<ArgumentException>(new GetAtTester<T>(instance, address).Invoke);
			}
		}
		/// <summary>
		/// Tests an <see cref="IRandomAccessSubset{T}"/>.
		/// </summary>
		/// <param name="instance">The <see cref="IRandomAccessSubset{T}"/>.</param>
		/// <param name="editor">The <see cref="IRandomAccessSubsetEditor{T}"/> of <paramref name="instance"/>.</param>
		/// <param name="iterationCount">The number of test iterations.</param>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="iterationCount"/> is less than 0.</exception>
		static public void TestIRandomAccessSubsetEditor(this IRandomAccessSubset<Integer> instance, IRandomAccessSubsetEditor<Integer> editor, int iterationCount)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			if (iterationCount < 0x0)
				throw new ArgumentOutOfRangeException(nameof(iterationCount));
			int maxCount;
			if ((maxCount = iterationCount >> 0x2) == 0x0)
				maxCount = iterationCount;
			int currentCount = 0x0;
			int?[] addresses = new int?[maxCount];
			for (int iterationIndex = 0x0; iterationIndex != iterationCount; iterationIndex++)
			{
				Integer element = PseudoRandomManager.GetInt32Remainder(maxCount);
				if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
				{
					if (instance.Contains(element))
					{
						if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
						{
							Assert.IsFalse(editor.TryAdd(element, out int gottenAddress));
							Assert.IsTrue(gottenAddress == addresses[element]);
						}
						else
						{
							AddTester<Integer> addTester;
							_ = Assert.ThrowsException<ArgumentException>((addTester = new AddTester<Integer>(editor, element)).Invoke);
						}
					}
					else
					{
						int gottenAddress;
						if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
							Assert.IsTrue(editor.TryAdd(element, out gottenAddress));
						else
							gottenAddress = editor.Add(element);
						addresses[element] = gottenAddress;
						currentCount++;
					}
				}
				else
				{
					if (addresses[element] == null || PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
					{
						if (instance.Contains(element))
						{
							if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
								Assert.IsTrue(editor.TryRemove(element));
							else
								editor.Remove(element);
							addresses[element] = null;
							currentCount--;
						}
						else
						{
							if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
								Assert.IsFalse(editor.TryRemove(element));
							else
								_ = Assert.ThrowsException<ArgumentException>(new RemoveTester<Integer>(editor, element).Invoke);
						}
					}
					else
					{
						if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
						{
							if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
							{
								Assert.IsTrue(editor.TryRemoveAt(addresses[element].Value, out Integer gottenElement));
								Assert.IsTrue(gottenElement == element);
							}
							else
								Assert.IsTrue(editor.TryRemoveAt(addresses[element].Value));
						}
						else
							Assert.IsTrue(editor.RemoveAt(addresses[element].Value) == element);
						addresses[element] = null;
						currentCount--;
					}
				}
				Assert.IsTrue(instance.Count == currentCount);
				instance.TestIRandomAccessSubset(new ContentSequence(addresses), new ComplementSequence(addresses), new EmptyAddressSequence(addresses));
			}
		}
	}
}