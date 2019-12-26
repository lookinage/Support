namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Defines connection lost error codes for the <see cref="Connection{TIPEndPoint, TData}"/>.
	/// </summary>
	public enum ConnectionLostError
	{
		/// <summary>
		/// Specifies the connection is lost because of it was manually closed.
		/// </summary>
		None,
		/// <summary>
		/// Specifies the connection is lost because of a socket error.
		/// </summary>
		SocketError,
		/// <summary>
		/// Specifies the connection is lost because of there is no local procedure with the received procedure key.
		/// </summary>
		LocalProcedureNotFound,
		/// <summary>
		/// Specifies the connection is lost because of the length of the received argument to call a local procedure is out of range.
		/// </summary>
		LocalProcedureArgumentLength,
		/// <summary>
		/// Specifies the connection is lost because of the received argument to call a local procedure failed to be deserialized.
		/// </summary>
		LocalProcedureArgumentDeserialization,
		/// <summary>
		/// Specifies the connection is lost because of the received argument to call a local procedure is invalid.
		/// </summary>
		LocalProcedureArgumentValidation,
	}
}