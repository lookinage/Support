namespace Support.Sets
{
	/// <summary>
	/// Represents an editor of an <see cref="ISubset{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements of the set.</typeparam>
	public interface ISubsetEditor<T> where T : ISetElement<T>
	{
		/// <summary>
		/// Removes all elements from the <see cref="ISubset{T}"/>.
		/// </summary>
		void Clear();
	}
}