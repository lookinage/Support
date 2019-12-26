using System;

namespace Support.Sets
{
	/// <summary>
	/// Represents an editor of an <see cref="ISurjection{TInput, TOutput}"/>.
	/// </summary>
	/// <typeparam name="TInput">The type of elements of the input set.</typeparam>
	/// <typeparam name="TOutput">The type of elements of the output set.</typeparam>
	public interface ISurjectionEditor<TInput, TOutput> : ISubsetEditor<Relation<TInput, TOutput>> where TInput : ISetElement<TInput> where TOutput : ISetElement<TOutput>
	{
		/// <summary>
		/// Sets the output element that corresponds an input element.
		/// </summary>
		/// <param name="input">The input element.</param>
		/// <exception cref="ArgumentException">The <see cref="ISurjection{TInput, TOutput}"/> does not contain a <see cref="Relation{TInput, TOutput}"/> with <paramref name="input"/>.</exception>
		TOutput this[TInput input] { set; }

		/// <summary>
		/// Sets an output element that corresponds an input element if the <see cref="ISurjection{TInput, TOutput}"/> contains a <see cref="Relation{TInput, TOutput}"/> with the input element.
		/// </summary>
		/// <param name="input">The input element.</param>
		/// <param name="newOutput">The new output element.</param>
		/// <param name="oldOutput">The old output element if the <see cref="ISurjection{TInput, TOutput}"/> contains a <see cref="Relation{TInput, TOutput}"/> with <paramref name="input"/>; otherwise, the default value.</param>
		/// <returns><see langword="true"/> whether the <see cref="ISurjection{TInput, TOutput}"/> contains a <see cref="Relation{TInput, TOutput}"/> with <paramref name="input"/>; otherwise, <see langword="false"/>.</returns>
		bool TrySet(TInput input, TOutput newOutput, out TOutput oldOutput);
		/// <summary>
		/// Sets an output element that corresponds an input element if the <see cref="ISurjection{TInput, TOutput}"/> contains a <see cref="Relation{TInput, TOutput}"/> with the input element.
		/// </summary>
		/// <param name="input">The input element.</param>
		/// <param name="newOutput">The new output element.</param>
		/// <returns><see langword="true"/> whether the <see cref="ISurjection{TInput, TOutput}"/> contains a <see cref="Relation{TInput, TOutput}"/> with <paramref name="input"/>; otherwise, <see langword="false"/>.</returns>
		bool TrySet(TInput input, TOutput newOutput);
		/// <summary>
		/// Sets an output element that corresponds an input element.
		/// </summary>
		/// <param name="input">The input element.</param>
		/// <param name="newOutput">The new output element.</param>
		/// <param name="oldOutput">The old output element.</param>
		/// <exception cref="ArgumentException">The <see cref="ISurjection{TInput, TOutput}"/> does not contain a <see cref="Relation{TInput, TOutput}"/> with <paramref name="input"/>.</exception>
		void Set(TInput input, TOutput newOutput, out TOutput oldOutput);
	}
}