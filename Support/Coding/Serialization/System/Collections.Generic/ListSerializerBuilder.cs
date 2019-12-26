using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	/// <summary>
	/// Represents a builder of serializers of the <see cref="List{T}"/> type.
	/// </summary>
	static public class ListSerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="List{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of a <see cref="List{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="ISerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="List{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<List<T>> CreateSerializer<T>(ISerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (ListSerializer<T>._serializers.TryGetValue(elementSerializer, out ListSerializer<T> serializer))
				return serializer;
			ListSerializer<T>._serializers.Add(elementSerializer, serializer = new ListSerializer<T>(elementSerializer));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="List{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of a <see cref="List{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="IConstantLengthSerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="List{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<List<T>> CreateSerializer<T>(IConstantLengthSerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (ListSerializerElementConstantLength<T>._serializers.TryGetValue(elementSerializer, out ListSerializerElementConstantLength<T> serializer))
				return serializer;
			ListSerializerElementConstantLength<T>._serializers.Add(elementSerializer, serializer = new ListSerializerElementConstantLength<T>(elementSerializer));
			return serializer;
		}
	}
}