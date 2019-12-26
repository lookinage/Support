namespace Support.Sets
{
	/// <summary>
	/// Represents an element of a quadratic set.
	/// </summary>
	/// <typeparam name="T">The type of elements of the set.</typeparam>
	public interface IQuadraticSetElement<T> : ILinearSetElement<T> where T : IQuadraticSetElement<T>
	{
		/// <summary>
		/// Compares the <see cref="IQuadraticSetElement{T}"/> with another <see cref="IQuadraticSetElement{T}"/>.
		/// </summary>
		/// <param name="element">Another <see cref="IQuadraticSetElement{T}"/> to compare with the <see cref="IQuadraticSetElement{T}"/>.</param>
		/// <returns>A <see cref="QuadraticComparisonResults"/> of these elements comparison.</returns>
		new QuadraticComparisonResults Compare(T element);
	}
}