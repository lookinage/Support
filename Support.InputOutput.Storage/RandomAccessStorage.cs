using System;
using System.IO;

namespace Support.InputOutput.Storage
{
	/// <summary>
	/// Represents a storage that provides read from and write to random position.
	/// </summary>
	public class RandomAccessStorage : IDisposable
	{
		private const int _defaultStreamBufferSize = 0x1000;

		/// <summary>
		/// Initializes the <see cref="RandomAccessStorage"/> class from a file.
		/// </summary>
		/// <param name="path">The path to the storage file.</param>
		/// <param name="bufferSize">The size of the buffer of the file stream.</param>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than 1.</exception>
		static public RandomAccessStorage FromFile(string path, int bufferSize)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));
			if (bufferSize < 0x1)
				throw new ArgumentOutOfRangeException(nameof(bufferSize));
			return new RandomAccessStorage(new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, bufferSize, FileOptions.RandomAccess));
		}
		/// <summary>
		/// Initializes the <see cref="RandomAccessStorage"/> class from a file.
		/// </summary>
		/// <param name="path">The path to the storage file.</param>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
		static public RandomAccessStorage FromFile(string path) => FromFile(path, _defaultStreamBufferSize);

		private readonly Stream _stream;
		private long _position;
		private bool _disposed;

		private RandomAccessStorage(Stream stream) => _stream = stream;

		/// <summary>
		/// Gets the size of the storage.
		/// </summary>
		public long Size => _stream.Length;

		private void ReadInternal(long position, int count, byte[] buffer, int index)
		{
			if (_position != position)
				_ = _stream.Seek(position, SeekOrigin.Begin);
			int readCount = _stream.Read(buffer, index, count);
			_position = position + readCount;
			if (readCount == count)
				return;
			Array.Clear(buffer, index + readCount, count - readCount);
		}
		private void WriteInternal(byte[] buffer, int index, int count, long position)
		{
			if (_position != position)
				_ = _stream.Seek(position, SeekOrigin.Begin);
			_stream.EnsureLength(_position = position + count);
			_stream.Write(buffer, index, count);
		}
		/// <summary>
		/// Reads bytes from the storage.
		/// </summary>
		/// <param name="position">The position of the bytes in the storage.</param>
		/// <param name="count">The number of the bytes.</param>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="position"/> to the end of the storage.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="RandomAccessStorage"/> has been closed.</exception>
		public void Read(long position, int count, byte[] buffer, int index)
		{
			if (position < 0x0)
				throw new ArgumentOutOfRangeException(nameof(position));
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > long.MaxValue - position)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of the storage.", nameof(count), nameof(position)));
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
			if (_disposed)
				throw new ObjectDisposedException(nameof(RandomAccessStorage));
			if (count == 0x0)
				return;
			ReadInternal(position, count, buffer, index);
		}
		/// <summary>
		/// Writes bytes to the storage.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <param name="count">The number of the bytes.</param>
		/// <param name="position">The position of the bytes in the storage.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="position"/> to the end of the storage.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="RandomAccessStorage"/> has been closed.</exception>
		public void Write(byte[] buffer, int index, int count, long position)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from the {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
			if (position < 0x0)
				throw new ArgumentOutOfRangeException(nameof(position));
			if (count > long.MaxValue - position)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from the {1} to the end of the storage.", nameof(count), nameof(position)));
			if (_disposed)
				throw new ObjectDisposedException(nameof(RandomAccessStorage));
			if (count == 0x0)
				return;
			WriteInternal(buffer, index, count, position);
		}
		/// <summary>
		/// Commits the written bytes.
		/// </summary>
		/// <exception cref="ObjectDisposedException">The <see cref="RandomAccessStorage"/> has been closed.</exception>
		public void Commit()
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(LogStorage));
			_stream.Flush();
		}
		/// <summary>
		/// Closes the storage and releases all resources used by the <see cref="RandomAccessStorage"/>.
		/// </summary>
		public void Close()
		{
			Commit();
			_stream.Close();
			_disposed = true;
		}
		void IDisposable.Dispose() => Close();
	}
}