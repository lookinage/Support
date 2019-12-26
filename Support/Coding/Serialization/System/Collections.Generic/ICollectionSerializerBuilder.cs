using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	/// <summary>
	/// Represents a builder of serializers of the <see cref="ICollection{T}"/> type.
	/// </summary>
	public static class ICollectionSerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="ICollection{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="ICollection{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="ISerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="ICollection{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<ICollection<T>> CreateSerializer<T>(ISerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (ICollectionSerializer<T>._serializers.TryGetValue(elementSerializer, out ICollectionSerializer<T> serializer))
				return serializer;
			ICollectionSerializer<T>._serializers.Add(elementSerializer, serializer = new ICollectionSerializer<T>(elementSerializer));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="ICollection{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="ICollection{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="IConstantLengthSerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="ICollection{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<ICollection<T>> CreateSerializer<T>(IConstantLengthSerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (ICollectionSerializerElementConstantLength<T>._serializers.TryGetValue(elementSerializer, out ICollectionSerializerElementConstantLength<T> serializer))
				return serializer;
			ICollectionSerializerElementConstantLength<T>._serializers.Add(elementSerializer, serializer = new ICollectionSerializerElementConstantLength<T>(elementSerializer));
			return serializer;
		}
	}
}