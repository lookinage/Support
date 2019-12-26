using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	/// <summary>
	/// Represents a builder of serializers of the <see cref="Stack{T}"/> type.
	/// </summary>
	public static class StackSerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="Stack{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="Stack{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="ISerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="Stack{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<Stack<T>> CreateSerializer<T>(ISerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (StackSerializer<T>._serializers.TryGetValue(elementSerializer, out StackSerializer<T> serializer))
				return serializer;
			StackSerializer<T>._serializers.Add(elementSerializer, serializer = new StackSerializer<T>(elementSerializer));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="Stack{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="Stack{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="IConstantLengthSerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="Stack{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<Stack<T>> CreateSerializer<T>(IConstantLengthSerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (StackSerializerElementConstantLength<T>._serializers.TryGetValue(elementSerializer, out StackSerializerElementConstantLength<T> serializer))
				return serializer;
			StackSerializerElementConstantLength<T>._serializers.Add(elementSerializer, serializer = new StackSerializerElementConstantLength<T>(elementSerializer));
			return serializer;
		}
	}
}