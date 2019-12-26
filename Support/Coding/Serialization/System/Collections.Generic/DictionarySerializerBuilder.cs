using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	/// <summary>
	/// Represents a builder of serializers of the <see cref="Dictionary{TKey, TValue}"/> type.
	/// </summary>
	static public class DictionarySerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="Dictionary{TKey, TValue}"/> type.
		/// </summary>
		/// <typeparam name="TKey">The type of keys.</typeparam>
		/// <typeparam name="TValue">The type of values.</typeparam>
		/// <param name="elementSerializer">An <see cref="ISerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="Dictionary{TKey, TValue}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<Dictionary<TKey, TValue>> CreateSerializer<TKey, TValue>(ISerializer<KeyValuePair<TKey, TValue>> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (DictionarySerializer<TKey, TValue>._serializers.TryGetValue(elementSerializer, out DictionarySerializer<TKey, TValue> serializer))
				return serializer;
			DictionarySerializer<TKey, TValue>._serializers.Add(elementSerializer, serializer = new DictionarySerializer<TKey, TValue>(elementSerializer));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="Dictionary{TKey, TValue}"/> type.
		/// </summary>
		/// <typeparam name="TKey">The type of keys.</typeparam>
		/// <typeparam name="TValue">The type of values.</typeparam>
		/// <param name="elementSerializer">An <see cref="IConstantLengthSerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="Dictionary{TKey, TValue}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<Dictionary<TKey, TValue>> CreateSerializer<TKey, TValue>(IConstantLengthSerializer<KeyValuePair<TKey, TValue>> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (DictionarySerializerElementConstantLength<TKey, TValue>._serializers.TryGetValue(elementSerializer, out DictionarySerializerElementConstantLength<TKey, TValue> serializer))
				return serializer;
			DictionarySerializerElementConstantLength<TKey, TValue>._serializers.Add(elementSerializer, serializer = new DictionarySerializerElementConstantLength<TKey, TValue>(elementSerializer));
			return serializer;
		}
	}
}