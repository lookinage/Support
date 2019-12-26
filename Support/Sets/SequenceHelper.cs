using System;

namespace Support.Sets
{
	/// <summary>
	/// Provides methods for sequences manipulation.
	/// </summary>
	static public class SequenceHelper
	{
		/// <summary>
		/// Represents a sequence of converted elements of another sequence.
		/// </summary>
		/// <typeparam name="TSource">The type of elements of the source sequence.</typeparam>
		/// <typeparam name="TResult">The type of elements of the result sequence.</typeparam>
		public struct ConvertedSequence<TSource, TResult> : ISequence<TResult>
		{
			/// <summary>
			/// Represents an enumerator of converted elements.
			/// </summary>
			public struct Enumerator : IEnumerator<TResult>
			{
				private readonly ConvertedSequence<TSource, TResult> _sequence;
				private readonly IEnumerator<TSource> _enumerator;

				internal Enumerator(ConvertedSequence<TSource, TResult> selector)
				{
					_sequence = selector;
					_enumerator = selector._sequence.GetEnumerator();
				}

				/// <summary>
				/// Gets the current element of the <see cref="Enumerator"/>.
				/// </summary>
				/// <exception cref="InvalidOperationException">The <see cref="Enumerator"/> is not started.</exception>
				/// <exception cref="InvalidOperationException">The <see cref="Enumerator"/> is over.</exception>
				public TResult Current => _sequence._converter(_enumerator.Current);

				/// <summary>
				/// Releases all resources used by the <see cref="Enumerator"/>.
				/// </summary>
				public void Dispose() => _enumerator.Dispose();
				/// <summary>
				/// Sets the next element of the sequence as current.
				/// </summary>
				/// <returns><see langword="true"/> whether the next element is set as current; otherwise, <see langword="false"/>.</returns>
				public bool MoveNext() => _enumerator.MoveNext();
			}

			private readonly ISequence<TSource> _sequence;
			private readonly Func<TSource, TResult> _converter;

			/// <summary>
			/// Gets the number of elements of the <see cref="ConvertedSequence{TSource, TResult}"/>.
			/// </summary>
			public int Count => _sequence.Count;

			internal ConvertedSequence(ISequence<TSource> sequence, Func<TSource, TResult> converter)
			{
				_sequence = sequence;
				_converter = converter;
			}

			/// <summary>
			/// Gets an <see cref="Enumerator"/> of the <see cref="ConvertedSequence{TSource, TResult}"/>.
			/// </summary>
			/// <returns>An <see cref="Enumerator"/> of the <see cref="ConvertedSequence{TSource, TResult}"/>.</returns>
			public Enumerator GetEnumerator() => new Enumerator(this);
			/// <summary>
			/// Handles elements of the <see cref="ConvertedSequence{TSource, TResult}"/> with an <see cref="ElementHandler{T}"/>.
			/// </summary>
			/// <param name="handler">The handler of the elements.</param>
			/// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
			public void Handle(ElementHandler<TResult> handler)
			{
				if (handler == null)
					throw new ArgumentNullException(nameof(handler));
				Func<TSource, TResult> selector = _converter;
				_sequence.Handle(a => handler(selector(a)));
			}
			IEnumerator<TResult> ISequence<TResult>.GetEnumerator() => GetEnumerator();
		}

		/// <summary>
		/// Gets a <see cref="ConvertedSequence{TSource, TResult}"/> of elements of the <see cref="ISequence{T}"/>.
		/// </summary>
		/// <typeparam name="TSource">The type of elements of the source sequence.</typeparam>
		/// <typeparam name="TResult">The type of elements of the result sequence.</typeparam>
		/// <param name="sequence">The source sequence.</param>
		/// <param name="converter">The converter of the elements of the source sequence.</param>
		/// <returns>A <see cref="ConvertedSequence{TSource, TResult}"/> of elements of the <see cref="ISequence{T}"/>.</returns>
		static public ConvertedSequence<TSource, TResult> GetConvertedSequence<TSource, TResult>(this ISequence<TSource> sequence, Func<TSource, TResult> converter) => new ConvertedSequence<TSource, TResult>(sequence, converter);
	}
}