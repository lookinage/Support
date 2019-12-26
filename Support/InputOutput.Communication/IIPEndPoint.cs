using System.Net;

namespace Support.InputOutput.Communication
{
	/// <summary>
	/// Represents a network endpoint as an IP address and a port number.
	/// </summary>
	public interface IIPEndPoint
	{
		/// <summary>
		/// Creates an <see cref="IPEndPoint"/> instance containing the IP address and the port.
		/// </summary>
		/// <returns>An <see cref="IPEndPoint"/> instance containing the IP address and the port.</returns>
		IPEndPoint CreateIPEndPoint();
	}
}