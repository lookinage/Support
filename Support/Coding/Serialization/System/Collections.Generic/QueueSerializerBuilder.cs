using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	/// <summary>
	/// Represents a builder of serializers of the <see cref="Queue{T}"/> type.
	/// </summary>
	public static class QueueSerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="Queue{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="Queue{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="ISerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="Queue{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<Queue<T>> CreateSerializer<T>(ISerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (QueueSerializer<T>._serializers.TryGetValue(elementSerializer, out QueueSerializer<T> serializer))
				return serializer;
			QueueSerializer<T>._serializers.Add(elementSerializer, serializer = new QueueSerializer<T>(elementSerializer));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="Queue{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="Queue{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="IConstantLengthSerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="Queue{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<Queue<T>> CreateSerializer<T>(IConstantLengthSerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (QueueSerializerElementConstantLength<T>._serializers.TryGetValue(elementSerializer, out QueueSerializerElementConstantLength<T> serializer))
				return serializer;
			QueueSerializerElementConstantLength<T>._serializers.Add(elementSerializer, serializer = new QueueSerializerElementConstantLength<T>(elementSerializer));
			return serializer;
		}
	}
}