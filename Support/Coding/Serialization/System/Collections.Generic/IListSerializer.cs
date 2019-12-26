using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class IListSerializer<T> : Serializer<IList<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<ISerializer<T>, IListSerializer<T>> _serializers;

		static IListSerializer() => _serializers = new Dictionary<ISerializer<T>, IListSerializer<T>>();

		private readonly ISerializer<T> _elementSerializer;
		private readonly Serializer<ICollection<T>> _iCollectionSerializer;

		internal IListSerializer(ISerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iCollectionSerializer = ICollectionSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(IList<T> instance) => _iCollectionSerializer.Count(instance);
		public override sealed void Serialize(IList<T> instance, byte[] buffer, ref int index) => _iCollectionSerializer.Serialize(instance, buffer, ref index);
		public override sealed IList<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return ListSerializer<T>.Deserialize(_elementSerializer, count, buffer, index);
		}
	}
}