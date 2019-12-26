using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	/// <summary>
	/// Represents a builder of serializers of the <see cref="IList{T}"/> type.
	/// </summary>
	public static class IListSerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="IList{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="IList{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="ISerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="IList{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<IList<T>> CreateSerializer<T>(ISerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (IListSerializer<T>._serializers.TryGetValue(elementSerializer, out IListSerializer<T> serializer))
				return serializer;
			IListSerializer<T>._serializers.Add(elementSerializer, serializer = new IListSerializer<T>(elementSerializer));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="IList{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="IList{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="IConstantLengthSerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="IList{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<IList<T>> CreateSerializer<T>(IConstantLengthSerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (IListSerializerElementConstantLength<T>._serializers.TryGetValue(elementSerializer, out IListSerializerElementConstantLength<T> serializer))
				return serializer;
			IListSerializerElementConstantLength<T>._serializers.Add(elementSerializer, serializer = new IListSerializerElementConstantLength<T>(elementSerializer));
			return serializer;
		}
	}
}