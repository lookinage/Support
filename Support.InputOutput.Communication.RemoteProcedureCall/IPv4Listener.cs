using System;
using System.Net.Sockets;

namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Represents an implementation of the <see cref="Listener{TIPEndPoint, TData}"/> class that uses IPv4.
	/// </summary>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	public class IPv4Listener<TData> : Listener<IPv4EndPoint, TData>
	{
		/// <summary>
		/// Initializes the <see cref="Listener{TIPEndPoint, TData}"/>.
		/// </summary>
		/// <param name="localEndPoint">A local endpoint to associate with accepted connections.</param>
		/// <param name="localIPEndPoint">An IP endpoint to associate with a <see cref="Socket"/> to listen incoming connections.</param>
		/// <exception cref="ArgumentNullException"><paramref name="localEndPoint"/> is <see langword="null"/>.</exception>
		public IPv4Listener(LocalEndPoint<IPv4EndPoint, TData> localEndPoint, IPv4EndPoint localIPEndPoint) : base(localEndPoint, localIPEndPoint) { }
	}
}