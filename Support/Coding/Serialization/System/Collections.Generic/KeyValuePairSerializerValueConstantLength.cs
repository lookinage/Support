using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class KeyValuePairSerializerValueConstantLength<TKey, TValue> : Serializer<KeyValuePair<TKey, TValue>>
	{
		internal struct Info
		{
			internal readonly ISerializer<TKey> _keySerializer;
			internal readonly IConstantLengthSerializer<TValue> _valueSerializer;

			public Info(ISerializer<TKey> keySerializer, IConstantLengthSerializer<TValue> valueSerializer)
			{
				_keySerializer = keySerializer;
				_valueSerializer = valueSerializer;
			}
		}

		[ThreadStatic]
		static internal readonly Dictionary<Info, KeyValuePairSerializerValueConstantLength<TKey, TValue>> _serializers;

		static KeyValuePairSerializerValueConstantLength() => _serializers = new Dictionary<Info, KeyValuePairSerializerValueConstantLength<TKey, TValue>>();

		private readonly Info _info;

		internal KeyValuePairSerializerValueConstantLength(Info info) => _info = info;

		public override sealed int Count(KeyValuePair<TKey, TValue> instance)
		{
			ValidateCount(instance);
			try { return checked(_info._valueSerializer.Count + _info._keySerializer.Count(instance.Key)); }
			catch (OverflowException exception) { throw new ArgumentException("An error occurred.", exception); }
		}
		public override sealed void Serialize(KeyValuePair<TKey, TValue> instance, byte[] buffer, ref int index)
		{
			ValidateSerialize(buffer, index);
			_info._valueSerializer.Serialize(instance.Value, buffer, index);
			index += _info._valueSerializer.Count;
			_info._keySerializer.Serialize(instance.Key, buffer, ref index);
		}
		public override sealed KeyValuePair<TKey, TValue> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return new KeyValuePair<TKey, TValue>(_info._keySerializer.Deserialize(count - _info._valueSerializer.Count, buffer, index + _info._valueSerializer.Count), _info._valueSerializer.Deserialize(buffer, index));
		}
	}
}