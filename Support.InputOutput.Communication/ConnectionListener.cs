using Support.Threading;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Support.InputOutput.Communication
{
	/// <summary>
	/// Represents a network endpoint that listens incoming connections using TCP.
	/// </summary>
	public sealed class ConnectionListener : IDisposable
	{
		private readonly Socket _acceptSocket;
		private readonly object _disposeLock;
		private readonly Stack<Connection> _connections;
		private readonly Thread _acceptingThread;
		private readonly SendOrPostCallback _communication;
		private readonly SendOrPostCallback _communicationManagement;
		private bool _isListening;
		private bool _isManagingCommunications;
		private int _communicationCount;
		private bool _disposed;

		/// <summary>
		/// Initializes the <see cref="ConnectionListener"/>.
		/// </summary>
		/// <param name="localIPEndPoint">An <see cref="IPEndPoint"/> to associate with a <see cref="Socket"/> to listen incoming connections.</param>
		/// <exception cref="ArgumentNullException"><paramref name="localIPEndPoint"/> is <see langword="null"/>.</exception>
		public ConnectionListener(IPEndPoint localIPEndPoint)
		{
			if (localIPEndPoint == null)
				throw new ArgumentNullException(nameof(localIPEndPoint));
			_acceptSocket = new Socket(localIPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			_acceptSocket.Bind(localIPEndPoint);
			_disposeLock = new object();
			_connections = new Stack<Connection>();
			_acceptingThread = new Thread(Accept);
			_communication = Communicate;
			_communicationManagement = ManageCommunications;
		}

		/// <summary>
		/// Occurs when a connection is accepted.
		/// </summary>
		public event EventTracker<ConnectionListener, ConnectionAcceptedEventArgument> Accepted;

		private void Accept(object state)
		{
			for (; ; )
			{
				Socket socket;
				try { socket = _acceptSocket.Accept(); }
				catch { return; }
				Connection connection = new Connection(socket);
				Accepted?.Invoke(this, new ConnectionAcceptedEventArgument(connection));
				lock (_connections)
					_connections.Push(connection);
			}
		}
		private void Communicate(object state)
		{
			Connection connection = (Connection)state;
			bool? result = connection.Communicate();
			if (result == true)
			{
				TaskManager.Post(_communication, connection);
				return;
			}
			lock (_connections)
			{
				_communicationCount--;
				if (result == false)
					_connections.Push(connection);
			}
		}
		private void ManageCommunications(object state)
		{
			if (_disposed)
			{
				_isManagingCommunications = false;
				return;
			}
			lock (_connections)
			{
				_communicationCount += _connections.Count;
				foreach (Connection connection in _connections)
					TaskManager.Post(_communication, connection);
				_connections.Clear();
			}
			TaskManager.PostYield(_communicationManagement, null);
		}
		/// <summary>
		/// Places the <see cref="ConnectionListener"/> in a listening state to accept connections.
		/// </summary>
		/// <param name="backlog">The maximum length of the pending connection queue.</param>
		/// <exception cref="ObjectDisposedException">The <see cref="ConnectionListener"/> has been closed.</exception>
		public void Listen(int backlog)
		{
			lock (_disposeLock)
			{
				if (_disposed)
					throw new ObjectDisposedException(nameof(ConnectionListener));
				if (_isListening)
					return;
				_acceptSocket.Listen(backlog);
				_acceptingThread.Start();
				_isListening = true;
			}
			TaskManager.Post(_communicationManagement, null, SynchronizationContext.Current);
			_isManagingCommunications = true;
		}
		/// <summary>
		/// Stops accepting of connections and closes all accepted connections.
		/// </summary>
		public void Close()
		{
			lock (_disposeLock)
			{
				if (_disposed)
					return;
				_disposed = true;
			}
			_acceptSocket.Close();
			while (_acceptingThread.IsAlive || _isManagingCommunications || _communicationCount != 0x0)
				Thread.Sleep(1);
			foreach (Connection connection in _connections)
				connection.Close();
			_connections.Clear();
		}
		void IDisposable.Dispose() => Close();
	}
}