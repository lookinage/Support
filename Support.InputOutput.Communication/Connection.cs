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
	/// Represents a connection that works via TCP.
	/// </summary>
	public sealed class Connection : IDisposable
	{
		private sealed class PostBuffer
		{
			internal byte[] _buffer;
			internal int _count;
		}

		static private Socket InitializeSocket(IPEndPoint localIPEndPoint, IPEndPoint remoteIPEndPoint)
		{
			if (localIPEndPoint == null)
				throw new ArgumentNullException(nameof(localIPEndPoint));
			if (remoteIPEndPoint == null)
				throw new ArgumentNullException(nameof(remoteIPEndPoint));
			if (localIPEndPoint.AddressFamily != remoteIPEndPoint.AddressFamily)
				throw new ArgumentException(string.Format("{0} address family does not equal to {1} address family.", nameof(remoteIPEndPoint), nameof(localIPEndPoint)));
			Socket socket = new Socket(localIPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			socket.Bind(localIPEndPoint);
			socket.Connect(remoteIPEndPoint);
			return socket;
		}

		private readonly object _sendLock;
		private readonly object _lostLock;
		private readonly object _disposeLock;
		private readonly ThreadLocal<PostBuffer> _postBuffer;
		private readonly SendOrPostCallback _communication;
		private readonly Socket _socket;
		private readonly IPEndPoint _localIPEndPoint;
		private readonly IPEndPoint _remoteIPEndPoint;
		private int _receiveBufferSize;
		private int _sendBufferSize;
		private byte[] _receiveBuffer;
		private byte[] _sendBuffer;
		private int _awaitedCount;
		private int _receivedCount;
		private int _sendCount;
		private int _sentCount;
		private Thread _receivingThread;
		private bool _lost;
		private bool _disposed;

		private Connection()
		{
			_sendLock = new object();
			_lostLock = new object();
			_disposeLock = new object();
			_postBuffer = new ThreadLocal<PostBuffer>(() => new PostBuffer());
			_communication = Communicate;
		}
		internal Connection(Socket socket) : this()
		{
			socket.Blocking = false;
			_socket = socket;
			_localIPEndPoint = (IPEndPoint)socket.LocalEndPoint;
			_remoteIPEndPoint = (IPEndPoint)socket.RemoteEndPoint;
			_receiveBufferSize = socket.ReceiveBufferSize;
			_sendBufferSize = socket.SendBufferSize;
		}
		/// <summary>
		/// Initializes the <see cref="Connection"/>.
		/// </summary>
		/// <param name="localIPEndPoint">An <see cref="IPEndPoint"/> to associate with a <see cref="Socket"/> to provide the connection.</param>
		/// <param name="remoteIPEndPoint">An <see cref="IPEndPoint"/> of the remote host to connect.</param>
		/// <exception cref="ArgumentNullException"><paramref name="localIPEndPoint"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="remoteIPEndPoint"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="remoteIPEndPoint"/> address family does not equal to <paramref name="localIPEndPoint"/> address family.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
		public Connection(IPEndPoint localIPEndPoint, IPEndPoint remoteIPEndPoint) : this(InitializeSocket(localIPEndPoint, remoteIPEndPoint)) => TaskManager.Post(_communication, null, SynchronizationContext.Current);

		/// <summary>
		/// Gets a value that indicates whether the connection is working.
		/// </summary>
		public bool Connected => _socket.Connected;
		/// <summary>
		/// Gets the local endpoint of the connection.
		/// </summary>
		public IPEndPoint LocalIPEndPoint => _localIPEndPoint;
		/// <summary>
		/// Gets the remote endpoint of the connection.
		/// </summary>
		public IPEndPoint RemoteIPEndPoint => _remoteIPEndPoint;
		/// <summary>
		/// Gets or sets a value of <see cref="Socket.ReceiveBufferSize"/> property of the <see cref="Socket"/> used by the <see cref="Connection"/>.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 0.</exception>
		public int ReceiveBufferSize
		{
			get => _receiveBufferSize;
			set
			{
				if (value < 0x0)
					throw new ArgumentOutOfRangeException(nameof(value));
				_receiveBufferSize = value;
				lock (_disposeLock)
				{
					if (_socket == null || _disposed)
						return;
					_socket.ReceiveBufferSize = value;
				}
			}
		}
		/// <summary>
		/// Gets or sets a value of <see cref="Socket.SendBufferSize"/> property of the <see cref="Socket"/> used by the <see cref="Connection"/>.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 0.</exception>
		public int SendBufferSize
		{
			get => _sendBufferSize;
			set
			{
				if (value < 0x0)
					throw new ArgumentOutOfRangeException(nameof(value));
				_sendBufferSize = value;
				lock (_disposeLock)
				{
					if (_socket == null || _disposed)
						return;
					_socket.SendBufferSize = value;
				}
			}
		}

		/// <summary>
		/// Occurs when the awaiting bytes are received.
		/// </summary>
		public event EventTracker<Connection> Received;
		/// <summary>
		/// Occurs when the connection is lost.
		/// </summary>
		public event EventTracker<Connection, ConnectionLostEventArgument> Lost;

		private void InvokeLost(SocketError error)
		{
			lock (_lostLock)
			{
				if (_lost)
					return;
				_lost = true;
			}
			Lost?.Invoke(this, new ConnectionLostEventArgument(error));
		}
		private void Communicate(object state)
		{
			bool? result = Communicate();
			if (result == null)
				return;
			if (result == false)
			{
				TaskManager.PostYield(_communication, null);
				return;
			}
			TaskManager.Post(_communication, null);
		}
		internal bool? Communicate()
		{
			bool communicated = false;
			SocketError error;
			int count;
			if (_receivedCount != _awaitedCount)
				for (; ; )
				{
					lock (_disposeLock)
					{
						if (_disposed)
							return null;
						count = _socket.Receive(_receiveBuffer, _receivedCount, _awaitedCount - _receivedCount, SocketFlags.None, out error);
					}
					if (error == SocketError.WouldBlock)
						break;
					if (error != SocketError.Success)
					{
						InvokeLost(error);
						return null;
					}
					if (count == 0x0)
					{
						InvokeLost(SocketError.HostDown);
						return null;
					}
					_receivedCount += count;
					communicated = true;
					if (_receivedCount != _awaitedCount)
						continue;
					_receivingThread = Thread.CurrentThread;
					Received?.Invoke(this);
					_receivingThread = null;
					if (_receivedCount != _awaitedCount)
						continue;
					if (_awaitedCount != 0x0)
					{
						_awaitedCount = 0x0;
						_receivedCount = 0x0;
					}
					break;
				}
			if (_sentCount != _sendCount)
				for (; ; )
				{
					lock (_disposeLock)
					{
						if (_disposed)
							return null;
						count = _socket.Send(_sendBuffer, _sentCount, _sendCount - _sentCount, SocketFlags.None, out error);
					}
					if (error != SocketError.Success)
					{
						InvokeLost(error);
						return null;
					}
					if (count == 0x0)
						break;
					_sentCount += count;
					communicated = true;
					lock (_sendLock)
						if (_sentCount == _sendCount)
						{
							_sendCount = 0x0;
							_sentCount = 0x0;
							break;
						}
				}
			return communicated;
		}
		/// <summary>
		/// Sets a number of bytes to await.
		/// </summary>
		/// <param name="count">The number of bytes to await.</param>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is less than 1.</exception>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
		public void Await(int count)
		{
			if (count < 0x1)
				throw new ArgumentOutOfRangeException(nameof(count));
			if (Thread.CurrentThread != _receivingThread && _awaitedCount != 0x0)
				throw new InvalidOperationException("The method has been called before the awaited bytes are received.");
			if (_disposed)
				throw new ObjectDisposedException(nameof(Connection));
			_ = ArrayHelper.EnsureLength(ref _receiveBuffer, count);
			_awaitedCount = count;
			_receivedCount = 0x0;
		}
		/// <summary>
		/// Decrypts the received bytes.
		/// </summary>
		/// <param name="keyBuffer">The buffer to contain the key to decrypt the bytes.</param>
		/// <param name="keyIndex">The position of the key in <paramref name="keyBuffer"/>.</param>
		/// <param name="keyLength">The length of the key.</param>
		/// <param name="cipher">An <see cref="ICipher"/> to decrypt the received bytes.</param>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="keyBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyIndex"/> is outside the range of valid indices of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyLength"/> is greater than the number of bytes from <paramref name="keyIndex"/> to the end of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="cipher"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
		public void Decrypt(byte[] keyBuffer, int keyIndex, int keyLength, ICipher cipher)
		{
			if (Thread.CurrentThread != _receivingThread)
				throw new InvalidOperationException("The method has been called before the awaited bytes are received.");
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
			if (_disposed)
				throw new ObjectDisposedException(nameof(Connection));
			cipher.Decrypt(_receiveBuffer, 0x0, _receivedCount, keyBuffer, keyIndex, keyLength);
		}
		/// <summary>
		/// Puts the received bytes to a specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer to contain the bytes.</param>
		/// <param name="index">The position of the bytes in <paramref name="buffer"/>.</param>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of <paramref name="buffer"/>.</exception>
		/// <exception cref="ArgumentException">The number of the received bytes is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
		public void Get(byte[] buffer, int index)
		{
			if (Thread.CurrentThread != _receivingThread)
				throw new InvalidOperationException("The method has been called before the awaited bytes are received.");
			if (buffer == null)
				throw new ArgumentNullException(nameof(buffer));
			if (index < 0x0 || index > buffer.Length)
				throw new ArgumentOutOfRangeException(nameof(index));
			if (_receivedCount > buffer.Length - index)
				throw new ArgumentException(string.Format("The number of the received bytes is greater than the number of bytes from {0} to the end of {1}.", nameof(index), nameof(buffer)));
			if (_disposed)
				throw new ObjectDisposedException(nameof(Connection));
			Array.Copy(_receiveBuffer, 0x0, buffer, index, _receivedCount);
		}
		/// <summary>
		/// Deserializes the received bytes to an instance.
		/// </summary>
		/// <typeparam name="T">The type of the instance.</typeparam>
		/// <param name="serializer">An <see cref="ISerializer{T}"/> to deserialize the received bytes.</param>
		/// <returns>The deserialized instance.</returns>
		/// <exception cref="InvalidOperationException">The method has been called when the awaited bytes are not received.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
		public T Get<T>(ISerializer<T> serializer)
		{
			if (Thread.CurrentThread != _receivingThread)
				throw new InvalidOperationException("The method has been called before the awaited bytes are received.");
			if (serializer == null)
				throw new ArgumentNullException(nameof(serializer));
			if (_disposed)
				throw new ObjectDisposedException(nameof(Connection));
			return serializer.Deserialize(_receivedCount, _receiveBuffer, 0x0);
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
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
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
			if (_disposed)
				throw new ObjectDisposedException(nameof(Connection));
			PostBuffer postBuffer = _postBuffer.Value;
			_ = ArrayHelper.EnsureLength(ref postBuffer._buffer, count);
			Array.Copy(buffer, index, postBuffer._buffer, 0x0, postBuffer._count = count);
		}
		/// <summary>
		/// Serializes a specified instance to a sequence of bytes and prepares the bytes to send.
		/// </summary>
		/// <typeparam name="T">The type of the instance.</typeparam>
		/// <param name="instance">The instance to serialize.</param>
		/// <param name="serializer">An <see cref="ISerializer{T}"/> to serialize <paramref name="instance"/> to the bytes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="serializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
		public void Post<T>(T instance, ISerializer<T> serializer)
		{
			if (serializer == null)
				throw new ArgumentNullException(nameof(serializer));
			if (_disposed)
				throw new ObjectDisposedException(nameof(Connection));
			PostBuffer postBuffer = _postBuffer.Value;
			postBuffer._count = serializer.Count(instance);
			_ = ArrayHelper.EnsureLength(ref postBuffer._buffer, postBuffer._count);
			serializer.Serialize(instance, postBuffer._buffer, 0x0);
		}
		/// <summary>
		/// Encrypts the posted bytes.
		/// </summary>
		/// <param name="keyBuffer">The buffer to contain the key to encrypt the bytes.</param>
		/// <param name="keyIndex">The position of the key in <paramref name="keyBuffer"/>.</param>
		/// <param name="keyLength">The length of the key.</param>
		/// <param name="cipher">An <see cref="ICipher"/> to encrypt the posted bytes.</param>
		/// <exception cref="ArgumentNullException"><paramref name="keyBuffer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyIndex"/> is outside the range of valid indices of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="keyLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentException"><paramref name="keyLength"/> is greater than the number of bytes from <paramref name="keyIndex"/> to the end of <paramref name="keyBuffer"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="cipher"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
		public void Encrypt(byte[] keyBuffer, int keyIndex, int keyLength, ICipher cipher)
		{
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
			if (_disposed)
				throw new ObjectDisposedException(nameof(Connection));
			PostBuffer postBuffer = _postBuffer.Value;
			if (postBuffer._count == 0x0)
				return;
			cipher.Encrypt(postBuffer._buffer, 0x0, postBuffer._count, keyBuffer, keyIndex, keyLength);
		}
		/// <summary>
		/// Sends the posted bytes.
		/// </summary>
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
		public void Send()
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(Connection));
			PostBuffer postBuffer = _postBuffer.Value;
			if (postBuffer._count == 0x0)
				return;
			lock (_sendLock)
			{
				_ = ArrayHelper.EnsureLength(ref _sendBuffer, _sendCount + postBuffer._count);
				Array.Copy(postBuffer._buffer, 0x0, _sendBuffer, _sendCount, postBuffer._count);
				_sendCount += postBuffer._count;
			}
			postBuffer._count = 0x0;
		}
		/// <summary>
		/// Closes the connection and releases all resources used by the <see cref="Connection"/>.
		/// </summary>
		public void Close()
		{
			InvokeLost(SocketError.Success);
			lock (_disposeLock)
			{
				if (_disposed)
					return;
				_disposed = true;
			}
			_socket.Close();
		}
		void IDisposable.Dispose() => Close();
	}
}