namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Provides data for the <see cref="Listener{TIPEndPoint, TData}.Accepted"/> event.
	/// </summary>
	/// <typeparam name="TIPEndPoint">The type of IP endpoints.</typeparam>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	public struct ConnectionAcceptedEventArgument<TIPEndPoint, TData> where TIPEndPoint : struct, IIPEndPoint
	{
		/// <summary>
		/// The accepted connection.
		/// </summary>
		public readonly Connection<TIPEndPoint, TData> Connection;

		/// <summary>
		/// Initializes the <see cref="ConnectionAcceptedEventArgument{TIPEndPoint, TData}"/>.
		/// </summary>
		/// <param name="connection">The accepted connection.</param>
		public ConnectionAcceptedEventArgument(Connection<TIPEndPoint, TData> connection) => Connection = connection;
	}
}