namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// References a method to be executed when the local procedure is invoked by a remote endpoint.
	/// </summary>
	/// <typeparam name="TIPEndPoint">The type of IP endpoints.</typeparam>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	/// <param name="connection">A connection that provides remote endpoint that have called the local procedure.</param>
	public delegate void LocalProcedureAction<TIPEndPoint, TData>(Connection<TIPEndPoint, TData> connection) where TIPEndPoint : struct, IIPEndPoint;
}