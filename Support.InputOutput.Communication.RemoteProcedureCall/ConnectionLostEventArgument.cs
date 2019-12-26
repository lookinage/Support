using System.Net.Sockets;

namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Provides data for the <see cref="Connection{TIPEndPoint, TData}.Lost"/> event.
	/// </summary>
	public struct ConnectionLostEventArgument
	{
		/// <summary>
		/// The <see cref="ConnectionLostError"/> because of which the connection has been closed.
		/// </summary>
		public readonly ConnectionLostError Error;
		/// <summary>
		/// The <see cref="System.Net.Sockets.SocketError"/> because of which the connection has been lost.
		/// </summary>
		public readonly SocketError SocketError;

		/// <summary>
		/// Initializes the <see cref="ConnectionAcceptedEventArgument"/>.
		/// </summary>
		/// <param name="error">The <see cref="ConnectionLostError"/> because of which the connection has been closed.</param>
		/// <param name="socketError">The <see cref="System.Net.Sockets.SocketError"/> because of which the connection has been lost.</param>
		public ConnectionLostEventArgument(ConnectionLostError error, SocketError socketError)
		{
			Error = error;
			SocketError = socketError;
		}
	}
}