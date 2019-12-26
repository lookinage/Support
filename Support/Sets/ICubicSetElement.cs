namespace Support.Sets
{
	/// <summary>
	/// Represents an element of a cubic set.
	/// </summary>
	/// <typeparam name="T">The type of elements of the set.</typeparam>
	public interface ICubicSetElement<T> : IQuadraticSetElement<T> where T : ICubicSetElement<T>
	{
		/// <summary>
		/// Compares the <see cref="ICubicSetElement{T}"/> with another <see cref="ICubicSetElement{T}"/>.
		/// </summary>
		/// <param name="element">Another <see cref="ICubicSetElement{T}"/> to compare with the <see cref="ICubicSetElement{T}"/>.</param>
		/// <returns>A <see cref="CubicComparisonResults"/> of these elements comparison.</returns>
		new CubicComparisonResults Compare(T element);
	}
}