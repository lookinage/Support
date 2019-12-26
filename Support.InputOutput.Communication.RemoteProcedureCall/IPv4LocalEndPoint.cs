namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Represents an implementation of the <see cref="LocalEndPoint{TIPEndPoint, TData}"/> class that uses IPv4.
	/// </summary>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	public class IPv4LocalEndPoint<TData> : LocalEndPoint<IPv4EndPoint, TData>
	{
		/// <summary>
		/// Initializes the <see cref="LocalEndPoint{TIPEndPoint, TData}"/>.
		/// </summary>
		public IPv4LocalEndPoint() : base(IPv4EndPoint.FromIPEndPoint, IPv4EndPointSerializerBuilder.Default) { }
	}
}