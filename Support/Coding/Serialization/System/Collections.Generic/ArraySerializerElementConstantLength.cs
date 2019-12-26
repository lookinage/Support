using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class ArraySerializerElementConstantLength<T> : Serializer<T[]>
	{
		[ThreadStatic]
		static internal readonly Dictionary<IConstantLengthSerializer<T>, ArraySerializerElementConstantLength<T>> _serializers;

		static ArraySerializerElementConstantLength() => _serializers = new Dictionary<IConstantLengthSerializer<T>, ArraySerializerElementConstantLength<T>>();

		static internal T[] Deserialize(IConstantLengthSerializer<T> elementSerializer, int count, byte[] buffer, int index)
		{
			IEnumerableDeserializerElementConstantLength<T> enumerator = new IEnumerableDeserializerElementConstantLength<T>(elementSerializer, count, buffer, index, out int length);
			T[] instance = new T[length];
			for (int i = 0x0; enumerator.MoveNext(); i++)
				instance[i] = enumerator.Current;
			return instance;
		}

		private readonly IConstantLengthSerializer<T> _elementSerializer;
		private readonly Serializer<IList<T>> _iListSerializer;

		internal ArraySerializerElementConstantLength(IConstantLengthSerializer<T> elementSerializer)
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