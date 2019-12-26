using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class QueueSerializerElementConstantLength<T> : Serializer<Queue<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<IConstantLengthSerializer<T>, QueueSerializerElementConstantLength<T>> _serializers;

		static QueueSerializerElementConstantLength() => _serializers = new Dictionary<IConstantLengthSerializer<T>, QueueSerializerElementConstantLength<T>>();

		private readonly IConstantLengthSerializer<T> _elementSerializer;
		private readonly Serializer<IEnumerable<T>> _iEnumerableSerializer;

		internal QueueSerializerElementConstantLength(IConstantLengthSerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iEnumerableSerializer = IEnumerableSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(Queue<T> instance)
		{
			ValidateCount(instance);
			try { return checked(_elementSerializer.Count * instance.Count); }
			catch (OverflowException exception) { throw new ArgumentException("An error occurred.", exception); }
		}
		public override sealed void Serialize(Queue<T> instance, byte[] buffer, ref int index) => _iEnumerableSerializer.Serialize(instance, buffer, ref index);
		public override sealed Queue<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			IEnumerableDeserializerElementConstantLength<T> enumerator = new IEnumerableDeserializerElementConstantLength<T>(_elementSerializer, count, buffer, index, out int length);
			Queue<T> instance = new Queue<T>(length);
			while (enumerator.MoveNext())
				instance.Enqueue(enumerator.Current);
			return instance;
		}
	}
}