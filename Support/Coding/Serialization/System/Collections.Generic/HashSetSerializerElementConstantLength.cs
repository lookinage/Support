using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class HashSetSerializerElementConstantLength<T> : Serializer<HashSet<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<IConstantLengthSerializer<T>, HashSetSerializerElementConstantLength<T>> _serializers;

		static HashSetSerializerElementConstantLength() => _serializers = new Dictionary<IConstantLengthSerializer<T>, HashSetSerializerElementConstantLength<T>>();

		static internal HashSet<T> Deserialize(IConstantLengthSerializer<T> elementSerializer, int count, byte[] buffer, int index)
		{
			IEnumerableDeserializerElementConstantLength<T> enumerator = new IEnumerableDeserializerElementConstantLength<T>(elementSerializer, count, buffer, index, out _);
			HashSet<T> instance = new HashSet<T>();
			while (enumerator.MoveNext())
				if (!instance.Add(enumerator.Current))
					throw new ArgumentException("An element is already present in the set.");
			return instance;
		}

		private readonly IConstantLengthSerializer<T> _elementSerializer;
		private readonly Serializer<ISet<T>> _iSetSerializer;

		internal HashSetSerializerElementConstantLength(IConstantLengthSerializer<T> elementSerializer)
		{
			_elementSerializer = elementSerializer;
			_iSetSerializer = ISetSerializerBuilder.CreateSerializer(elementSerializer);
		}

		public override sealed int Count(HashSet<T> instance) => _iSetSerializer.Count(instance);
		public override sealed void Serialize(HashSet<T> instance, byte[] buffer, ref int index) => _iSetSerializer.Serialize(instance, buffer, ref index);
		public override sealed HashSet<T> Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			return Deserialize(_elementSerializer, count, buffer, index);
		}
	}
}