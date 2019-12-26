using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class DictionarySerializerElementConstantLength<TKey, TValue> : Serializer<Dictionary<TKey, TValue>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<IConstantLengthSerializer<KeyValuePair<TKey, TValue>>, DictionarySerializerElementConstantLength<TKey, TValue>> _serializers;

		static DictionarySerializerElementConstantLength() => _serializers = new Dictionary<IConstantLengthSerializer<KeyValuePair<TKey, TValue>>, DictionarySerializerElementConstantLength<TKey, TValue>>();

		private readonly IConstantLengthSerializer<KeyValuePair<TKey, TValue>> _elementSerializer;
		private readonly Serializer<ICollection<KeyValuePair<TKey, TValue>>> _iCollectionSerializer;

		internal DictionarySerializerElementConstantLength(IConstantLengthSerializer<KeyValuePair<TKey, TValue>> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iCollectionSerializer = ICollectionSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(Dictionary<TKey, TValue> instance) => _iCollectionSerializer.Count(instance);
		public override sealed void Serialize(Dictionary<TKey, TValue> instance, byte[] buffer, ref int index) => _iCollectionSerializer.Serialize(instance, buffer, ref index);
		public override sealed Dictionary<TKey, TValue> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			IEnumerableDeserializerElementConstantLength<KeyValuePair<TKey, TValue>> enumerator = new IEnumerableDeserializerElementConstantLength<KeyValuePair<TKey, TValue>>(_elementSerializer, count, buffer, index, out int length);
			Dictionary<TKey, TValue> instance = new Dictionary<TKey, TValue>(length);
			while (enumerator.MoveNext())
				instance.Add(enumerator.Current.Key, enumerator.Current.Value);
			return instance;
		}
	}
}