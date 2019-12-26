using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	/// <summary>
	/// Represents a builder of serializers of the <see cref="KeyValuePair{TKey, TValue}"/> type.
	/// </summary>
	static public class KeyValuePairSerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="KeyValuePair{TKey, TValue}"/> type.
		/// </summary>
		/// <typeparam name="TKey">The type of a key.</typeparam>
		/// <typeparam name="TValue">The type of a value.</typeparam>
		/// <param name="keySerializer">An <see cref="ISerializer{T}"/> of the <typeparamref name="TKey"/> type.</param>
		/// <param name="valueSerializer">An <see cref="ISerializer{T}"/> of the <typeparamref name="TValue"/> type.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="KeyValuePair{TKey, TValue}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="keySerializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="valueSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<KeyValuePair<TKey, TValue>> CreateSerializer<TKey, TValue>(ISerializer<TKey> keySerializer, ISerializer<TValue> valueSerializer)
		{
			if (keySerializer == null)
				throw new ArgumentNullException(nameof(keySerializer));
			if (valueSerializer == null)
				throw new ArgumentNullException(nameof(valueSerializer));
			KeyValuePairSerializer<TKey, TValue>.Info info = new KeyValuePairSerializer<TKey, TValue>.Info(keySerializer, valueSerializer);
			if (KeyValuePairSerializer<TKey, TValue>._serializers.TryGetValue(info, out KeyValuePairSerializer<TKey, TValue> serializer))
				return serializer;
			KeyValuePairSerializer<TKey, TValue>._serializers.Add(info, serializer = new KeyValuePairSerializer<TKey, TValue>(info));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="KeyValuePair{TKey, TValue}"/> type.
		/// </summary>
		/// <typeparam name="TKey">The type of a key.</typeparam>
		/// <typeparam name="TValue">The type of a value.</typeparam>
		/// <param name="keySerializer">An <see cref="ISerializer{T}"/> of the <typeparamref name="TKey"/> type.</param>
		/// <param name="valueSerializer">An <see cref="IConstantLengthSerializer{T}"/> of the <typeparamref name="TValue"/> type.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="KeyValuePair{TKey, TValue}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="keySerializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="valueSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<KeyValuePair<TKey, TValue>> CreateSerializer<TKey, TValue>(ISerializer<TKey> keySerializer, IConstantLengthSerializer<TValue> valueSerializer)
		{
			if (keySerializer == null)
				throw new ArgumentNullException(nameof(keySerializer));
			if (valueSerializer == null)
				throw new ArgumentNullException(nameof(valueSerializer));
			KeyValuePairSerializerValueConstantLength<TKey, TValue>.Info info = new KeyValuePairSerializerValueConstantLength<TKey, TValue>.Info(keySerializer, valueSerializer);
			if (KeyValuePairSerializerValueConstantLength<TKey, TValue>._serializers.TryGetValue(info, out KeyValuePairSerializerValueConstantLength<TKey, TValue> serializer))
				return serializer;
			KeyValuePairSerializerValueConstantLength<TKey, TValue>._serializers.Add(info, serializer = new KeyValuePairSerializerValueConstantLength<TKey, TValue>(info));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="KeyValuePair{TKey, TValue}"/> type.
		/// </summary>
		/// <typeparam name="TKey">The type of a key.</typeparam>
		/// <typeparam name="TValue">The type of a value.</typeparam>
		/// <param name="keySerializer">An <see cref="IConstantLengthSerializer{T}"/> of the <typeparamref name="TKey"/> type.</param>
		/// <param name="valueSerializer">An <see cref="ISerializer{T}"/> of the <typeparamref name="TValue"/> type.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="KeyValuePair{TKey, TValue}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="keySerializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="valueSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<KeyValuePair<TKey, TValue>> CreateSerializer<TKey, TValue>(IConstantLengthSerializer<TKey> keySerializer, ISerializer<TValue> valueSerializer)
		{
			if (keySerializer == null)
				throw new ArgumentNullException(nameof(keySerializer));
			if (valueSerializer == null)
				throw new ArgumentNullException(nameof(valueSerializer));
			KeyValuePairSerializerKeyConstantLength<TKey, TValue>.Info info = new KeyValuePairSerializerKeyConstantLength<TKey, TValue>.Info(keySerializer, valueSerializer);
			if (KeyValuePairSerializerKeyConstantLength<TKey, TValue>._serializers.TryGetValue(info, out KeyValuePairSerializerKeyConstantLength<TKey, TValue> serializer))
				return serializer;
			KeyValuePairSerializerKeyConstantLength<TKey, TValue>._serializers.Add(info, serializer = new KeyValuePairSerializerKeyConstantLength<TKey, TValue>(info));
			return serializer;
		}
		/// <summary>
		/// Returns an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="KeyValuePair{TKey, TValue}"/> type.
		/// </summary>
		/// <typeparam name="TKey">The type of a key.</typeparam>
		/// <typeparam name="TValue">The type of a value.</typeparam>
		/// <param name="keySerializer">An <see cref="IConstantLengthSerializer{T}"/> of the <typeparamref name="TKey"/> type.</param>
		/// <param name="valueSerializer">An <see cref="IConstantLengthSerializer{T}"/> of the <typeparamref name="TValue"/> type.</param>
		/// <returns>An <see cref="IConstantLengthSerializer{T}"/> of the <see cref="KeyValuePair{TKey, TValue}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="keySerializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="valueSerializer"/> is <see langword="null"/>.</exception>
		static public ConstantLengthSerializer<KeyValuePair<TKey, TValue>> CreateSerializer<TKey, TValue>(IConstantLengthSerializer<TKey> keySerializer, IConstantLengthSerializer<TValue> valueSerializer)
		{
			if (keySerializer == null)
				throw new ArgumentNullException(nameof(keySerializer));
			if (valueSerializer == null)
				throw new ArgumentNullException(nameof(valueSerializer));
			KeyValuePairSerializerKeyConstantLengthValueConstantLength<TKey, TValue>.Info info = new KeyValuePairSerializerKeyConstantLengthValueConstantLength<TKey, TValue>.Info(keySerializer, valueSerializer);
			if (KeyValuePairSerializerKeyConstantLengthValueConstantLength<TKey, TValue>._serializers.TryGetValue(info, out KeyValuePairSerializerKeyConstantLengthValueConstantLength<TKey, TValue> serializer))
				return serializer;
			KeyValuePairSerializerKeyConstantLengthValueConstantLength<TKey, TValue>._serializers.Add(info, serializer = new KeyValuePairSerializerKeyConstantLengthValueConstantLength<TKey, TValue>(info));
			return serializer;
		}
	}
}