using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class IListSerializerElementConstantLength<T> : Serializer<IList<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<IConstantLengthSerializer<T>, IListSerializerElementConstantLength<T>> _serializers;

		static IListSerializerElementConstantLength() => _serializers = new Dictionary<IConstantLengthSerializer<T>, IListSerializerElementConstantLength<T>>();

		private readonly IConstantLengthSerializer<T> _elementSerializer;
		private readonly Serializer<ICollection<T>> _iCollectionSerializer;

		internal IListSerializerElementConstantLength(IConstantLengthSerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iCollectionSerializer = ICollectionSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(IList<T> instance) => _iCollectionSerializer.Count(instance);
		public override sealed void Serialize(IList<T> instance, byte[] buffer, ref int index) => _iCollectionSerializer.Serialize(instance, buffer, ref index);
		public override sealed IList<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return ListSerializerElementConstantLength<T>.Deserialize(_elementSerializer, count, buffer, index);
		}
	}
}