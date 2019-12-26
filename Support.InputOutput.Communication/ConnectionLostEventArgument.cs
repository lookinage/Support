using System.Net.Sockets;

namespace Support.InputOutput.Communication
{
	/// <summary>
	/// Provides data for the <see cref="Connection.Lost"/> event.
	/// </summary>
	public struct ConnectionLostEventArgument
	{
		/// <summary>
		/// The <see cref="SocketError"/> because of which the connection has been lost.
		/// </summary>
		public readonly SocketError Error;

		/// <summary>
		/// Initializes the <see cref="ConnectionAcceptedEventArgument"/>.
		/// </summary>
		/// <param name="error">The <see cref="SocketError"/> because of which the connection has been lost.</param>
		public ConnectionLostEventArgument(SocketError error) => Error = error;
	}
}