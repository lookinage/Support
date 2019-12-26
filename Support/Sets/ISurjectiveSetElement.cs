namespace Support.Sets
{
	/// <summary>
	/// Represents an element of a set that is related to another element of another set.
	/// </summary>
	/// <typeparam name="T">The type of elements of another set.</typeparam>
	public interface ISurjectiveSetElement<out T>
	{
		/// <summary>
		/// Gets the element the <see cref="ISurjectiveSetElement{T}"/> is related to.
		/// </summary>
		T RelatedElement { get; }
	}
}