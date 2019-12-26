using System;

namespace Support.Sets
{
	/// <summary>
	/// Represents an editor of an <see cref="IRing{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements of the output set.</typeparam>
	public interface IRingEditor<T> : ISurjectionEditor<Integer, T> where T : ISetElement<T>
	{
		/// <summary>
		/// Inserts an element at an offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="element">The element.</param>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> would be overflowed.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the number of elements of the <see cref="IRing{T}"/>.</exception>
		void Insert(Integer offset, T element);
		/// <summary>
		/// Inserts the elements of an <see cref="ISequence{T}"/> so that each subsequent element has an offset that is greater than the previous element's one.
		/// </summary>
		/// <param name="offset">The offset of the first element of the <see cref="ISequence{T}"/>.</param>
		/// <param name="sequence">The <see cref="ISequence{T}"/>.</param>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> would be overflowed.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the number of elements of the <see cref="IRing{T}"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="sequence"/> does not contain elements as many as it is specified.</exception>
		void InsertDirectly(Integer offset, ISequence<T> sequence);
		/// <summary>
		/// Inserts the elements of an <see cref="ISequence{T}"/> so that each subsequent element has an offset that is less than the previous element's one.
		/// </summary>
		/// <param name="offset">The offset of the last element of the <see cref="ISequence{T}"/>.</param>
		/// <param name="sequence">The <see cref="ISequence{T}"/>.</param>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> would be overflowed.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the number of elements of the <see cref="IRing{T}"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="sequence"/> does not contain elements as many as it is specified.</exception>
		void InsertInversely(Integer offset, ISequence<T> sequence);
		/// <summary>
		/// Adds an element to the beginning of the <see cref="IRing{T}"/>.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> would be overflowed.</exception>
		void AddFirst(T element);
		/// <summary>
		/// Adds an element to the end of the <see cref="IRing{T}"/>.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> would be overflowed.</exception>
		void AddLast(T element);
		/// <summary>
		/// Adds the elements of an <see cref="ISequence{T}"/> to the beginning of the <see cref="IRing{T}"/> so that each subsequent element has an offset that is less than the previous element's one.
		/// </summary>
		/// <param name="sequence">The <see cref="ISequence{T}"/>.</param>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> would be overflowed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="sequence"/> does not contain elements as many as it is specified.</exception>
		void AddFirstDirectly(ISequence<T> sequence);
		/// <summary>
		/// Adds the elements of an <see cref="ISequence{T}"/> to the beginning of the <see cref="IRing{T}"/> so that each subsequent element has an offset that is greater than the previous element's one.
		/// </summary>
		/// <param name="sequence">The <see cref="ISequence{T}"/>.</param>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> would be overflowed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="sequence"/> does not contain elements as many as it is specified.</exception>
		void AddFirstInversely(ISequence<T> sequence);
		/// <summary>
		/// Adds the elements of an <see cref="ISequence{T}"/> to the end of the <see cref="IRing{T}"/> so that each subsequent element has an offset that is greater than the previous element's one.
		/// </summary>
		/// <param name="sequence">The <see cref="ISequence{T}"/>.</param>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> would be overflowed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="sequence"/> does not contain elements as many as it is specified.</exception>
		void AddLastDirectly(ISequence<T> sequence);
		/// <summary>
		/// Adds the elements of an <see cref="ISequence{T}"/> to the end of the <see cref="IRing{T}"/> so that each subsequent element has an offset that is less than the previous element's one.
		/// </summary>
		/// <param name="sequence">The <see cref="ISequence{T}"/>.</param>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> would be overflowed.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="sequence"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="sequence"/> does not contain elements as many as it is specified.</exception>
		void AddLastInversely(ISequence<T> sequence);
		/// <summary>
		/// Removes the element from the beginning of the <see cref="IRing{T}"/> if the <see cref="IRing{T}"/> is not empty.
		/// </summary>
		/// <param name="element">The element from the beginning of the <see cref="IRing{T}"/> whether it is removed; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the element is removed; otherwise, <see langword="false"/>.</returns>
		bool TryRemoveFirst(out T element);
		/// <summary>
		/// Removes the element from the end of the <see cref="IRing{T}"/> if the <see cref="IRing{T}"/> is not empty.
		/// </summary>
		/// <param name="element">The element from the end of the <see cref="IRing{T}"/> whether it is removed; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the element is removed; otherwise, <see langword="false"/>.</returns>
		bool TryRemoveLast(out T element);
		/// <summary>
		/// Removes the element from the beginning of the <see cref="IRing{T}"/>.
		/// </summary>
		/// <returns>The element from the beginning of the <see cref="IRing{T}"/>.</returns>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> is empty.</exception>
		T RemoveFirst();
		/// <summary>
		/// Removes the element from the end of the <see cref="IRing{T}"/>.
		/// </summary>
		/// <returns>The element from the end of the <see cref="IRing{T}"/>.</returns>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> is empty.</exception>
		T RemoveLast();
	}
}