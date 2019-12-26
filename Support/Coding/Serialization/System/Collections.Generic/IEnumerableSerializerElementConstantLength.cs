using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class IEnumerableSerializerElementConstantLength<T> : Serializer<IEnumerable<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<IConstantLengthSerializer<T>, IEnumerableSerializerElementConstantLength<T>> _serializers;

		static IEnumerableSerializerElementConstantLength() => _serializers = new Dictionary<IConstantLengthSerializer<T>, IEnumerableSerializerElementConstantLength<T>>();

		private readonly IConstantLengthSerializer<T> _elementSerializer;

		internal IEnumerableSerializerElementConstantLength(IConstantLengthSerializer<T> elementSerializer) => _elementSerializer = elementSerializer;

		public override sealed int Count(IEnumerable<T> instance)
		{
			ValidateCount(instance);
			int count = 0x0;
			IEnumerator<T> enumerator = instance.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
					checked { count++; }
				return checked(_elementSerializer.Count * count);
			}
			catch (OverflowException exception) { throw new ArgumentException("An error occurred.", exception); }
		}
		public override sealed void Serialize(IEnumerable<T> instance, byte[] buffer, ref int index)
		{
			ValidateSerialize(instance, buffer, index);
			foreach (T element in instance)
				_elementSerializer.Serialize(element, buffer, ref index);
		}
		public override sealed IEnumerable<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return ArraySerializerElementConstantLength<T>.Deserialize(_elementSerializer, count, buffer, index);
		}
	}
}