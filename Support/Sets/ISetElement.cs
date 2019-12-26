namespace Support.Sets
{
	/// <summary>
	/// Represents an element of a set.
	/// </summary>
	/// <typeparam name="T">The type of elements of the set.</typeparam>
	public interface ISetElement<T> where T : ISetElement<T>
	{
		/// <summary>
		/// Compares the <see cref="ISetElement{T}"/> with another <see cref="ISetElement{T}"/>.
		/// </summary>
		/// <param name="element">Another <see cref="ISetElement{T}"/> to compare with the <see cref="ISetElement{T}"/>.</param>
		/// <returns><see langword="true"/> whether the <see cref="ISetElement{T}"/> equals to <paramref name="element"/>; otherwise, <see langword="false"/>.</returns>
		bool Compare(T element);
	}
}