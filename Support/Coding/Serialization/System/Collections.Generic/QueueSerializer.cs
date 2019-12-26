using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class QueueSerializer<T> : Serializer<Queue<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<ISerializer<T>, QueueSerializer<T>> _serializers;

		static QueueSerializer() => _serializers = new Dictionary<ISerializer<T>, QueueSerializer<T>>();

		private readonly ISerializer<T> _elementSerializer;
		private readonly Serializer<IEnumerable<T>> _iEnumerableSerializer;

		internal QueueSerializer(ISerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iEnumerableSerializer = IEnumerableSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(Queue<T> instance) => _iEnumerableSerializer.Count(instance);
		public override sealed void Serialize(Queue<T> instance, byte[] buffer, ref int index) => _iEnumerableSerializer.Serialize(instance, buffer, ref index);
		public override sealed Queue<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			IEnumerableDeserializer<T> enumerator = new IEnumerableDeserializer<T>(_elementSerializer, count, buffer, index, out int length);
			Queue<T> instance = new Queue<T>(length);
			while (enumerator.MoveNext())
				instance.Enqueue(enumerator.Current);
			return instance;
		}
	}
}