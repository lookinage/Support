using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class KeyValuePairSerializer<TKey, TValue> : Serializer<KeyValuePair<TKey, TValue>>
	{
		internal struct Info
		{
			internal readonly ISerializer<TKey> _keySerializer;
			internal readonly ISerializer<TValue> _valueSerializer;

			public Info(ISerializer<TKey> keySerializer, ISerializer<TValue> valueSerializer)
			{
				_keySerializer = keySerializer;
				_valueSerializer = valueSerializer;
			}
		}

		[ThreadStatic]
		static internal readonly Dictionary<Info, KeyValuePairSerializer<TKey, TValue>> _serializers;

		static KeyValuePairSerializer() => _serializers = new Dictionary<Info, KeyValuePairSerializer<TKey, TValue>>();

		private readonly Info _info;

		internal KeyValuePairSerializer(Info info) => _info = info;

		public override sealed int Count(KeyValuePair<TKey, TValue> instance)
		{
			ValidateCount(instance);
			try { return checked(Int32SerializerBuilder.Default.Count + _info._keySerializer.Count(instance.Key) + _info._valueSerializer.Count(instance.Value)); }
			catch (OverflowException exception) { throw new ArgumentException("An error occurred.", exception); }
		}
		public override sealed void Serialize(KeyValuePair<TKey, TValue> instance, byte[] buffer, ref int index)
		{
			ValidateSerialize(buffer, index);
			int startIndex = index += Int32SerializerBuilder.Default.Count;
			_info._keySerializer.Serialize(instance.Key, buffer, ref index);
			Int32SerializerBuilder.Default.Serialize(index - startIndex, buffer, startIndex - Int32SerializerBuilder.Default.Count);
			_info._valueSerializer.Serialize(instance.Value, buffer, ref index);
		}
		public override sealed KeyValuePair<TKey, TValue> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			int startIndex = index;
			return new KeyValuePair<TKey, TValue>(_info._keySerializer.Deserialize(Int32SerializerBuilder.Default.Deserialize(buffer, ref index), buffer, ref index), _info._valueSerializer.Deserialize(count - index + startIndex, buffer, index));
		}
	}
}