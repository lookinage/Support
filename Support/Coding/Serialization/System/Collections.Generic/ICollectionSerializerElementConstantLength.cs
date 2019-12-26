using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class ICollectionSerializerElementConstantLength<T> : Serializer<ICollection<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<IConstantLengthSerializer<T>, ICollectionSerializerElementConstantLength<T>> _serializers;

		static ICollectionSerializerElementConstantLength() => _serializers = new Dictionary<IConstantLengthSerializer<T>, ICollectionSerializerElementConstantLength<T>>();

		private readonly IConstantLengthSerializer<T> _elementSerializer;
		private readonly Serializer<IEnumerable<T>> _iEnumerableSerializer;

		internal ICollectionSerializerElementConstantLength(IConstantLengthSerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iEnumerableSerializer = IEnumerableSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(ICollection<T> instance)
		{
			ValidateCount(instance);
			try { return checked(_elementSerializer.Count * instance.Count); }
			catch (OverflowException exception) { throw new ArgumentException("An error occurred.", exception); }
		}
		public override sealed void Serialize(ICollection<T> instance, byte[] buffer, ref int index) => _iEnumerableSerializer.Serialize(instance, buffer, ref index);
		public override sealed ICollection<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return ArraySerializerElementConstantLength<T>.Deserialize(_elementSerializer, count, buffer, index);
		}
	}
}