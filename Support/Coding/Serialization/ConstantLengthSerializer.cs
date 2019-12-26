using System;

namespace Support.Coding.Serialization
{
	/// <summary>
	/// Represents an <see cref="IConstantLengthSerializer{T}"/> implementation.
	/// </summary>
	/// <typeparam name="T">The type which instances are to be able to be serialized to a sequence of bytes with constant length and deserialized back.</typeparam>
	public abstract class ConstantLengthSerializer<T> : IConstantLengthSerializer<T>
	{
		private readonly int _count;

		/// <summary>
		/// Initializes the <see cref="ConstantLengthSerializer{T}"/>.
		/// </summary>
		/// <param name="count">A number of bytes requiring to serialize any instance of the type.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		protected ConstantLengthSerializer(int count) => _count = count < 0x0 ? throw new ArgumentOutOfRangeException(nameof(count)) : count;

		/// <summary>
		/// Gets the number of bytes requiring to serialize any instance of the type.
		/// </summary>
		public int Count => _count;

		private void ValidateCount(int count)
		{
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count != Count)
				throw new ArgumentException(string.Format("{0} must equals to {1}.", nameof(count), Count));
		}
		/// <summary>
		/// Validates arguments of <see cref="Serialize(T, byte[], int)"/> method.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><see cref="Count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/></exception>
		protected void ValidateSerialize(byte[] buffer, int index)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (Count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0}.{1} is greater than the number of bytes from {2} to the end of {3}", nameof(ConstantLengthSerializer<T>), nameof(Count), nameof(index), nameof(buffer)));
		}
		/// <summary>
		/// Validates arguments of <see cref="Serialize(T, byte[], int)"/> method.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><see cref="Count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/></exception>
		protected void ValidateSerialize(T instance, byte[] buffer, int index)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			ValidateSerialize(buffer, index);
		}
		/// <summary>
		/// Validates arguments of <see cref="Deserialize(byte[], int)"/> method.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><see cref="Count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/></exception>
		protected void ValidateDeserialize(byte[] buffer, int index)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (Count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0}.{1} is greater than the number of bytes from {2} to the end of {3}", nameof(ConstantLengthSerializer<T>), nameof(Count), nameof(index), nameof(buffer)));
		}
		/// <summary>
		/// Serializes a specified instance to a sequence of bytes then writes the bytes to a specified buffer.
		/// </summary>
		/// <param name="instance">An instance to serialize.</param>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><see cref="Count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/> or another error occurred.</exception>
		public abstract void Serialize(T instance, byte[] buffer, int index);
		/// <summary>
		/// Serializes a specified instance to a sequence of bytes then writes the bytes to a specified buffer and increases a specified index by the number of serialized bytes.
		/// </summary>
		/// <param name="instance">An instance to serialize.</param>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><see cref="Count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/> or another error occurred.</exception>
		public void Serialize(T instance, byte[] buffer, ref int index)
		{
			Serialize(instance, buffer, index);
			index += Count;
		}
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
		public abstract T Deserialize(byte[] buffer, int index);
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
		public T Deserialize(byte[] buffer, ref int index)
		{
			try { return Deserialize(buffer, index); }
			finally { index += Count; }
		}
		int ISerializer<T>.Count(T instance) => Count;
		T ISerializer<T>.Deserialize(int count, byte[] buffer, int index)
		{
			ValidateCount(count);
			return Deserialize(buffer, index);
		}
		T ISerializer<T>.Deserialize(int count, byte[] buffer, ref int index)
		{
			ValidateCount(count);
			try { return Deserialize(buffer, index); }
			finally { index += count; }
		}
	}
}