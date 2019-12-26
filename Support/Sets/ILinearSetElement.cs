namespace Support.Sets
{
	/// <summary>
	/// Represents an element of a linear set.
	/// </summary>
	/// <typeparam name="T">The type of elements of the set.</typeparam>
	public interface ILinearSetElement<T> : ISetElement<T> where T : ILinearSetElement<T>
	{
		/// <summary>
		/// Compares the <see cref="ILinearSetElement{T}"/> with another <see cref="ILinearSetElement{T}"/>.
		/// </summary>
		/// <param name="element">Another <see cref="ILinearSetElement{T}"/> to compare with the <see cref="ILinearSetElement{T}"/>.</param>
		/// <returns>A <see cref="LinearComparisonResults"/> of these elements comparison.</returns>
		new LinearComparisonResults Compare(T element);
	}
}