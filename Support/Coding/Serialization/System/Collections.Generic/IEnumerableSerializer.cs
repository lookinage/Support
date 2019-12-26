using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class IEnumerableSerializer<T> : Serializer<IEnumerable<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<ISerializer<T>, IEnumerableSerializer<T>> _serializers;

		static IEnumerableSerializer() => _serializers = new Dictionary<ISerializer<T>, IEnumerableSerializer<T>>();

		private readonly ISerializer<T> _elementSerializer;

		internal IEnumerableSerializer(ISerializer<T> elementSerializer) => _elementSerializer = elementSerializer;

		public override sealed int Count(IEnumerable<T> instance)
		{
			ValidateCount(instance);
			IEnumerator<T> enumerator = instance.GetEnumerator();
			if (!enumerator.MoveNext())
				return Int32SerializerBuilder.Default.Count;
			int byteCount = Int32SerializerBuilder.Default.Count;
			try
			{
				T item;
				for (item = enumerator.Current; enumerator.MoveNext(); item = enumerator.Current)
					checked { byteCount += Int32SerializerBuilder.Default.Count + _elementSerializer.Count(item); }
				checked { byteCount += _elementSerializer.Count(item); }
			}
			catch (OverflowException exception) { throw new ArgumentException("An error occurred.", exception); }
			return byteCount;
		}
		public override sealed void Serialize(IEnumerable<T> instance, byte[] buffer, ref int index)
		{
			ValidateSerialize(instance, buffer, index);
			int startIndex = index;
			index += Int32SerializerBuilder.Default.Count;
			int count = 0x0;
			IEnumerator<T> enumerator = instance.GetEnumerator();
			try
			{
				if (!enumerator.MoveNext())
					return;
				T item;
				for (item = enumerator.Current, count++; enumerator.MoveNext(); item = enumerator.Current, count++)
				{
					int startItemIndex = index += Int32SerializerBuilder.Default.Count;
					_elementSerializer.Serialize(item, buffer, ref index);
					Int32SerializerBuilder.Default.Serialize(index - startItemIndex, buffer, startItemIndex - Int32SerializerBuilder.Default.Count);
					item = enumerator.Current;
				}
				_elementSerializer.Serialize(item, buffer, ref index);
			}
			finally { Int32SerializerBuilder.Default.Serialize(count, buffer, startIndex); }
		}
		public override sealed IEnumerable<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return ArraySerializer<T>.Deserialize(_elementSerializer, count, buffer, index);
		}
	}
}