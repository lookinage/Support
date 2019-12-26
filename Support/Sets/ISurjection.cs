using System;

namespace Support.Sets
{
	/// <summary>
	/// Represents a surjective correspondence function implemented as a <see cref="ISubset{T}"/> of <see cref="Relation{TInput, TOutput}"/> instances.
	/// </summary>
	/// <typeparam name="TInput">The type of elements of the input set.</typeparam>
	/// <typeparam name="TOutput">The type of elements of the output set.</typeparam>
	public interface ISurjection<TInput, TOutput> : ISubset<Relation<TInput, TOutput>> where TInput : ISetElement<TInput> where TOutput : ISetElement<TOutput>
	{
		/// <summary>
		/// Gets the output element that corresponds an input element.
		/// </summary>
		/// <param name="input">The input element.</param>
		/// <exception cref="ArgumentException">The <see cref="ISurjection{TInput, TOutput}"/> does not contain a <see cref="Relation{TInput, TOutput}"/> with <paramref name="input"/>.</exception>
		TOutput this[TInput input] { get; }

		/// <summary>
		/// Determines whether the <see cref="ISurjection{TInput, TOutput}"/> contains a relation with an input element.
		/// </summary>
		/// <param name="input">The input element.</param>
		/// <returns><see langword="true"/> whether the <see cref="ISurjection{TInput, TOutput}"/> contains a <see cref="Relation{TInput, TOutput}"/> with <paramref name="input"/>; otherwise, <see langword="false"/>.</returns>
		bool Contains(TInput input);
		/// <summary>
		/// Gets the output element that corresponds an input element if the <see cref="ISurjection{TInput, TOutput}"/> contains a <see cref="Relation{TInput, TOutput}"/> with the input element.
		/// </summary>
		/// <param name="input">The input element.</param>
		/// <param name="output">The output element if the <see cref="ISurjection{TInput, TOutput}"/> contains a <see cref="Relation{TInput, TOutput}"/> with <paramref name="input"/>; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the <see cref="ISurjection{TInput, TOutput}"/> contains a <see cref="Relation{TInput, TOutput}"/> with <paramref name="input"/>; otherwise, <see langword="false"/>.</returns>
		bool TryGet(TInput input, out TOutput output);
	}
}