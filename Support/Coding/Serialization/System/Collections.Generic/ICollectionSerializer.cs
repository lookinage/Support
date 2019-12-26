using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class ICollectionSerializer<T> : Serializer<ICollection<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<ISerializer<T>, ICollectionSerializer<T>> _serializers;

		static ICollectionSerializer() => _serializers = new Dictionary<ISerializer<T>, ICollectionSerializer<T>>();

		private readonly ISerializer<T> _elementSerializer;
		private readonly Serializer<IEnumerable<T>> _iEnumerableSerializer;

		internal ICollectionSerializer(ISerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iEnumerableSerializer = IEnumerableSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(ICollection<T> instance) => _iEnumerableSerializer.Count(instance);
		public override sealed void Serialize(ICollection<T> instance, byte[] buffer, ref int index) => _iEnumerableSerializer.Serialize(instance, buffer, ref index);
		public override sealed ICollection<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return ArraySerializer<T>.Deserialize(_elementSerializer, count, buffer, index);
		}
	}
}