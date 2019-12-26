using System;

namespace Support.Sets
{
	/// <summary>
	/// Represents a sequence of elements.
	/// </summary>
	/// <typeparam name="T">The type of elements of the sequence.</typeparam>
	public interface ISequence<out T>
	{
		/// <summary>
		/// Gets the number of elements of the <see cref="ISequence{T}"/>.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Gets an <see cref="IEnumerator{T}"/> of the <see cref="ISequence{T}"/>.
		/// </summary>
		/// <returns>An <see cref="IEnumerator{T}"/> of the <see cref="ISequence{T}"/>.</returns>
		IEnumerator<T> GetEnumerator();
		/// <summary>
		/// Handles elements of the <see cref="ISequence{T}"/> with an <see cref="ElementHandler{T}"/>.
		/// </summary>
		/// <param name="handler">The handler of the elements.</param>
		/// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
		void Handle(ElementHandler<T> handler);
	}
}