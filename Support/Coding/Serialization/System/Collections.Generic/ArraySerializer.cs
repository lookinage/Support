using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class ArraySerializer<T> : Serializer<T[]>
	{
		[ThreadStatic]
		static internal readonly Dictionary<ISerializer<T>, ArraySerializer<T>> _serializers;

		static ArraySerializer() => _serializers = new Dictionary<ISerializer<T>, ArraySerializer<T>>();

		static internal T[] Deserialize(ISerializer<T> elementSerializer, int count, byte[] buffer, int index)
		{
			IEnumerableDeserializer<T> enumerator = new IEnumerableDeserializer<T>(elementSerializer, count, buffer, index, out int length);
			T[] instance = new T[length];
			for (int i = 0x0; enumerator.MoveNext(); i++)
				instance[i] = enumerator.Current;
			return instance;
		}

		private readonly ISerializer<T> _elementSerializer;
		private readonly Serializer<IList<T>> _iListSerializer;

		internal ArraySerializer(ISerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iListSerializer = IListSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(T[] instance) => _iListSerializer.Count(instance);
		public override sealed void Serialize(T[] instance, byte[] buffer, ref int index) => _iListSerializer.Serialize(instance, buffer, ref index);
		public override sealed T[] Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return Deserialize(_elementSerializer, count, buffer, index);
		}
	}
}