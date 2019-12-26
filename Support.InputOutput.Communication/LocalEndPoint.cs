using Support.Coding.Cryptography;
using Support.Coding.Serialization;
using Support.Threading;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Support.InputOutput.Communication
{
	/// <summary>
	/// Represents a network endpoint that sends and receives data via udp protocol.
	/// </summary>
	public sealed class LocalEndPoint : IDisposable
	{
		private sealed class PostBuffer
		{
			internal byte[] _buffer;
			internal int _count;
		}

		private const int _receiveBufferSize = ushort.MaxValue;

		private readonly Socket _socket;
		private readonly IPEndPoint _localIPEndPoint;
		private readonly byte[] _receiveBuffer;
		private readonly ThreadLocal<PostBuffer> _postBuffer;
		private readonly SendOrPostCallback _receipt;
		private readonly object _disposeLock;
		private bool _isReceiving;
		private EndPoint _remoteIPEndPoint;
		private int _receivedCount;
		private int _gottenCount;
		private Thread _receivingThread;
		private bool _disposed;

		/// <summary>
		/// Initializes the <see cref="LocalEndPoint"/>.
		/// </summary>
		/// <param name="localIPEndPoint">The <see cref="IPEndPoint"/> to associate with the <see cref="Socket"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="localIPEndPoint"/> is <see langword="null"/>.</exception>
		public LocalEndPoint(IPEndPoint localIPEndPoint)
		{
			if (localIPEndPoint == null)
				throw new ArgumentNullException(nameof(localIPEndPoint));
			_socket = new Socket(localIPEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
			_socket.Bind(localIPEndPoint);
			_localIPEndPoint = (IPEndPoint)_socket.LocalEndPoint;
			_receiveBuffer = new byte[_receiveBufferSize];
			_postBuffer = new ThreadLocal<PostBuffer>(() => new PostBuffer());
			_receipt = Receive;
			_disposeLock = new object();
			_isReceiving = true;
			_remoteIPEndPoint = new IPEndPoint(IPAddress.None, IPEndPoint.MinPort);
			TaskManager.Post(_receipt, null, SynchronizationContext.Current);
		}

		/// <summary>
		/// Gets the <see cref="IPEndPoint"/> the <see cref="LocalEndPoint"/> associated with.
		/// </summary>
		public IPEndPoint LocalIPEndPoint => _localIPEndPoint;
		/// <summary>
		/// Gets the <see cref="IPEndPoint"/> of the endpoint the received bytes have been received from.
		/// </summary>
		/// <exception cref="InvalidOperationException">The method has been called when there are no received bytes.</exception>
		public IPEndPoint RemoteIPEndPoint
		{
			get
			{
				if (Thread.CurrentThread != _receivingThread)
					throw new InvalidOperationException("The method has been called when there are no received bytes.");
				return (IPEndPoint)_remoteIPEndPoint;
			}
		}
		/// <summary>
		/// Gets the number of received bytes which are available to get.
		/// </summary>
		/// <exception cref="InvalidOperationException">The method has been called when there are no received bytes.</exception>
		public int Available
		{
			get
			{
				if (Thread.CurrentThread != _receivingThread)
					throw new InvalidOperationException("The method has been called when there are no received bytes.");
				return _receivedCount - _gottenCount;
			}
		}

		/// <summary>
		/// Occurs when bytes are received.
		/// </summary>
		public event EventTracker<LocalEndPoint> Received;

		private void Receive(object state)
		{
			bool received = false;
			for (; ; )
			{
				lock (_disposeLock)
				{
					if (_disposed)
					{
						_isReceiving = false;
						return;
					}
					if (_socket.Available == 0x0)
						break;
					_receivedCount = _socket.ReceiveFrom(_receiveBuffer, 0x0, _receiveBuffer.Length, SocketFlags.None, ref _remoteIPEndPoint);
				}
				received = true;
				_gottenCount = 0x0;
				_receivingThread = Thread.CurrentThread;
				Received?.Invoke(this);
				_receivingThread = null;
			}
			if (received)
				TaskManager.Post(_receipt, null);
			else
				TaskManager.PostYield(_receipt, null);
		}
		/// <summary>
		/// Puts a specified available bytes to a specified buffer.
		/// </summary>
		/// <param name="count">The number of the bytes to put.</param>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of the available bytes.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		public void Get(int count, byte[] buffer, int index)
		{
			if (Thread.CurrentThread != _receivingThread)
				throw new InvalidOperationException("The method has been called before the awaited bytes are received.");
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > _receivedCount - _gottenCount)
				throw new ArgumentException(string.Format("{0} is greater than the number of the available bytes.", nameof(count)));
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
			Array.Copy(_receiveBuffer, _gottenCount, buffer, index, count);
			_gottenCount += count;
		}
		/// <summary>
		/// Puts the available bytes to a specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">The number of the available bytes is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		public void Get(byte[] buffer, int index) => Get(_receivedCount - _gottenCount, buffer, index);
		/// <summary>
		/// Deserializes a specified available bytes to an instance.
		/// </summary>
		/// <typeparam name="T">The type of the instance.</typeparam>
		/// <param name="count">The number of the bytes to deserialize.</param>
		/// <param name="serializer">An <see cref="ISerializer{T}"/> to deserialize the available bytes.</param>
		/// <returns>The deserialized instance.</returns>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of the available bytes.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
		public T Get<T>(int count, ISerializer<T> serializer)
		{
			if (Thread.CurrentThread != _receivingThread)
				throw new InvalidOperationException("The method has been called before the awaited bytes are received.");
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > _receivedCount - _gottenCount)
				throw new ArgumentException(string.Format("{0} is greater than the number of the available bytes.", nameof(count)));
			if (serializer == null)
				throw new ArgumentNullException(nameof(serializer));
			try { return serializer.Deserialize(count, _receiveBuffer, _gottenCount); }
			finally { _gottenCount += count; }
		}
		/// <summary>
		/// Deserializes the available bytes to an instance.
		/// </summary>
		/// <typeparam name="T">The type of the instance.</typeparam>
		/// <param name="serializer">An <see cref="ISerializer{T}"/> to deserialize the available bytes.</param>
		/// <returns>The deserialized instance.</returns>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
		public T Get<T>(ISerializer<T> serializer) => Get(_receivedCount - _gottenCount, serializer);
		/// <summary>
		/// Deserializes a specified available bytes to an instance.
		/// </summary>
		/// <typeparam name="T">The type of the instance.</typeparam>
		/// <param name="serializer">An <see cref="IConstantLengthSerializer{T}"/> to deserialize the available bytes.</param>
		/// <returns>The deserialized instance.</returns>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The number of bytes required for the deserialization is less than 0.</exception>
		/// <exception cref="ArgumentException">The number of bytes required for the deserialization is greater than the number of the available bytes.</exception>
		public T Get<T>(IConstantLengthSerializer<T> serializer)
		{
			if (Thread.CurrentThread != _receivingThread)
				throw new InvalidOperationException("The method has been called before the awaited bytes are received.");
			if (serializer == null)
				throw new ArgumentNullException(nameof(serializer));
			int count = serializer.Count;
			if (count < 0x0)
				throw new ArgumentException("The number of bytes required for the deserialization is less than 0.");
			if (count > _receivedCount - _gottenCount)
				throw new ArgumentException("The number of bytes required for the deserialization is greater than the number of the available bytes.");
			try { return serializer.Deserialize(_receiveBuffer, _gottenCount); }
			finally { _gottenCount += count; }
		}
		/// <summary>
		/// Decrypts a specified available bytes then puts the bytes to a specified buffer.
		/// </summary>
		/// <param name="count">The number of the bytes to decrypt.</param>
		/// <param name="buffer">The buffer to contain the decrypted bytes.</param>
		/// <param name="index">The position of the decrypted bytes in <paramref name="buffer"/>.</param>
		/// <param name="keyBuffer">The buffer to contain the key to decrypt the bytes.</param>
		/// <param name="keyIndex">The position of the key in <paramref name="keyBuffer"/>.</param>
		/// <param name="keyLength">The length of the key.</param>
		/// <param name="cipher">An <see cref="ICipher"/> to decrypt the available bytes.</param>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of the available bytes.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="keyBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyIndex"/> is outside the range of valid indices of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyLength"/> is greater than the number of bytes from <paramref name="keyIndex"/> to the end of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="cipher"/> is <see langword="null"/>.</exception>
		public void Get(int count, byte[] buffer, int index, byte[] keyBuffer, int keyIndex, int keyLength, ICipher cipher)
		{
			if (Thread.CurrentThread != _receivingThread)
				throw new InvalidOperationException("The method has been called before the awaited bytes are received.");
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > _receivedCount - _gottenCount)
				throw new ArgumentException(string.Format("{0} is greater than the number of the available bytes.", nameof(count)));
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
			if (keyBuffer == null)
				throw new ArgumentNullException(nameof(keyBuffer));
			if (keyIndex < 0x0 || keyIndex > keyBuffer.Length)
				throw new ArgumentOutOfRangeException(nameof(keyIndex));
			if (keyLength < 0x0)
				throw new ArgumentOutOfRangeException(nameof(keyLength));
			if (keyLength > keyBuffer.Length - keyIndex)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(keyLength), nameof(keyIndex), nameof(keyBuffer)));
			if (cipher == null)
				throw new ArgumentNullException(nameof(cipher));
			cipher.Decrypt(_receiveBuffer, _gottenCount, count, keyBuffer, keyIndex, keyLength);
			Array.Copy(_receiveBuffer, _gottenCount, buffer, index, count);
			_gottenCount += count;
		}
		/// <summary>
		/// Decrypts the available bytes and puts then puts the bytes to a specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer to contain the decrypted bytes.</param>
		/// <param name="index">The position of the decrypted bytes in <paramref name="buffer"/>.</param>
		/// <param name="keyBuffer">The buffer to contain the key to decrypt the bytes.</param>
		/// <param name="keyIndex">The position of the key in <paramref name="keyBuffer"/>.</param>
		/// <param name="keyLength">The length of the key.</param>
		/// <param name="cipher">An <see cref="ICipher"/> to decrypt the available bytes.</param>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">The number of the available bytes is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="keyBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyIndex"/> is outside the range of valid indices of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyLength"/> is greater than the number of bytes from <paramref name="keyIndex"/> to the end of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="cipher"/> is <see langword="null"/>.</exception>
		public void Get(byte[] buffer, int index, byte[] keyBuffer, int keyIndex, int keyLength, ICipher cipher) => Get(_receivedCount - _gottenCount, buffer, index, keyBuffer, keyIndex, keyLength, cipher);
		/// <summary>
		/// Decrypts a specified available bytes then deserializes the bytes to an instance.
		/// </summary>
		/// <typeparam name="T">The type of the instance.</typeparam>
		/// <param name="count">The number of bytes to decrypt.</param>
		/// <param name="serializer">An <see cref="ISerializer{T}"/> to deserialize the decrypted bytes.</param>
		/// <param name="keyBuffer">The buffer to contain the key to decrypt the bytes.</param>
		/// <param name="keyIndex">The position of the key in <paramref name="keyBuffer"/>.</param>
		/// <param name="keyLength">The length of the key.</param>
		/// <param name="cipher">An <see cref="ICipher"/> to decrypt the available bytes.</param>
		/// <returns>The deserialized instance.</returns>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of the available bytes.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="keyBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyIndex"/> is outside the range of valid indices of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyLength"/> is greater than the number of bytes from <paramref name="keyIndex"/> to the end of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="cipher"/> is <see langword="null"/>.</exception>
		public T Get<T>(int count, ISerializer<T> serializer, byte[] keyBuffer, int keyIndex, int keyLength, ICipher cipher)
		{
			if (Thread.CurrentThread != _receivingThread)
				throw new InvalidOperationException("The method has been called before the awaited bytes are received.");
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > _receivedCount - _gottenCount)
				throw new ArgumentException(string.Format("{0} is greater than the number of the available bytes.", nameof(count)));
			if (serializer == null)
				throw new ArgumentNullException(nameof(serializer));
			if (keyBuffer == null)
				throw new ArgumentNullException(nameof(keyBuffer));
			if (keyIndex < 0x0 || keyIndex > keyBuffer.Length)
				throw new ArgumentOutOfRangeException(nameof(keyIndex));
			if (keyLength < 0x0)
				throw new ArgumentOutOfRangeException(nameof(keyLength));
			if (keyLength > keyBuffer.Length - keyIndex)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(keyLength), nameof(keyIndex), nameof(keyBuffer)));
			if (cipher == null)
				throw new ArgumentNullException(nameof(cipher));
			cipher.Decrypt(_receiveBuffer, _gottenCount, count, keyBuffer, keyIndex, keyLength);
			try { return serializer.Deserialize(count, _receiveBuffer, _gottenCount); }
			finally { _gottenCount += count; }
		}
		/// <summary>
		/// Decrypts the available bytes then deserializes the bytes to an instance.
		/// </summary>
		/// <typeparam name="T">The type of the instance.</typeparam>
		/// <param name="serializer">An <see cref="ISerializer{T}"/> to deserialize the decrypted bytes.</param>
		/// <param name="keyBuffer">The buffer to contain the key to decrypt the bytes.</param>
		/// <param name="keyIndex">The position of the key in <paramref name="keyBuffer"/>.</param>
		/// <param name="keyLength">The length of the key.</param>
		/// <param name="cipher">An <see cref="ICipher"/> to decrypt the available bytes.</param>
		/// <returns>The deserialized instance.</returns>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="keyBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyIndex"/> is outside the range of valid indices of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyLength"/> is greater than the number of bytes from <paramref name="keyIndex"/> to the end of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="cipher"/> is <see langword="null"/>.</exception>
		public T Get<T>(ISerializer<T> serializer, byte[] keyBuffer, int keyIndex, int keyLength, ICipher cipher) => Get(_receivedCount - _gottenCount, serializer, keyBuffer, keyIndex, keyLength, cipher);
		/// <summary>
		/// Decrypts a specified available bytes then deserializes the bytes to an instance.
		/// </summary>
		/// <typeparam name="T">The type of the instance.</typeparam>
		/// <param name="serializer">An <see cref="ISerializer{T}"/> to deserialize the decrypted bytes.</param>
		/// <param name="keyBuffer">The buffer to contain the key to decrypt the bytes.</param>
		/// <param name="keyIndex">The position of the key in <paramref name="keyBuffer"/>.</param>
		/// <param name="keyLength">The length of the key.</param>
		/// <param name="cipher">An <see cref="ICipher"/> to decrypt the available bytes.</param>
		/// <returns>The deserialized instance.</returns>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">The number of bytes required for the deserialization is less than 0.</exception>
		/// <exception cref="ArgumentException">The number of bytes required for the deserialization is greater than the number of the available bytes.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="keyBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyIndex"/> is outside the range of valid indices of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyLength"/> is greater than the number of bytes from <paramref name="keyIndex"/> to the end of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="cipher"/> is <see langword="null"/>.</exception>
		public T Decrypt<T>(IConstantLengthSerializer<T> serializer, byte[] keyBuffer, int keyIndex, int keyLength, ICipher cipher)
		{
			if (Thread.CurrentThread != _receivingThread)
				throw new InvalidOperationException("The method has been called before the awaited bytes are received.");
			if (serializer == null)
				throw new ArgumentNullException(nameof(serializer));
			int count = serializer.Count;
			if (count < 0x0)
				throw new ArgumentException("The number of bytes required for the deserialization is less than 0.");
			if (count > _receivedCount - _gottenCount)
				throw new ArgumentException("The number of bytes required for the deserialization is greater than the number of the available bytes.");
			if (keyBuffer == null)
				throw new ArgumentNullException(nameof(keyBuffer));
			if (keyIndex < 0x0 || keyIndex > keyBuffer.Length)
				throw new ArgumentOutOfRangeException(nameof(keyIndex));
			if (keyLength < 0x0)
				throw new ArgumentOutOfRangeException(nameof(keyLength));
			if (keyLength > keyBuffer.Length - keyIndex)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(keyLength), nameof(keyIndex), nameof(keyBuffer)));
			if (cipher == null)
				throw new ArgumentNullException(nameof(cipher));
			cipher.Decrypt(_receiveBuffer, _gottenCount, count, keyBuffer, keyIndex, keyLength);
			try { return serializer.Deserialize(_receiveBuffer, _gottenCount); }
			finally { _gottenCount += count; }
		}
		/// <summary>
		/// Skips a specified number of the available bytes.
		/// </summary>
		/// <param name="count">A number of the available bytes to skip.</param>
		/// <exception cref="InvalidOperationException">The method has been called when there are no received bytes.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of the available bytes.</exception>
		public void Skip(int count)
		{
			if (Thread.CurrentThread != _receivingThread)
				throw new InvalidOperationException("The method has been called before the awaited bytes are received.");
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > _receivedCount - _gottenCount)
				throw new ArgumentException(string.Format("{0} is greater than the number of the available bytes.", nameof(count)));
			_gottenCount += count;
		}
		/// <summary>
		/// Prepares a specified bytes to send.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <param name="count">The number of the bytes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from the <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		public void Post(byte[] buffer, int index, int count)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
			PostBuffer postBuffer = _postBuffer.Value;
			_ = ArrayHelper.EnsureLength(ref postBuffer._buffer, postBuffer._count + count);
			Array.Copy(buffer, index, postBuffer._buffer, postBuffer._count, count);
			postBuffer._count += count;
		}
		/// <summary>
		/// Serializes a specified instance to a sequence of bytes and prepares the bytes to send.
		/// </summary>
		/// <typeparam name="T">The type of the instance.</typeparam>
		/// <param name="instance">The instance to serialize.</param>
		/// <param name="serializer">An <see cref="ISerializer{T}"/> to serialize <paramref name="instance"/> to the bytes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
		public void Post<T>(T instance, ISerializer<T> serializer)
		{
			if (serializer == null)
				throw new ArgumentNullException(nameof(serializer));
			PostBuffer postBuffer = _postBuffer.Value;
			int count = serializer.Count(instance);
			_ = ArrayHelper.EnsureLength(ref postBuffer._buffer, postBuffer._count + count);
			try { serializer.Serialize(instance, postBuffer._buffer, postBuffer._count); }
			finally { postBuffer._count += count; }
		}
		/// <summary>
		/// Encrypts a specified bytes then prepares the bytes to send.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <param name="count">The number of the bytes.</param>
		/// <param name="keyBuffer">The buffer to contain the key to encrypt the bytes.</param>
		/// <param name="keyIndex">The position of the key in <paramref name="keyBuffer"/>.</param>
		/// <param name="keyLength">The length of the key.</param>
		/// <param name="cipher">An <see cref="ICipher"/> to decrypt the available bytes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="keyBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyIndex"/> is outside the range of valid indices of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyLength"/> is greater than the number of bytes from <paramref name="keyIndex"/> to the end of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="cipher"/> is <see langword="null"/>.</exception>
		public void Post(byte[] buffer, int index, int count, byte[] keyBuffer, int keyIndex, int keyLength, ICipher cipher)
		{
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (count < 0x0)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (count > buffer.Length - index)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(count), nameof(index), nameof(buffer)));
			if (keyBuffer == null)
				throw new ArgumentNullException(nameof(keyBuffer));
			if (keyIndex < 0x0 || keyIndex > keyBuffer.Length)
				throw new ArgumentOutOfRangeException(nameof(keyIndex));
			if (keyLength < 0x0)
				throw new ArgumentOutOfRangeException(nameof(keyLength));
			if (keyLength > keyBuffer.Length - keyIndex)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(keyLength), nameof(keyIndex), nameof(keyBuffer)));
			if (cipher == null)
				throw new ArgumentNullException(nameof(cipher));
			PostBuffer postBuffer = _postBuffer.Value;
			_ = ArrayHelper.EnsureLength(ref postBuffer._buffer, postBuffer._count + count);
			Array.Copy(buffer, index, postBuffer._buffer, postBuffer._count, count);
			cipher.Encrypt(postBuffer._buffer, postBuffer._count, count, keyBuffer, keyIndex, keyLength);
			postBuffer._count += count;
		}
		/// <summary>
		/// Serializes a specified instance to a sequence of bytes then encrypts then prepares the bytes to send.
		/// </summary>
		/// <typeparam name="T">The type of the instance.</typeparam>
		/// <param name="instance">The instance to serialize.</param>
		/// <param name="serializer">An <see cref="ISerializer{T}"/> to serialize <paramref name="instance"/> to the bytes.</param>
		/// <param name="keyBuffer">The buffer to contain the key to decrypt the bytes.</param>
		/// <param name="keyIndex">The position of the key in <paramref name="keyBuffer"/>.</param>
		/// <param name="keyLength">The length of the key.</param>
		/// <param name="cipher">An <see cref="ICipher"/> to decrypt the available bytes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="keyBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyIndex"/> is outside the range of valid indices of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyLength"/> is greater than the number of bytes from <paramref name="keyIndex"/> to the end of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="cipher"/> is <see langword="null"/>.</exception>
		public void Post<T>(T instance, ISerializer<T> serializer, byte[] keyBuffer, int keyIndex, int keyLength, ICipher cipher)
		{
			if (serializer == null)
				throw new ArgumentNullException(nameof(serializer));
			if (keyBuffer == null)
				throw new ArgumentNullException(nameof(keyBuffer));
			if (keyIndex < 0x0 || keyIndex > keyBuffer.Length)
				throw new ArgumentOutOfRangeException(nameof(keyIndex));
			if (keyLength < 0x0)
				throw new ArgumentOutOfRangeException(nameof(keyLength));
			if (keyLength > keyBuffer.Length - keyIndex)
				throw new ArgumentException(string.Format("{0} is greater than the number of bytes from {1} to the end of {2}.", nameof(keyLength), nameof(keyIndex), nameof(keyBuffer)));
			if (cipher == null)
				throw new ArgumentNullException(nameof(cipher));
			PostBuffer postBuffer = _postBuffer.Value;
			int count = serializer.Count(instance);
			_ = ArrayHelper.EnsureLength(ref postBuffer._buffer, postBuffer._count + count);
			serializer.Serialize(instance, postBuffer._buffer, postBuffer._count);
			cipher.Encrypt(postBuffer._buffer, postBuffer._count, count, keyBuffer, keyIndex, keyLength);
			postBuffer._count += count;
		}
		/// <summary>
		/// Sends the posted bytes.
		/// </summary>
		/// <param name="remoteEndPoint">An <see cref="IPEndPoint"/> the posted bytes are to be sent to.</param>
		/// <exception cref="ArgumentNullException"><paramref name="remoteEndPoint"/> is <see langword="null"/>.</exception>
		public void Send(IPEndPoint remoteEndPoint)
		{
			if (remoteEndPoint == null)
				throw new ArgumentNullException(nameof(remoteEndPoint));
			PostBuffer postBuffer = _postBuffer.Value;
			if (postBuffer._count == 0x0)
				return;
			lock (_disposeLock)
			{
				if (_disposed)
					return;
				_ = _socket.SendTo(postBuffer._buffer, 0, postBuffer._count, SocketFlags.None, remoteEndPoint);
			}
			postBuffer._count = 0x0;
		}
		/// <summary>
		/// Releases all resources used by the <see cref="LocalEndPoint"/>.
		/// </summary>
		public void Close()
		{
			lock (_disposeLock)
			{
				if (_disposed)
					return;
				_disposed = true;
			}
			while (_isReceiving)
				Thread.Sleep(1);
			_socket.Close();
		}
		void IDisposable.Dispose() => Close();
	}
}