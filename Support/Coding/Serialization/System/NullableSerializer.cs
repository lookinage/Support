using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System
{
	internal sealed class NullableSerializer<T> : Serializer<Nullable<T>> where T : struct
	{
		[ThreadStatic]
		static internal readonly Dictionary<ISerializer<T>, NullableSerializer<T>> _serializers;

		static NullableSerializer() => _serializers = new Dictionary<ISerializer<T>, NullableSerializer<T>>();

		internal readonly ISerializer<T> _underlyingSerializer;

		internal NullableSerializer(ISerializer<T> underlyingSerializer) => _underlyingSerializer = underlyingSerializer;

		public override sealed int Count(Nullable<T> instance)
		{
			try { return instance.HasValue ? checked(BooleanSerializerBuilder.Default.Count + _underlyingSerializer.Count(instance.Value)) : BooleanSerializerBuilder.Default.Count; }
			catch (OverflowException exception) { throw new ArgumentException("An error occurred.", exception); }
		}
		public override sealed void Serialize(Nullable<T> instance, byte[] buffer, ref int index)
		{
			ValidateSerialize(buffer, index);
			if (!instance.HasValue)
			{
				BooleanSerializerBuilder.Default.Serialize(false, buffer, ref index);
				return;
			}
			BooleanSerializerBuilder.Default.Serialize(true, buffer, ref index);
			_underlyingSerializer.Serialize(instance.Value, buffer, ref index);
		}
		public override sealed Nullable<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return BooleanSerializerBuilder.Default.Deserialize(buffer, index) ? new Nullable<T>(_underlyingSerializer.Deserialize(count - BooleanSerializerBuilder.Default.Count, buffer, index + BooleanSerializerBuilder.Default.Count)) : new Nullable<T>();
		}
	}
}