using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	/// <summary>
	/// Represents a builder of serializers of the <see cref="HashSet{T}"/> type.
	/// </summary>
	static public class HashSetSerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="HashSet{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of a <see cref="HashSet{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="ISerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="HashSet{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<HashSet<T>> CreateSerializer<T>(ISerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (HashSetSerializer<T>._serializers.TryGetValue(elementSerializer, out HashSetSerializer<T> serializer))
				return serializer;
			HashSetSerializer<T>._serializers.Add(elementSerializer, serializer = new HashSetSerializer<T>(elementSerializer));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="HashSet{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of a <see cref="HashSet{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="IConstantLengthSerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="HashSet{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<HashSet<T>> CreateSerializer<T>(IConstantLengthSerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (HashSetSerializerElementConstantLength<T>._serializers.TryGetValue(elementSerializer, out HashSetSerializerElementConstantLength<T> serializer))
				return serializer;
			HashSetSerializerElementConstantLength<T>._serializers.Add(elementSerializer, serializer = new HashSetSerializerElementConstantLength<T>(elementSerializer));
			return serializer;
		}
	}
}