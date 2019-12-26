using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class KeyValuePairSerializerKeyConstantLength<TKey, TValue> : Serializer<KeyValuePair<TKey, TValue>>
	{
		internal struct Info
		{
			internal readonly IConstantLengthSerializer<TKey> _keySerializer;
			internal readonly ISerializer<TValue> _valueSerializer;

			public Info(IConstantLengthSerializer<TKey> keySerializer, ISerializer<TValue> valueSerializer)
			{
				_keySerializer = keySerializer;
				_valueSerializer = valueSerializer;
			}
		}

		[ThreadStatic]
		static internal readonly Dictionary<Info, KeyValuePairSerializerKeyConstantLength<TKey, TValue>> _serializers;

		static KeyValuePairSerializerKeyConstantLength() => _serializers = new Dictionary<Info, KeyValuePairSerializerKeyConstantLength<TKey, TValue>>();

		private readonly Info _info;

		internal KeyValuePairSerializerKeyConstantLength(Info info) => _info = info;

		public override sealed int Count(KeyValuePair<TKey, TValue> instance)
		{
			ValidateCount(instance);
			try { return checked(_info._keySerializer.Count + _info._valueSerializer.Count(instance.Value)); }
			catch (OverflowException exception) { throw new ArgumentException("An error occurred.", exception); }
		}
		public override sealed void Serialize(KeyValuePair<TKey, TValue> instance, byte[] buffer, ref int index)
		{
			ValidateSerialize(buffer, index);
			_info._keySerializer.Serialize(instance.Key, buffer, index);
			index += _info._keySerializer.Count;
			_info._valueSerializer.Serialize(instance.Value, buffer, ref index);
		}
		public override sealed KeyValuePair<TKey, TValue> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return new KeyValuePair<TKey, TValue>(_info._keySerializer.Deserialize(buffer, index), _info._valueSerializer.Deserialize(count - _info._keySerializer.Count, buffer, index + _info._keySerializer.Count));
		}
	}
}