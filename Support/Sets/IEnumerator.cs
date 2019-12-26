using System;

namespace Support.Sets
{
	/// <summary>
	/// Represents an enumerator of elements.
	/// </summary>
	/// <typeparam name="T">The type of elements of the enumerator.</typeparam>
	public interface IEnumerator<out T> : IDisposable
	{
		/// <summary>
		/// Gets the current element of the <see cref="IEnumerator{T}"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">The <see cref="IEnumerator{T}"/> is not started.</exception>
		/// <exception cref="InvalidOperationException">The <see cref="IEnumerator{T}"/> is over.</exception>
		T Current { get; }

		/// <summary>
		/// Sets the next element of the enumeration as current.
		/// </summary>
		/// <returns><see langword="true"/> whether the next element is set as current; otherwise, <see langword="false"/>.</returns>
		bool MoveNext();
	}
}