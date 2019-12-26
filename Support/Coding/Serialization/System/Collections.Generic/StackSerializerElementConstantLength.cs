using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class StackSerializerElementConstantLength<T> : Serializer<Stack<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<IConstantLengthSerializer<T>, StackSerializerElementConstantLength<T>> _serializers;

		static StackSerializerElementConstantLength() => _serializers = new Dictionary<IConstantLengthSerializer<T>, StackSerializerElementConstantLength<T>>();

		private readonly IConstantLengthSerializer<T> _elementSerializer;
		private readonly Serializer<IEnumerable<T>> _iEnumerableSerializer;

		internal StackSerializerElementConstantLength(IConstantLengthSerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iEnumerableSerializer = IEnumerableSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(Stack<T> instance)
		{
			ValidateCount(instance);
			try { return checked(_elementSerializer.Count * instance.Count); }
			catch (OverflowException exception) { throw new ArgumentException("An error occurred.", exception); }
		}
		public override sealed void Serialize(Stack<T> instance, byte[] buffer, ref int index) => _iEnumerableSerializer.Serialize(instance, buffer, ref index);
		public override sealed Stack<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			IEnumerableDeserializerElementConstantLength<T> enumerator = new IEnumerableDeserializerElementConstantLength<T>(_elementSerializer, count, buffer, index, out int length);
			Stack<T> instance = new Stack<T>(length);
			while (enumerator.MoveNext())
				instance.Push(enumerator.Current);
			return instance;
		}
	}
}