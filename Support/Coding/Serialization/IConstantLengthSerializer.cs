using System;

namespace Support.Coding.Serialization
{
	/// <summary>
	/// Represents a serializer that serializes instances of to a sequence of bytes with constant length and deserializes back
	/// </summary>
	/// <typeparam name="T">The type which instances are to be able to be serialized to a sequence of bytes with constant length and deserialized back.</typeparam>
	public interface IConstantLengthSerializer<T> : ISerializer<T>
	{
		/// <summary>
		/// Gets the number of bytes requiring to serialize any instance of the type.
		/// </summary>
		new int Count { get; }
		/// <summary>
		/// Deserializes bytes to an instance.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <returns>An instance deserialized from the bytes.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><see cref="Count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">An error occurred.</exception>
		T Deserialize(byte[] buffer, int index);
		/// <summary>
		/// Deserializes bytes to an instance and increases a specified index by the number of deserialized bytes.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <returns>An instance deserialized from the bytes.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><see cref="Count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">An error occurred.</exception>
		T Deserialize(byte[] buffer, ref int index);
	}
}