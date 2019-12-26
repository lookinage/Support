using System;

namespace Support.Coding.Serialization
{
	/// <summary>
	/// Represents a serializer that serializes instances to a sequence of bytes and deserializes back. 
	/// </summary>
	/// <typeparam name="T">The type which instances are to be able to be serialized to a sequence of bytes and deserialized back.</typeparam>
	public interface ISerializer<T>
	{
		/// <summary>
		/// Counts a number of bytes requiring to serialize <paramref name="instance"/>.
		/// </summary>
		/// <param name="instance">An instance the number of bytes counts for.</param>
		/// <returns>A number of bytes requiring to serialize <paramref name="instance"/>.</returns>
		/// <exception cref="ArgumentException">An error occurred.</exception>
		int Count(T instance);
		/// <summary>
		/// Serializes a specified instance to a sequence of bytes then writes the bytes to a specified buffer.
		/// </summary>
		/// <param name="instance">An instance to serialize.</param>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">An error occurred.</exception>
		void Serialize(T instance, byte[] buffer, int index);
		/// <summary>
		/// Serializes a specified instance to a sequence of bytes then writes the bytes to a specified buffer and increases a specified index by the number of serialized bytes.
		/// </summary>
		/// <param name="instance">An instance to serialize.</param>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">An error occurred.</exception>
		void Serialize(T instance, byte[] buffer, ref int index);
		/// <summary>
		/// Deserializes bytes to an instance.
		/// </summary>
		/// <param name="count">A number of bytes to deserialize.</param>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <returns>An instance deserialized from the bytes.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">An error occurred.</exception>
		T Deserialize(int count, byte[] buffer, int index);
		/// <summary>
		/// Deserializes bytes to an instance and increases a specified index by the number of deserialized bytes.
		/// </summary>
		/// <param name="count">A number of bytes to deserialize.</param>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <returns>An instance deserialized from the bytes.</returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">An error occurred.</exception>
		T Deserialize(int count, byte[] buffer, ref int index);
	}
}