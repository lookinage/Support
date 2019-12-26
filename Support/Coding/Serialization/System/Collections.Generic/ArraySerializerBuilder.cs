using System;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	/// <summary>
	/// Represents a builder of serializers of the first rank array type.
	/// </summary>
	static public class ArraySerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the first rank array type.
		/// </summary>
		/// <typeparam name="T">The type of elements of a first rank array.</typeparam>
		/// <param name="elementSerializer">An <see cref="ISerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the first rank array type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<T[]> CreateSerializer<T>(ISerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (ArraySerializer<T>._serializers.TryGetValue(elementSerializer, out ArraySerializer<T> serializer))
				return serializer;
			ArraySerializer<T>._serializers.Add(elementSerializer, serializer = new ArraySerializer<T>(elementSerializer));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the first rank array type.
		/// </summary>
		/// <typeparam name="T">The type of elements of a first rank array.</typeparam>
		/// <param name="elementSerializer">An <see cref="IConstantLengthSerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the first rank array type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<T[]> CreateSerializer<T>(IConstantLengthSerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (ArraySerializerElementConstantLength<T>._serializers.TryGetValue(elementSerializer, out ArraySerializerElementConstantLength<T> serializer))
				return serializer;
			ArraySerializerElementConstantLength<T>._serializers.Add(elementSerializer, serializer = new ArraySerializerElementConstantLength<T>(elementSerializer));
			return serializer;
		}
	}
}