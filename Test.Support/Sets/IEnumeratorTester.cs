using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Sets;
using System;

namespace Test.Support.Sets
{
	/// <summary>
	/// Represents the tester of <see cref="IEnumerator{T}"/> instances.
	/// </summary>
	static public class IEnumeratorTester
	{
		private struct CurrentGetTester<T>
		{
			private readonly IEnumerator<T> _instance;
			private T _current;

			internal CurrentGetTester(IEnumerator<T> instance)
			{
				_instance = instance;
				_current = default;
			}

			public T Current => _current;

			internal void Invoke() => _current = _instance.Current;
		}

		/// <summary>
		/// Tests an <see cref="IEnumerator{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of elements of the <see cref="IEnumerator{T}"/>.</typeparam>
		/// <param name="instance">The <see cref="IEnumerator{T}"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="instance"/> has not enumerated any elements.</exception>
		static public void TestIEnumerator<T>(this IEnumerator<T> instance)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			Action currentGetInvoker = new CurrentGetTester<T>(instance).Invoke;
			_ = Assert.ThrowsException<InvalidOperationException>(currentGetInvoker);
			bool iterated = false;
			while (instance.MoveNext())
			{
				_ = instance.Current;
				iterated = true;
			}
			if (!iterated)
				throw new ArgumentException(string.Format("{0} has not enumerated any elements.", instance));
			_ = Assert.ThrowsException<InvalidOperationException>(currentGetInvoker);
		}
	}
}