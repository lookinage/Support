namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// References a method to be executed when the local procedure is invoked by a remote endpoint.
	/// </summary>
	/// <typeparam name="TIPEndPoint">The type of IP endpoints.</typeparam>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	/// <typeparam name="T">The type of an argument that is passed for the procedure execution.</typeparam>
	/// <param name="connection">A connection that provides remote endpoint that have called the local procedure.</param>
	/// <param name="argument">An argument that has been passed for the local procedure execution.</param>
	public delegate void ParameterizedLocalProcedureAction<TIPEndPoint, TData, in T>(Connection<TIPEndPoint, TData> connection, T argument) where TIPEndPoint : struct, IIPEndPoint;
}