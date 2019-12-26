using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class ListSerializerElementConstantLength<T> : Serializer<List<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<IConstantLengthSerializer<T>, ListSerializerElementConstantLength<T>> _serializers;

		static ListSerializerElementConstantLength() => _serializers = new Dictionary<IConstantLengthSerializer<T>, ListSerializerElementConstantLength<T>>();

		static internal List<T> Deserialize(IConstantLengthSerializer<T> elementSerializer, int count, byte[] buffer, int index)
		{
			IEnumerableDeserializerElementConstantLength<T> enumerator = new IEnumerableDeserializerElementConstantLength<T>(elementSerializer, count, buffer, index, out int length);
			List<T> instance = new List<T>(length);
			while (enumerator.MoveNext())
				instance.Add(enumerator.Current);
			return instance;
		}

		private readonly IConstantLengthSerializer<T> _elementSerializer;
		private readonly Serializer<IList<T>> _iListSerializer;

		internal ListSerializerElementConstantLength(IConstantLengthSerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iListSerializer = IListSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(List<T> instance) => _iListSerializer.Count(instance);
		public override sealed void Serialize(List<T> instance, byte[] buffer, ref int index) => _iListSerializer.Serialize(instance, buffer, ref index);
		public override sealed List<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return Deserialize(_elementSerializer, count, buffer, index);
		}
	}
}