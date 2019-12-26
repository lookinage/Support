using System;
using System.IO;

namespace Support.InputOutput.Storage
{
	/// <summary>
	/// Represents a storage for logging.
	/// </summary>
	public class LogStorage : IDisposable
	{
		private const int _defaultStreamBufferSize = 0x1000;

		/// <summary>
		/// Initializes the <see cref="LogStorage"/> class from a file.
		/// </summary>
		/// <param name="length">The length of the log.</param>
		/// <param name="path">The path to the storage file.</param>
		/// <param name="bufferSize">The size of the buffer of the file stream.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than 0.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than 1.</exception>
		static public LogStorage FromFile(long length, string path, int bufferSize)
		{
			if (length < 0x0)
				throw new ArgumentOutOfRangeException(nameof(length));
			if (path == null)
				throw new ArgumentNullException(nameof(path));
			if (bufferSize < 0x1)
				throw new ArgumentOutOfRangeException(nameof(bufferSize));
			return new LogStorage(new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, bufferSize, FileOptions.SequentialScan), length);
		}
		/// <summary>
		/// Initializes the <see cref="LogStorage"/> class from a file.
		/// </summary>
		/// <param name="length">The length of the log.</param>
		/// <param name="path">The path to the storage file.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is less than 0.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
		static public LogStorage FromFile(long length, string path) => FromFile(length, path, _defaultStreamBufferSize);

		private readonly Stream _stream;
		private long _position;
		private long _length;
		private bool _disposed;

		private LogStorage(Stream stream, long length)
		{
			stream.EnsureLength(length);
			_stream = stream;
			_position = stream.Position;
			_length = length;
		}

		/// <summary>
		/// Gets the length of the log.
		/// </summary>
		public long Length => _length;

		/// <summary>
		/// Reads bytes from the log.
		/// </summary>
		/// <param name="position">The position of the bytes in the log.</param>
		/// <param name="count">The number of the bytes.</param>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="position"/> is less than 0 or greater than the number of bytes of the log.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="position"/> to the end of the log.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="LogStorage"/> has been closed.</exception>
		public void Read(long position, int count, byte[] buffer, int index)
		{
			if (position < 0x0 || position > Length)
				throw new ArgumentOutOfRangeException(nameof(position));
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > Length - position)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of the log.", nameof(count), nameof(position)));
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
			if (_disposed)
				throw new ObjectDisposedException(nameof(LogStorage));
			if (count == 0x0)
				return;
			if (position != _position)
				_ = _stream.Seek(position, SeekOrigin.Begin);
			_ = _stream.Read(buffer, index, count);
			_position = position + count;
		}
		/// <summary>
		/// Writes bytes to the end of the log.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <param name="count">The number of the bytes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="LogStorage"/> has been closed.</exception>
		public void Write(byte[] buffer, int index, int count)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from the {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
			if (_disposed)
				throw new ObjectDisposedException(nameof(LogStorage));
			if (count == 0x0)
				return;
			if (_length != _position)
				_ = _stream.Seek(_length, SeekOrigin.Begin);
			checked { _length += count; }
			_position = _length;
			_stream.EnsureLength(_length);
			_stream.Write(buffer, index, count);
		}
		/// <summary>
		/// Commits the written bytes.
		/// </summary>
		/// <exception cref="ObjectDisposedException">The <see cref="LogStorage"/> has been closed.</exception>
		public void Commit()
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(LogStorage));
			_stream.Flush();
		}
		/// <summary>
		/// Commits then closes the storage and releases all resources used by the <see cref="LogStorage"/>.
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