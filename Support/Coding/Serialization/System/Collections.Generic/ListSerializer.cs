using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class ListSerializer<T> : Serializer<List<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<ISerializer<T>, ListSerializer<T>> _serializers;

		static ListSerializer() => _serializers = new Dictionary<ISerializer<T>, ListSerializer<T>>();

		static internal List<T> Deserialize(ISerializer<T> elementSerializer, int count, byte[] buffer, int index)
		{
			IEnumerableDeserializer<T> enumerator = new IEnumerableDeserializer<T>(elementSerializer, count, buffer, index, out int length);
			List<T> instance = new List<T>(length);
			while (enumerator.MoveNext())
				instance.Add(enumerator.Current);
			return instance;
		}

		private readonly ISerializer<T> _elementSerializer;
		private readonly Serializer<IList<T>> _iListSerializer;

		internal ListSerializer(ISerializer<T> elementSerializer)
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