namespace Support.Sets
{
	/// <summary>
	/// References a method that handles an element of a sequence.
	/// </summary>
	/// <typeparam name="T">The type of elements of the sequence.</typeparam>
	/// <param name="element">An element of the sequence.</param>
	/// <returns><see langword="true"/> whether the enumerating of the elements of the sequence is to be broken; otherwise, <see langword="false"/>.</returns>
	public delegate bool ElementHandler<in T>(T element);
}