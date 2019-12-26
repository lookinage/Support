using Support.Coding.Serialization.System;
using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization
{
	internal sealed class DefaultableSerializer<T> : Serializer<T> where T : class
	{
		static private readonly IEqualityComparer<T> _equalityComparer;
		[ThreadStatic]
		static internal readonly Dictionary<ISerializer<T>, DefaultableSerializer<T>> _serializers;

		static DefaultableSerializer()
		{
			_equalityComparer = EqualityComparer<T>.Default;
			_serializers = new Dictionary<ISerializer<T>, DefaultableSerializer<T>>();
		}

		private readonly ISerializer<T> _underlyingSerializer;

		internal DefaultableSerializer(ISerializer<T> underlyingSerializer) => _underlyingSerializer = underlyingSerializer;

		public override sealed int Count(T instance)
		{
			try { return _equalityComparer.Equals(instance, default) ? BooleanSerializerBuilder.Default.Count : checked(BooleanSerializerBuilder.Default.Count + _underlyingSerializer.Count(instance)); }
			catch (OverflowException exception) { throw new ArgumentException("An error occurred.", exception); }
		}
		public override sealed void Serialize(T instance, byte[] buffer, ref int index)
		{
			ValidateSerialize(buffer, index);
			if (_equalityComparer.Equals(instance, default))
			{
				BooleanSerializerBuilder.Default.Serialize(false, buffer, ref index);
				return;
			}
			BooleanSerializerBuilder.Default.Serialize(true, buffer, ref index);
			_underlyingSerializer.Serialize(instance, buffer, ref index);
		}
		public override sealed T Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return BooleanSerializerBuilder.Default.Deserialize(buffer, index) ? _underlyingSerializer.Deserialize(count - BooleanSerializerBuilder.Default.Count, buffer, index + BooleanSerializerBuilder.Default.Count) : default;
		}
	}
}