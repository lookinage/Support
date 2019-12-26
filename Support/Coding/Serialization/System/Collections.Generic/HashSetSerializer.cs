using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal sealed class HashSetSerializer<T> : Serializer<HashSet<T>>
	{
		[ThreadStatic]
		static internal readonly Dictionary<ISerializer<T>, HashSetSerializer<T>> _serializers;

		static HashSetSerializer() => _serializers = new Dictionary<ISerializer<T>, HashSetSerializer<T>>();

		static internal HashSet<T> Deserialize(ISerializer<T> elementSerializer, int count, byte[] buffer, int index)
		{
			IEnumerableDeserializer<T> enumerator = new IEnumerableDeserializer<T>(elementSerializer, count, buffer, index, out _);
			HashSet<T> instance = new HashSet<T>();
			while (enumerator.MoveNext())
				if (!instance.Add(enumerator.Current))
					throw new ArgumentException("An element is already present in the set.");
			return instance;
		}

		private readonly ISerializer<T> _elementSerializer;
		private readonly Serializer<ISet<T>> _iSetSerializer;

		internal HashSetSerializer(ISerializer<T> elementSerializer)
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