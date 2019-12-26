namespace Support.Sets
{
	/// <summary>
	/// Represents a relation of elements from input and output sets.
	/// </summary>
	/// <typeparam name="TInput">The type of elements of the input set.</typeparam>
	/// <typeparam name="TOutput">The type of elements of the output set.</typeparam>
	public struct Relation<TInput, TOutput> : ISetElement<Relation<TInput, TOutput>> where TInput : ISetElement<TInput> where TOutput : ISetElement<TOutput>
	{
		/// <summary>
		/// The element of the input set.
		/// </summary>
		public TInput Input;
		/// <summary>
		/// The element of the output set.
		/// </summary>
		public TOutput Output;

		/// <summary>
		/// Initializes the <see cref="Relation{TInput, TOutput}"/>.
		/// </summary>
		/// <param name="input">The element of the input set.</param>
		/// <param name="output">The element of the output set.</param>
		public Relation(TInput input, TOutput output)
		{
			Input = input;
			Output = output;
		}

		/// <summary>
		/// Compares the <see cref="Relation{TInput, TOutput}"/> with another <see cref="Relation{TInput, TOutput}"/>.
		/// </summary>
		/// <param name="element">Another <see cref="Relation{TInput, TOutput}"/> to compare with the <see cref="Relation{TInput, TOutput}"/>.</param>
		/// <returns><see langword="true"/> whether the <see cref="Relation{TInput, TOutput}"/> equals to <paramref name="element"/>; otherwise, <see langword="false"/>.</returns>
		public bool Compare(Relation<TInput, TOutput> element) => (Input == null ? element.Input == null : Input.Compare(element.Input)) && (Output == null ? element.Output == null : Output.Compare(element.Output));
	}
}