using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Represents a network local endpoint that invokes remote procedures on a remote endpoint and executes local procedures which are invoked by a remote endpoint.
	/// </summary>
	/// <typeparam name="TIPEndPoint">The type of IP endpoints.</typeparam>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	public abstract class Listener<TIPEndPoint, TData> : IDisposable where TIPEndPoint : struct, IIPEndPoint
	{
		private readonly LocalEndPoint<TIPEndPoint, TData> _localEndPoint;
		private readonly ConnectionListener _listener;
		private readonly object _connectionLock;
		private readonly Dictionary<TIPEndPoint, Connection<TIPEndPoint, TData>> _connections;
		private readonly EventTracker<Connection<TIPEndPoint, TData>, ConnectionLostEventArgument> _connectionLoss;
		private bool _disposed;

		/// <summary>
		/// Initializes the <see cref="Listener{TIPEndPoint, TData}"/>.
		/// </summary>
		/// <param name="localEndPoint">A local endpoint to associate with accepted connections.</param>
		/// <param name="localIPEndPoint">An IP endpoint to associate with a <see cref="Socket"/> to listen incoming connections.</param>
		/// <exception cref="ArgumentNullException"><paramref name="localEndPoint"/> is <see langword="null"/>.</exception>
		public Listener(LocalEndPoint<TIPEndPoint, TData> localEndPoint, TIPEndPoint localIPEndPoint)
		{
			_localEndPoint = localEndPoint ?? throw new ArgumentNullException(nameof(localEndPoint));
			_listener = new ConnectionListener(localIPEndPoint.CreateIPEndPoint());
			_listener.Accepted += TCPListener_Accepted;
			_connectionLock = new object();
			_connections = new Dictionary<TIPEndPoint, Connection<TIPEndPoint, TData>>();
			_connectionLoss = Connection_Lost;
		}

		/// <summary>
		/// Occurs when a connection is accepted.
		/// </summary>
		public event EventTracker<Listener<TIPEndPoint, TData>, ConnectionAcceptedEventArgument<TIPEndPoint, TData>> Accepted;

		private void TCPListener_Accepted(ConnectionListener source, ConnectionAcceptedEventArgument argument)
		{
			Connection<TIPEndPoint, TData> connection = new Connection<TIPEndPoint, TData>(_localEndPoint, argument.Connection);
			lock (_connectionLock)
				_connections.Add(connection.RemoteIPEndPoint, connection);
			connection.Lost += _connectionLoss;
			Accepted?.Invoke(this, new ConnectionAcceptedEventArgument<TIPEndPoint, TData>(connection));
		}
		private void Connection_Lost(Connection<TIPEndPoint, TData> source, ConnectionLostEventArgument argument)
		{
			lock (_connectionLock)
				_ = _connections.Remove(source.RemoteIPEndPoint);
		}
		/// <summary>
		/// Places the <see cref="Listener{TIPEndPoint, TData}"/> in a listening state.
		/// </summary>
		/// <param name="backlog">The maximum length of the pending connection queue.</param>
		/// <exception cref="ObjectDisposedException">The <see cref="Listener{TIPEndPoint, TData}"/> has been closed.</exception>
		public void Listen(int backlog)
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(Listener<TIPEndPoint, TData>));
			_listener.Listen(backlog);
		}
		/// <summary>
		/// Stops accepting of connections and closes all accepted connections.
		/// </summary>
		public void Close()
		{
			_listener.Close();
			_disposed = true;
		}
		void IDisposable.Dispose() => Close();
	}
}