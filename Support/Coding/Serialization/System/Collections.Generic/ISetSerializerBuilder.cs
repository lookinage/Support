using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	/// <summary>
	/// Represents a builder of serializers of the <see cref="ISet{T}"/> type.
	/// </summary>
	public static class ISetSerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="ISet{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="ISet{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="ISerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="ISet{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<ISet<T>> CreateSerializer<T>(ISerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (ISetSerializer<T>._serializers.TryGetValue(elementSerializer, out ISetSerializer<T> serializer))
				return serializer;
			ISetSerializer<T>._serializers.Add(elementSerializer, serializer = new ISetSerializer<T>(elementSerializer));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="ISet{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="ISet{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="IConstantLengthSerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="ISet{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<ISet<T>> CreateSerializer<T>(IConstantLengthSerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (ISetSerializerElementConstantLength<T>._serializers.TryGetValue(elementSerializer, out ISetSerializerElementConstantLength<T> serializer))
				return serializer;
			ISetSerializerElementConstantLength<T>._serializers.Add(elementSerializer, serializer = new ISetSerializerElementConstantLength<T>(elementSerializer));
			return serializer;
		}
	}
}