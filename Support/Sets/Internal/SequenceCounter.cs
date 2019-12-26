namespace Support.Sets.Internal
{
	internal sealed class SequenceCounter<T> where T : ISetElement<T>
	{
		private readonly ISequence<T> _sequence;
		private int _count;

		internal SequenceCounter(ISequence<T> sequence) => _sequence = sequence;

		internal int Count => _count;

		internal void Increase() => _count++;
	}
}