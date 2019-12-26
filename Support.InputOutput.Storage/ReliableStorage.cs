using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using System;
using System.IO;

namespace Support.InputOutput.Storage
{
	/// <summary>
	/// Represents a storage that provides reliable read and write of a value.
	/// </summary>
	public class ReliableStorage : IDisposable
	{
		private const int _defaultStreamBufferSize = 0x1000;

		static private readonly int _metaSize;

		static ReliableStorage() => _metaSize = sizeof(byte) + Meta.Serializer._default.Count + Meta.Serializer._default.Count;

		private struct Meta
		{
			internal sealed class Serializer : ConstantLengthSerializer<Meta>
			{
				static internal readonly IConstantLengthSerializer<Meta> _default;

				static Serializer() => _default = new Serializer();

				internal Serializer() : base(Int64SerializerBuilder.Default.Count + Int32SerializerBuilder.Default.Count) { }

				public override void Serialize(Meta instance, byte[] buffer, int index)
				{
					Int64SerializerBuilder.Default.Serialize(instance._position, buffer, ref index);
					Int32SerializerBuilder.Default.Serialize(instance._length, buffer, ref index);
				}
				public override Meta Deserialize(byte[] buffer, int index) => new Meta(Int64SerializerBuilder.Default.Deserialize(buffer, ref index), Int32SerializerBuilder.Default.Deserialize(buffer, ref index));
			}

			internal long _position;
			internal int _length;

			internal Meta(long position, int length)
			{
				_position = position;
				_length = length;
			}
		}

		/// <summary>
		/// Initializes the <see cref="ReliableStorage"/> class from a file.
		/// </summary>
		/// <param name="path">The path to the storage file.</param>
		/// <param name="bufferSize">The size of the buffer of the file stream.</param>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> is less than 1.</exception>
		/// <exception cref="ArgumentException">Invalid storage format.</exception>
		static public ReliableStorage FromFile(string path, int bufferSize)
		{
			if (path == null)
				throw new ArgumentNullException(nameof(path));
			if (bufferSize < 0x1)
				throw new ArgumentOutOfRangeException(nameof(bufferSize));
			return new ReliableStorage(new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, bufferSize, FileOptions.None));
		}
		/// <summary>
		/// Initializes the <see cref="ReliableStorage"/> class from a file.
		/// </summary>
		/// <param name="path">The path to the storage file.</param>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">Invalid storage format.</exception>
		static public ReliableStorage FromFile(string path) => FromFile(path, _defaultStreamBufferSize);

		private readonly Stream _stream;
		private readonly byte[] _metaBuffer;
		private byte _metaPosition;
		private Meta _meta;
		private bool _disposed;

		private ReliableStorage(Stream stream)
		{
			_stream = stream;
			_metaBuffer = new byte[Meta.Serializer._default.Count];
			stream.EnsureLength(_metaSize);
			_ = stream.Seek(0x0, SeekOrigin.Begin);
			_metaPosition = (byte)stream.ReadByte();
			if (_metaPosition == 0x0)
			{
				_ = stream.Read(_metaBuffer, 0x0, Meta.Serializer._default.Count);
				_meta = Meta.Serializer._default.Deserialize(_metaBuffer, 0x0);
			}
			else
			{
				if (_metaPosition != 0xFF)
					throw new ArgumentException("Invalid storage format.");
				_ = stream.Seek(sizeof(byte) + Meta.Serializer._default.Count, SeekOrigin.Begin);
				_ = stream.Read(_metaBuffer, 0x0, Meta.Serializer._default.Count);
				_meta = Meta.Serializer._default.Deserialize(_metaBuffer, 0x0);
			}
			if (_meta._position < 0x0 || _meta._length < 0x0)
				throw new ArgumentException("Invalid storage format.");
		}

		/// <summary>
		/// Gets the length of the value.
		/// </summary>
		public int Length => _meta._length;

		private void EnsureLength() => _stream.EnsureLength(_metaSize + _meta._position + _meta._length);
		/// <summary>
		/// Reads the value from the storage.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes of the value.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">The length of the value is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="ReliableStorage"/> has been closed.</exception>
		public void Read(byte[] buffer, int index)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (_meta._length > buffer.Length - index)
				throw new ArgumentException(string.Format("The length of the value is greater than the number of bytes from the {0} to the end of {1}.", nameof(index), nameof(buffer)));
			if (_disposed)
				throw new ObjectDisposedException(nameof(ReliableStorage));
			if (_meta._length == 0x0)
				return;
			_ = _stream.Seek(_metaSize + _meta._position, SeekOrigin.Begin);
			_ = _stream.Read(buffer, index, _meta._length);
		}
		/// <summary>
		/// Writes the value to the storage.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes of the value.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <param name="count">The number of the bytes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="ReliableStorage"/> has been closed.</exception>
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
				throw new ObjectDisposedException(nameof(ReliableStorage));
			if (count > _meta._position)
			{
				_meta._position += _meta._length;
				EnsureLength();
			}
			else
				_meta._position = 0x0;
			_meta._length = count;
			if (count != 0x0)
			{
				_ = _stream.Seek(_metaSize + _meta._position, SeekOrigin.Begin);
				_stream.Write(buffer, index, count);
			}
			_stream.Flush();
			_metaPosition = (byte)~_metaPosition;
			Meta.Serializer._default.Serialize(_meta, _metaBuffer, 0x0);
			_ = _stream.Seek(_metaPosition == 0x0 ? sizeof(byte) : sizeof(byte) + Meta.Serializer._default.Count, SeekOrigin.Begin);
			_stream.Write(_metaBuffer, 0x0, Meta.Serializer._default.Count);
			_stream.Flush();
			_ = _stream.Seek(0x0, SeekOrigin.Begin);
			_stream.WriteByte(_metaPosition);
			_stream.Flush();
		}
		/// <summary>
		/// Closes the storage and releases all resources used by the <see cref="ReliableStorage"/>.
		/// </summary>
		public void Close()
		{
			_disposed = true;
			_stream.Close();
		}
		void IDisposable.Dispose() => Close();
	}
}