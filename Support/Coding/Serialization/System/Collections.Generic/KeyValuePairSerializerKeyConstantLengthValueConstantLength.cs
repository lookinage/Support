using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class KeyValuePairSerializerKeyConstantLengthValueConstantLength<TKey, TValue> : ConstantLengthSerializer<KeyValuePair<TKey, TValue>>
	{
		internal struct Info
		{
			internal readonly IConstantLengthSerializer<TKey> _keySerializer;
			internal readonly IConstantLengthSerializer<TValue> _valueSerializer;

			public Info(IConstantLengthSerializer<TKey> keySerializer, IConstantLengthSerializer<TValue> valueSerializer)
			{
				_keySerializer = keySerializer;
				_valueSerializer = valueSerializer;
			}
		}

		[ThreadStatic]
		static internal readonly Dictionary<Info, KeyValuePairSerializerKeyConstantLengthValueConstantLength<TKey, TValue>> _serializers;

		static KeyValuePairSerializerKeyConstantLengthValueConstantLength() => _serializers = new Dictionary<Info, KeyValuePairSerializerKeyConstantLengthValueConstantLength<TKey, TValue>>();

		private readonly Info _info;

		internal KeyValuePairSerializerKeyConstantLengthValueConstantLength(Info info) : base(info._keySerializer.Count + info._valueSerializer.Count) => _info = info;

		public override sealed void Serialize(KeyValuePair<TKey, TValue> instance, byte[] buffer, int index)
		{
			ValidateSerialize(buffer, index);
			_info._keySerializer.Serialize(instance.Key, buffer, index);
			_info._valueSerializer.Serialize(instance.Value, buffer, index + _info._keySerializer.Count);
		}
		public override sealed KeyValuePair<TKey, TValue> Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			return new KeyValuePair<TKey, TValue>(_info._keySerializer.Deserialize(buffer, index), _info._valueSerializer.Deserialize(buffer, index + _info._keySerializer.Count));
		}
	}
}