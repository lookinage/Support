using System;

namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="ISerializer{T}"/> of the <see cref="Nullable{T}"/> type.
	/// </summary>
	static public class NullableSerializerBuilder
	{
		/// <summary>
		/// Builds an <see cref="ISerializer{T}"/> of the <see cref="Nullable{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The underlying value type of the <see cref="Nullable{T}"/>.</typeparam>
		/// <param name="underlyingSerializer">The underlying <see cref="ISerializer{T}"/> of the underlying value type.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of <see cref="Nullable{T}"/> instances.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="underlyingSerializer"/> is <see langword="null"/>.</exception>
		static public ISerializer<Nullable<T>> CreateSerializer<T>(ISerializer<T> underlyingSerializer) where T : struct
		{
			if (underlyingSerializer == null)
				throw new ArgumentNullException(nameof(underlyingSerializer));
			if (NullableSerializer<T>._serializers.TryGetValue(underlyingSerializer, out NullableSerializer<T> serializer))
				return serializer;
			NullableSerializer<T>._serializers.Add(underlyingSerializer, serializer = new NullableSerializer<T>(underlyingSerializer));
			return serializer;
		}
	}
}