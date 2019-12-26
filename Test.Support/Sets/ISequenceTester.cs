using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support;
using Support.Sets;
using System;

namespace Test.Support.Sets
{
	/// <summary>
	/// Represents the tester of <see cref="ISequence{T}"/> instances.
	/// </summary>
	static public class ISequenceTester
	{
		private struct HandlerTester<T>
		{
			private readonly ISequence<T> _instance;
			private readonly ElementHandler<T> _elementHandler;

			internal HandlerTester(ISequence<T> instance, ElementHandler<T> elementHandler)
			{
				_instance = instance;
				_elementHandler = elementHandler;
			}
			internal HandlerTester(ISequence<T> instance) : this(instance, default) { }

			internal void Invoke() => _instance.Handle(_elementHandler);
		}
		private sealed class ElementHandlerTester<T>
		{
			private int _count;
			internal readonly ElementHandler<T> _fullElementHandler;
			internal readonly ElementHandler<T> _notFullElementHandler;

			internal ElementHandlerTester(int count)
			{
				_count = count;
				_fullElementHandler = HandleElementFull;
				_notFullElementHandler = HandleElementNotFull;
			}

			internal int Count => _count;

			private bool HandleElementFull(T element)
			{
				_count--;
				return false;
			}
			private bool HandleElementNotFull(T element) => --_count == 0x0;
		}

		/// <summary>
		/// Tests an <see cref="ISequence{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of elements of the <see cref="ISequence{T}"/>.</typeparam>
		/// <param name="instance">The <see cref="ISequence{T}"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
		static public void TestISequence<T>(this ISequence<T> instance)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			int count = instance.Count;
			instance.GetEnumerator().TestIDisposable();
			instance.GetEnumerator().TestIEnumerator();
			int index = 0x1 + PseudoRandomManager.GetInt32Remainder(instance.Count);
			foreach (T element in instance)
				if (--index == 0x0)
					break;
			Assert.IsTrue(instance.Count == count);
			if (index != 0x0)
				Assert.Fail();
			index = instance.Count;
			foreach (T element in instance)
				index--;
			Assert.IsTrue(instance.Count == count);
			if (index != 0x0)
				Assert.Fail();
			_ = Assert.ThrowsException<ArgumentNullException>(new HandlerTester<T>(instance).Invoke);
			ElementHandlerTester<T> tester;
			instance.Handle((tester = new ElementHandlerTester<T>(PseudoRandomManager.GetInt32Remainder(instance.Count)))._notFullElementHandler);
			Assert.IsTrue(instance.Count == count);
			if (tester.Count != 0x0)
				Assert.Fail();
			instance.Handle((tester = new ElementHandlerTester<T>(instance.Count))._fullElementHandler);
			Assert.IsTrue(instance.Count == count);
			if (tester.Count != 0x0)
				Assert.Fail();
		}
	}
}