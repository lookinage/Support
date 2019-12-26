namespace Support.InputOutput.Communication
{
	/// <summary>
	/// Provides data for the <see cref="ConnectionListener.Accepted"/> event.
	/// </summary>
	public struct ConnectionAcceptedEventArgument
	{
		/// <summary>
		/// The accepted connection.
		/// </summary>
		public readonly Connection Connection;

		/// <summary>
		/// Initializes the <see cref="ConnectionAcceptedEventArgument"/>.
		/// </summary>
		/// <param name="connection">The accepted connection.</param>
		public ConnectionAcceptedEventArgument(Connection connection) => Connection = connection;
	}
}