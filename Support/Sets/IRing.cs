using System;

namespace Support.Sets
{
	/// <summary>
	/// Represents an <see cref="ISurjection{TInput, TOutput}"/> that represents a ring of elements of output set each of those has the own non-negative integer offset (element of input set) from the beginning of the ring.
	/// </summary>
	/// <typeparam name="T">The type of elements of the output set.</typeparam>
	public interface IRing<T> : ISurjection<Integer, T> where T : ISetElement<T>
	{
		/// <summary>
		/// Gets the first element of the <see cref="IRing{T}"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> is empty.</exception>
		T First { get; }
		/// <summary>
		/// Gets the last element of the <see cref="IRing{T}"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">The <see cref="IRing{T}"/> is empty.</exception>
		T Last { get; }

		/// <summary>
		/// Gets the first element of the <see cref="IRing{T}"/> if the <see cref="IRing{T}"/> is not empty.
		/// </summary>
		/// <param name="element">The element if the <see cref="IRing{T}"/> is not empty; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the element is gotten; otherwise, <see langword="false"/>.</returns>
		bool TryGetFirst(out T element);
		/// <summary>
		/// Gets the last element of the <see cref="IRing{T}"/> if the <see cref="IRing{T}"/> is not empty.
		/// </summary>
		/// <param name="element">The element if the <see cref="IRing{T}"/> is not empty; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the element is gotten; otherwise, <see langword="false"/>.</returns>
		bool TryGetLast(out T element);
	}
}