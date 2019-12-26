//using System;

//namespace Support.Sets
//{
//	/// <summary>
//	/// Represents an <see cref="ISurjection{TInput, TOutput}"/> that is sorted by input elements.
//	/// </summary>
//	/// <typeparam name="TInput">The type of elements of the input set.</typeparam>
//	/// <typeparam name="TOutput">The type of elements of the output set.</typeparam>
//	public interface ISortedSurjection<TInput, TOutput> : ISurjection<TInput, TOutput>
//	{
//		/// <summary>
//		/// Handles each relation of the <see cref="ISortedSurjection{TInput, TOutput}"/> from a specified input element with a specified <see cref="IElementHandler{T}"/>.
//		/// </summary>
//		/// <param name="handler">The handler of the relations.</param>
//		/// <param name="start">The starting element.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
//		void HandleEachFrom(IElementHandler<Relation<TInput, TOutput>> handler, TInput start);
//		/// <summary>
//		/// Handles each relation of the <see cref="ISortedSurjection{TInput, TOutput}"/> to a specified input element with a specified <see cref="IElementHandler{T}"/>.
//		/// </summary>
//		/// <param name="handler">The handler of the relations.</param>
//		/// <param name="start">The finite element.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
//		void HandleEachTo(IElementHandler<Relation<TInput, TOutput>> handler, TInput start);
//		/// <summary>
//		/// Determines the relation which input element is maximum and is not greater than a specified threshold.
//		/// </summary>
//		/// <param name="threshold">The threshold.</param>
//		/// <param name="input">The input element of the relation if the relation is found; otherwise, the default value.</param>
//		/// <param name="output">The output element of the relation if the relation is found; otherwise, the default value.</param>
//		/// <returns><see langword="true"/> if the <see cref="ISortedSurjection{TInput, TOutput}"/> contains the relation; otherwise, <see langword="false"/>.</returns>
//		bool TryGetMax(TInput threshold, out TInput input, out TOutput output);
//		/// <summary>
//		/// Determines the relation which input element is minimum and is not less than a specified threshold.
//		/// </summary>
//		/// <param name="threshold">The threshold.</param>
//		/// <param name="input">The input element of the relation if the relation is found; otherwise, the default value.</param>
//		/// <param name="output">The output element of the relation if the relation is found; otherwise, the default value.</param>
//		/// <returns><see langword="true"/> if the <see cref="ISortedSurjection{TInput, TOutput}"/> contains the relation; otherwise, <see langword="false"/>.</returns>
//		bool TryGetMin(TInput threshold, out TInput input, out TOutput output);
//	}
//}