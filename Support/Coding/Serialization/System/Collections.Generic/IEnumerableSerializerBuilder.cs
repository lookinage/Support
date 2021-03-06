﻿using System;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	/// <summary>
	/// Represents a builder of serializers of the <see cref="IEnumerable{T}"/> type.
	/// </summary>
	public static class IEnumerableSerializerBuilder
	{
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="IEnumerable{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="IEnumerable{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="ISerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="IEnumerable{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<IEnumerable<T>> CreateSerializer<T>(ISerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (IEnumerableSerializer<T>._serializers.TryGetValue(elementSerializer, out IEnumerableSerializer<T> serializer))
				return serializer;
			IEnumerableSerializer<T>._serializers.Add(elementSerializer, serializer = new IEnumerableSerializer<T>(elementSerializer));
			return serializer;
		}
		/// <summary>
		/// Creates an <see cref="ISerializer{T}"/> of the <see cref="IEnumerable{T}"/> type.
		/// </summary>
		/// <typeparam name="T">The type of elements of an <see cref="IEnumerable{T}"/>.</typeparam>
		/// <param name="elementSerializer">An <see cref="IConstantLengthSerializer{T}"/> of elements.</param>
		/// <returns>An <see cref="ISerializer{T}"/> of the <see cref="IEnumerable{T}"/> type.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="elementSerializer"/> is <see langword="null"/>.</exception>
		static public Serializer<IEnumerable<T>> CreateSerializer<T>(IConstantLengthSerializer<T> elementSerializer)
		{
			if (elementSerializer == null)
				throw new ArgumentNullException(nameof(elementSerializer));
			if (IEnumerableSerializerElementConstantLength<T>._serializers.TryGetValue(elementSerializer, out IEnumerableSerializerElementConstantLength<T> serializer))
				return serializer;
			IEnumerableSerializerElementConstantLength<T>._serializers.Add(elementSerializer, serializer = new IEnumerableSerializerElementConstantLength<T>(elementSerializer));
			return serializer;
		}
	}
}