using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class StackSerializer<T> : Serializer<Stack<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<ISerializer<T>, StackSerializer<T>> _serializers;

		static StackSerializer() => _serializers = new Dictionary<ISerializer<T>, StackSerializer<T>>();

		private readonly ISerializer<T> _elementSerializer;
		private readonly Serializer<IEnumerable<T>> _iEnumerableSerializer;

		internal StackSerializer(ISerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iEnumerableSerializer = IEnumerableSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(Stack<T> instance) => _iEnumerableSerializer.Count(instance);
		public override sealed void Serialize(Stack<T> instance, byte[] buffer, ref int index) => _iEnumerableSerializer.Serialize(instance, buffer, ref index);
		public override sealed Stack<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			IEnumerableDeserializer<T> enumerator = new IEnumerableDeserializer<T>(_elementSerializer, count, buffer, index, out int length);
			Stack<T> instance = new Stack<T>(length);
			while (enumerator.MoveNext())
				instance.Push(enumerator.Current);
			return instance;
		}
	}
}