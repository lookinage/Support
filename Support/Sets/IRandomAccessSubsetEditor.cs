using System;

namespace Support.Sets
{
	/// <summary>
	/// Represents an editor of an <see cref="IRandomAccessSubset{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements of the set.</typeparam>
	public interface IRandomAccessSubsetEditor<T> : ISubsetEditor<T> where T : ISetElement<T>
	{
		/// <summary>
		/// Adds an element to the <see cref="IRandomAccessSubset{T}"/> if the <see cref="IRandomAccessSubset{T}"/> does not contain the element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="address">The address of the element in the <see cref="IRandomAccessSubset{T}"/>.</param>
		/// <returns><see langword="true"/> whether <paramref name="element"/> is added to the <see cref="IRandomAccessSubset{T}"/>; otherwise, <see langword="false"/>.</returns>
		/// <exception cref="InvalidOperationException">The <see cref="IRandomAccessSubset{T}"/> would be overflowed.</exception>
		bool TryAdd(T element, out int address);
		/// <summary>
		/// Adds an element to the <see cref="IRandomAccessSubset{T}"/>.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The address of the element in the <see cref="IRandomAccessSubset{T}"/>.</returns>
		/// <exception cref="InvalidOperationException">The <see cref="IRandomAccessSubset{T}"/> would be overflowed.</exception>
		/// <exception cref="ArgumentException">The <see cref="IRandomAccessSubset{T}"/> already contains the element.</exception>
		int Add(T element);
		/// <summary>
		/// Removes an element from the <see cref="IRandomAccessSubset{T}"/> if the <see cref="IRandomAccessSubset{T}"/> contains the element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><see langword="true"/> whether <paramref name="element"/> is removed from the <see cref="IRandomAccessSubset{T}"/>; otherwise, <see langword="false"/>.</returns>
		bool TryRemove(T element);
		/// <summary>
		/// Removes an element from the <see cref="IRandomAccessSubset{T}"/>.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <exception cref="ArgumentException">The <see cref="IRandomAccessSubset{T}"/> does not contain <paramref name="element"/>.</exception>
		void Remove(T element);
		/// <summary>
		/// Removes an element at an address from the <see cref="IRandomAccessSubset{T}"/> if the <see cref="IRandomAccessSubset{T}"/> contains the element.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="element">The element if the <see cref="IRandomAccessSubset{T}"/> contains; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the element is removed from the <see cref="IRandomAccessSubset{T}"/>; otherwise, <see langword="false"/>.</returns>
		bool TryRemoveAt(int address, out T element);
		/// <summary>
		/// Removes an element at an address from the <see cref="IRandomAccessSubset{T}"/> if the <see cref="IRandomAccessSubset{T}"/> contains the element.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns><see langword="true"/> whether the element is removed from the <see cref="IRandomAccessSubset{T}"/>; otherwise, <see langword="false"/>.</returns>
		bool TryRemoveAt(int address);
		/// <summary>
		/// Removes an element at an address from the <see cref="IRandomAccessSubset{T}"/>.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns>The element.</returns>
		/// <exception cref="ArgumentException">The <see cref="IRandomAccessSubset{T}"/> does not contain the element.</exception>
		T RemoveAt(int address);
	}
}