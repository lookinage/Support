using System;

namespace Support.Coding.Serialization
{
	/// <summary>
	/// Represents a builder of an <see cref="ISerializer{T}"/> that can serialize null value of a reference type.
	/// </summary>
	static public class DefaultableSerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> that can serialize null value of a reference type.
		/// </summary>
		/// <param name="underlyingSerializer">The underlying <see cref="ISerializer{T}"/> of the type.</param>
		/// <returns>An <see cref="ISerializer{T}"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="underlyingSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<T> CreateSerializer<T>(ISerializer<T> underlyingSerializer) where T : class
		{
			if (underlyingSerializer == null)
				throw new ArgumentNullException(nameof(underlyingSerializer));
			if (DefaultableSerializer<T>._serializers.TryGetValue(underlyingSerializer, out DefaultableSerializer<T> serializer))
				return serializer;
			DefaultableSerializer<T>._serializers.Add(underlyingSerializer, serializer = new DefaultableSerializer<T>(underlyingSerializer));
			return serializer;
		}
	}
}