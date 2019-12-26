using Support.Coding.Serialization.System;
using System;
using System.Net.Sockets;

namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Represents a connection that provides remote procedure calling.
	/// </summary>
	/// <typeparam name="TIPEndPoint">The type of IP endpoints.</typeparam>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	public sealed class Connection<TIPEndPoint, TData> : IDisposable where TIPEndPoint : struct, IIPEndPoint
	{
		private readonly object _lostLock;
		private readonly object _disposeLock;
		private bool _receiving;
		private int? _localProcedureKey;
		private bool _lost;
		private bool _disposed;
		internal readonly LocalEndPoint<TIPEndPoint, TData> _localEndPoint;
		internal readonly Connection _connection;
		internal readonly TIPEndPoint _localIPEndPoint;
		internal readonly TIPEndPoint _remoteIPEndPoint;
		internal int? _localProcedureArgumentLength;
		/// <summary>
		/// The data of the connection.
		/// </summary>
		public TData Data;

		private Connection()
		{
			_lostLock = new object();
			_disposeLock = new object();
		}
		internal Connection(LocalEndPoint<TIPEndPoint, TData> localEndPoint, Connection connection) : this()
		{
			_localEndPoint = localEndPoint ?? throw new ArgumentNullException(nameof(localEndPoint));
			_connection = connection;
			_localIPEndPoint = localEndPoint._ipEndPointConverter(connection.LocalIPEndPoint);
			_remoteIPEndPoint = localEndPoint._ipEndPointConverter(connection.RemoteIPEndPoint);
		}
		/// <summary>
		/// Initializes the <see cref="Connection{TIPEndPoint, TData}"/>.
		/// </summary>
		/// <param name="localEndPoint">A local endpoint to associate with the connection.</param>
		/// <param name="localIPEndPoint">An IP endpoint to associate with a <see cref="Socket"/> to provide the connection.</param>
		/// <param name="remoteIPEndPoint">An IP endpoint of the remote host to connect.</param>
		/// <exception cref="ArgumentNullException"><paramref name="localEndPoint"/> is <see langword="null"/>.</exception>
		public Connection(LocalEndPoint<TIPEndPoint, TData> localEndPoint, TIPEndPoint localIPEndPoint, TIPEndPoint remoteIPEndPoint) : this(localEndPoint, new Connection(localIPEndPoint.CreateIPEndPoint(), remoteIPEndPoint.CreateIPEndPoint())) { }

		/// <summary>
		/// Gets a value that indicates whether the connection is working.
		/// </summary>
		public bool Connected => _connection.Connected;
		/// <summary>
		/// Gets the local IP endpoint of the connection.
		/// </summary>
		public TIPEndPoint LocalIPEndPoint => _localIPEndPoint;
		/// <summary>
		/// Gets the remote IP endpoint of the connection.
		/// </summary>
		public TIPEndPoint RemoteIPEndPoint => _remoteIPEndPoint;
		/// <summary>
		/// Gets or sets a value of <see cref="Socket.ReceiveBufferSize"/> property of the <see cref="Socket"/> used by the connection.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 0.</exception>
		public int ReceiveBufferSize
		{
			get => _connection.ReceiveBufferSize;
			set
			{
				if (value < 0x0)
					throw new ArgumentOutOfRangeException(nameof(value));
				_connection.ReceiveBufferSize = value;
			}
		}
		/// <summary>
		/// Gets or sets a value of <see cref="Socket.SendBufferSize"/> property of the <see cref="Socket"/> used by the connection.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">The specified value is less than 0.</exception>
		public int SendBufferSize
		{
			get => _connection.SendBufferSize;
			set
			{
				if (value < 0x0)
					throw new ArgumentOutOfRangeException(nameof(value));
				_connection.SendBufferSize = value;
			}
		}

		/// <summary>
		/// Occurs when the connection is lost.
		/// </summary>
		public event EventTracker<Connection<TIPEndPoint, TData>, ConnectionLostEventArgument> Lost;

		private void InvokeLost(ConnectionLostError reason, SocketError error)
		{
			lock (_lostLock)
			{
				if (_lost)
					return;
				_lost = true;
			}
			_connection.Received -= Connection_Received;
			_connection.Lost -= Connection_Lost;
			Lost?.Invoke(this, new ConnectionLostEventArgument(reason, error));
		}
		private void Connection_Received(Connection source)
		{
			if (_localProcedureKey == null)
				_localProcedureKey = _connection.Get(Int32SerializerBuilder.Default);
			if (!_localEndPoint._localProcedures.TryGetValue(_localProcedureKey.Value, out LocalProcedureBase<TIPEndPoint, TData> localProcedure))
			{
				InvokeLost(ConnectionLostError.LocalProcedureNotFound);
				return;
			}
			if (!localProcedure.Invoke(this, _localEndPoint._synchronizationContext))
				return;
			_localProcedureKey = null;
			_connection.Await(Int32SerializerBuilder.Default.Count);
		}
		private void Connection_Lost(Connection source, Communication.ConnectionLostEventArgument argument) => InvokeLost(ConnectionLostError.SocketError, argument.Error);
		internal void InvokeLost(ConnectionLostError reason) => InvokeLost(reason, SocketError.Success);
		/// <summary>
		/// Allows to receive remote procedure calls.
		/// </summary>
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
		public void Receive()
		{
			lock (_disposeLock)
			{
				if (_disposed)
					throw new ObjectDisposedException(nameof(Connection));
				if (_receiving)
					return;
				_receiving = true;
			}
			_connection.Received += Connection_Received;
			_connection.Lost += Connection_Lost;
			_connection.Await(Int32SerializerBuilder.Default.Count);
		}
		/// <summary>
		/// Invokes a procedure on a remote endpoint of the connection.
		/// </summary>
		/// <param name="procedure">The procedure that is to be called on the remote endpoint of the connection.</param>
		/// <exception cref="ArgumentNullException"><paramref name="procedure"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
		public void Call(RemoteProcedure procedure)
		{
			if (procedure == null)
				throw new ArgumentNullException(nameof(procedure));
			if (_disposed)
				throw new ObjectDisposedException(nameof(Connection));
			_connection.Post(procedure.Key, Int32SerializerBuilder.Default);
			_connection.Send();
		}
		/// <summary>
		/// Invokes a procedure on a remote endpoint of the connection passing a specified argument.
		/// </summary>
		/// <typeparam name="T">The type of an argument that is passed for the procedure execution.</typeparam>
		/// <param name="procedure">The procedure that is to be called on the remote endpoint of the connection.</param>
		/// <param name="argument">An argument that is passed for the remote procedure execution.</param>
		/// <exception cref="ArgumentNullException"><paramref name="procedure"/> is <see langword="null"/>.</exception>
		/// <exception cref="ObjectDisposedException">The <see cref="Connection"/> has been closed.</exception>
		public void Call<T>(RemoteProcedure<T> procedure, T argument)
		{
			if (procedure == null)
				throw new ArgumentNullException(nameof(procedure));
			if (_disposed)
				throw new ObjectDisposedException(nameof(Connection));
			_connection.Post(procedure.Key, Int32SerializerBuilder.Default);
			_connection.Send();
			_connection.Post(procedure.ArgumentSerializer.Count(argument), Int32SerializerBuilder.Default);
			_connection.Send();
			_connection.Post(argument, procedure.ArgumentSerializer);
			_connection.Send();
		}
		/// <summary>
		/// Closes the connection and releases all resources used by the <see cref="Connection{TIPEndPoint, TData}"/>.
		/// </summary>
		public void Close()
		{
			InvokeLost(ConnectionLostError.None);
			lock (_disposeLock)
			{
				if (_disposed)
					return;
				_disposed = true;
			}
			_connection.Close();
		}
		void IDisposable.Dispose() => Close();
	}
}