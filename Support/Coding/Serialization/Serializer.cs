using System;

namespace Support.Coding.Serialization
{
	/// <summary>
	/// Represents an <see cref="ISerializer{T}"/> implementation.
	/// </summary>
	/// <typeparam name="T">The type which instances are to be able to be serialized to a sequence of bytes and deserialized back.</typeparam>
	public abstract class Serializer<T> : ISerializer<T>
	{
		/// <summary>
		/// Initializes the <see cref="Serializer{T}"/>.
		/// </summary>
		protected Serializer() { }

		/// <summary>
		/// Validates arguments of <see cref="Count(T)"/> method.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
		protected void ValidateCount(T instance)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
		}
		/// <summary>
		/// Validates arguments of <see cref="Serialize(T, byte[], int)"/> method.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		protected void ValidateSerialize(byte[] buffer, int index)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
		}
		/// <summary>
		/// Validates arguments of <see cref="Serialize(T, byte[], int)"/> method.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		protected void ValidateSerialize(T instance, byte[] buffer, int index)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			ValidateSerialize(buffer, index);
		}
		/// <summary>
		/// Validates arguments of <see cref="Deserialize(int, byte[], int)"/> method.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		protected void ValidateDeserialize(int count, byte[] buffer, int index)
		{
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
		}
		/// <summary>
		/// Counts a number of bytes requiring to serialize <paramref name="instance"/>.
		/// </summary>
		/// <param name="instance">An instance the number of bytes counts for.</param>
		/// <returns>A number of bytes requiring to serialize <paramref name="instance"/>.</returns>
		/// <exception cref="ArgumentException">An error occurred.</exception>
		public abstract int Count(T instance);
		/// <summary>
		/// Serializes a specified instance to a sequence of bytes then writes the bytes to a specified buffer and increases a specified index by the number of serialized bytes.
		/// </summary>
		/// <param name="instance">An instance to serialize.</param>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">An error occurred.</exception>
		public abstract void Serialize(T instance, byte[] buffer, ref int index);
		/// <summary>
		/// Serializes a specified instance to a sequence of bytes then writes the bytes to a specified buffer.
		/// </summary>
		/// <param name="instance">An instance to serialize.</param>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">An error occurred.</exception>
		public void Serialize(T instance, byte[] buffer, int index) => Serialize(instance, buffer, ref index);
		/// <summary>
		/// Deserializes bytes to an instance.
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
		public abstract T Deserialize(int count, byte[] buffer, int index);
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
		public T Deserialize(int count, byte[] buffer, ref int index)
		{
			try { return Deserialize(count, buffer, index); }
			finally { index += count; }
		}
	}
}