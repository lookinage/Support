using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class ISetSerializer<T> : Serializer<ISet<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<ISerializer<T>, ISetSerializer<T>> _serializers;

		static ISetSerializer() => _serializers = new Dictionary<ISerializer<T>, ISetSerializer<T>>();

		private readonly ISerializer<T> _elementSerializer;
		private readonly Serializer<ICollection<T>> _iCollectionSerializer;

		internal ISetSerializer(ISerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iCollectionSerializer = ICollectionSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(ISet<T> instance) => _iCollectionSerializer.Count(instance);
		public override sealed void Serialize(ISet<T> instance, byte[] buffer, ref int index) => _iCollectionSerializer.Serialize(instance, buffer, ref index);
		public override sealed ISet<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return HashSetSerializer<T>.Deserialize(_elementSerializer, count, buffer, index);
		}
	}
}