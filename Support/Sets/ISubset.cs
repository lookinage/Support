namespace Support.Sets
{
	/// <summary>
	/// Represents a subset of elements of a set.
	/// </summary>
	/// <typeparam name="T">The type of elements of the set.</typeparam>
	public interface ISubset<T> : ISequence<T> where T : ISetElement<T>
	{
		/// <summary>
		/// Determines whether the <see cref="ISubset{T}"/> contains an element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <returns><see langword="true"/> whether the <see cref="ISubset{T}"/> contains <paramref name="element"/>; otherwise, <see langword="false"/>.</returns>
		bool Contains(T element);
	}
}