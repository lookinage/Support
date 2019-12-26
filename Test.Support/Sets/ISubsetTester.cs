using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Sets;
using System;

namespace Test.Support.Sets
{
	/// <summary>
	/// Represents the tester of <see cref="ISubset{T}"/> instances.
	/// </summary>
	static public class ISubsetTester
	{
		internal struct AddingSequence : ISequence<Integer>
		{
			internal struct Enumerator : IEnumerator<Integer>
			{
				private readonly AddingSequence _instance;
				private int _index;
				private int _current;
				private bool _disposed;

				internal Enumerator(AddingSequence instance)
				{
					_instance = instance;
					_index = -0x1;
					_current = default;
					_disposed = false;
				}

				public Integer Current
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
					int index;
					if ((index = _index + 0x1) == _instance._count)
					{
						_index = -0x2;
						return false;
					}
					_index = index;
					_current = _instance._buffer[index];
					return true;
				}
			}

			private const int _maxAddCount = 0x10;

			private int[] _buffer;
			private int _count;

			public int Count => _count;

			internal void Randomize()
			{
				int count = _count = PseudoRandomManager.GetNonNegativeInt32(_maxAddCount);
				_ = ArrayHelper.EnsureLength(ref _buffer, count);
				int[] buffer = _buffer;
				for (int index = 0x0; index != count; index++)
					buffer[index] = PseudoRandomManager.GetInt32();
			}
			internal void RandomizeMoreThanItIsSpecified()
			{
				int count;
				_count = (count = PseudoRandomManager.GetNonNegativeInt32(_maxAddCount)) - 0x1;
				_ = ArrayHelper.EnsureLength(ref _buffer, count);
				int[] buffer = _buffer;
				for (int index = 0x0; index != count; index++)
					buffer[index] = PseudoRandomManager.GetInt32();
			}
			internal void RandomizeLessThanItIsSpecified()
			{
				int count;
				_count = (count = PseudoRandomManager.GetNonNegativeInt32(_maxAddCount)) + 0x1;
				_ = ArrayHelper.EnsureLength(ref _buffer, count);
				int[] buffer = _buffer;
				for (int index = 0x0; index != count; index++)
					buffer[index] = PseudoRandomManager.GetInt32();
			}

			public Enumerator GetEnumerator() => new Enumerator(this);
			public void Handle(ElementHandler<Integer> handler)
			{
				int[] buffer = _buffer;
				for (int index = 0x0, count = _count; index != count; index++)
					if (handler(buffer[index]))
						return;
			}
			IEnumerator<Integer> ISequence<Integer>.GetEnumerator() => GetEnumerator();
		}

		/// <summary>
		/// Tests an <see cref="ISubset{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of elements of the <see cref="ISubset{T}"/>.</typeparam>
		/// <param name="instance">The <see cref="ISubset{T}"/>.</param>
		/// <param name="content">An <see cref="ISequence{T}"/> of elements that <paramref name="instance"/> contains.</param>
		/// <param name="complement">An <see cref="ISequence{T}"/> of elements that <paramref name="instance"/> does not contain.</param>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="content"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="complement"/> is <see langword="null"/>.</exception>
		static public void TestISubset<T>(this ISubset<T> instance, ISequence<T> content, ISequence<T> complement) where T : ISetElement<T>
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			if (content == null)
				throw new ArgumentNullException(nameof(content));
			if (complement == null)
				throw new ArgumentNullException(nameof(complement));
			foreach (T element in content)
				Assert.IsTrue(instance.Contains(element));
			foreach (T element in complement)
				Assert.IsFalse(instance.Contains(element));
		}
		/// <summary>
		/// Tests an <see cref="ISubset{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of elements of the <see cref="ISubset{T}"/>.</typeparam>
		/// <param name="instance">The <see cref="ISubset{T}"/>.</param>
		/// <param name="editor">The <see cref="ISubsetEditor{T}"/> of <paramref name="instance"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
		static public void TestISubsetEditor<T>(this ISubset<T> instance, ISubsetEditor<T> editor) where T : ISetElement<T>
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			if (editor == null)
				throw new ArgumentNullException(nameof(editor));
			editor.Clear();
			Assert.IsTrue(instance.Count == 0x0);
		}
	}
}