using System;

namespace Support.Sets
{
	/// <summary>
	/// Represents an <see cref="ISubset{T}"/> that pseudo-randomly assigns elements their addresses.
	/// </summary>
	/// <typeparam name="T">The type of elements of the set.</typeparam>
	public interface IRandomAccessSubset<T> : ISubset<T> where T : ISetElement<T>
	{
		/// <summary>
		/// Gets an element at an address in the <see cref="IRandomAccessSubset{T}"/>.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <exception cref="ArgumentException">The <see cref="IRandomAccessSubset{T}"/> does not contain the element.</exception>
		T this[int address] { get; }

		/// <summary>
		/// Determines whether the <see cref="IRandomAccessSubset{T}"/> contains an element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="address">The address of <paramref name="element"/> if the <see cref="IRandomAccessSubset{T}"/> contains <paramref name="element"/>; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the <see cref="ISubset{T}"/> contains <paramref name="element"/>; otherwise, <see langword="false"/>.</returns>
		bool TryGetAddress(T element, out int address);
		/// <summary>
		/// Determines whether the <see cref="IRandomAccessSubset{T}"/> contains an element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns>The address of the element.</returns>
		/// <exception cref="ArgumentException">The <see cref="IRandomAccessSubset{T}"/> does not contain <paramref name="element"/>.</exception>
		int GetAddress(T element);
		/// <summary>
		/// Determines whether the <see cref="IRandomAccessSubset{T}"/> contains an element at an address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns><see langword="true"/> whether the <see cref="IRandomAccessSubset{T}"/> contains the element; otherwise, <see langword="false"/>.</returns>
		bool ContainsAt(int address);
		/// <summary>
		/// Determines whether the <see cref="IRandomAccessSubset{T}"/> contains an element at an address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="element">The element if the <see cref="IRandomAccessSubset{T}"/> contains; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the <see cref="IRandomAccessSubset{T}"/> contains the element; otherwise, <see langword="false"/>.</returns>
		bool TryGetAt(int address, out T element);
	}
}