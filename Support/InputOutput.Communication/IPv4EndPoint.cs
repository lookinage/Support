using Support.Coding.Serialization.System;
using System;
using System.Net;
using System.Net.Sockets;

namespace Support.InputOutput.Communication
{
	/// <summary>
	/// Represents a network endpoint as an IPv4 address and a port number.
	/// </summary>
	public struct IPv4EndPoint : IIPEndPoint
	{
		/// <summary>
		/// Initializes the <see cref="IPv4EndPoint"/>.
		/// </summary>
		/// <param name="endPoint">An <see cref="IPEndPoint"/> containing the IP address and the port.</param>
		/// <exception cref="ArgumentException"><paramref name="endPoint"/> is not IPv4 endpoint.</exception>
		static public IPv4EndPoint FromIPEndPoint(IPEndPoint endPoint)
		{
			if (endPoint.AddressFamily != AddressFamily.InterNetwork)
				throw new ArgumentException(string.Format("{0} is not IPv4 endpoint", nameof(endPoint)));
			byte[] addressBytes = endPoint.Address.GetAddressBytes();
			return new IPv4EndPoint(UInt32SerializerBuilder.Default.Deserialize(addressBytes, 0x0), (ushort)endPoint.Port);
		}

		/// <summary>
		/// Determines whether two specified IPv4 endpoints have the same value.
		/// </summary>
		/// <param name="a">The first IPv4 endpoint to compare.</param>
		/// <param name="b">The second IPv4 endpoint to compare.</param>
		/// <returns><see langword="true"/> whether the value of <paramref name="a"/> is the same as the value of <paramref name="b"/>; otherwise, <see langword="false"/>.</returns>
		static public bool operator ==(IPv4EndPoint a, IPv4EndPoint b) => a.Address == b.Address && a.Port == b.Port;
		/// <summary>
		/// Determines whether two specified IPv4 endpoints have different values.
		/// </summary>
		/// <param name="a">The first IPv4 endpoint to compare.</param>
		/// <param name="b">The second IPv4 endpoint to compare.</param>
		/// <returns><see langword="true"/> whether the value of <paramref name="a"/> is different from the value of <paramref name="b"/>; otherwise, <see langword="false"/>.</returns>
		static public bool operator !=(IPv4EndPoint a, IPv4EndPoint b) => a.Address != b.Address || a.Port != b.Port;

		/// <summary>
		/// The address of the IPv4 endpoint.
		/// </summary>
		public readonly uint Address;
		/// <summary>
		/// The port of the IPv4 endpoint.
		/// </summary>
		public readonly ushort Port;

		/// <summary>
		/// Initializes the <see cref="IPv4EndPoint"/>.
		/// </summary>
		/// <param name="address">The address of the IPv4 endpoint.</param>
		/// <param name="port">The port of the IPv4 endpoint.</param>
		public IPv4EndPoint(uint address, ushort port)
		{
			Address = address;
			Port = port;
		}
		/// <summary>
		/// Initializes the <see cref="IPv4EndPoint"/>.
		/// </summary>
		/// <param name="firstAddressPart">The first part of address of the IPv4 endpoint.</param>
		/// <param name="secondAddressPart">The second part of address of the IPv4 endpoint.</param>
		/// <param name="thirdAddressPart">The third part of address of the IPv4 endpoint.</param>
		/// <param name="fourthAddressPart">The fourth part of address of the IPv4 endpoint.</param>
		/// <param name="port">The port of the IPv4 endpoint.</param>
		public unsafe IPv4EndPoint(byte firstAddressPart, byte secondAddressPart, byte thirdAddressPart, byte fourthAddressPart, ushort port)
		{
			fixed (uint* p = &Address)
			{
				*(byte*)p = firstAddressPart;
				*((byte*)p + 0x1) = secondAddressPart;
				*((byte*)p + 0x2) = thirdAddressPart;
				*((byte*)p + 0x3) = fourthAddressPart;
			}
			Port = port;
		}

		/// <summary>
		/// Creates an <see cref="IPEndPoint"/> instance containing the IP address and the port.
		/// </summary>
		/// <returns>An <see cref="IPEndPoint"/> instance containing the IP address and the port.</returns>
		public IPEndPoint CreateIPEndPoint() => new IPEndPoint(new IPAddress(Address), Port);
		/// <summary>
		/// Determines whether this IPv4 end point and a specified object, which must also be a <see cref="IPv4EndPoint"/> object, have the same value.
		/// </summary>
		/// <param name="obj">The IPv4 end point to compare to this instance.</param>
		/// <returns><see langword="true"/> whether <paramref name="obj"/> is an <see cref="IPv4EndPoint"/> and its value is the same as this instance; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj) => base.Equals(obj);
		/// <summary>
		/// Returns the hash code for this IPv4 end point.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode() => base.GetHashCode();
		/// <summary>
		/// Returns a string that represents the current IPv4 endpoint.
		/// </summary>
		/// <returns>A string that represents the current IPv4 endpoint.</returns>
		public override string ToString() => CreateIPEndPoint().ToString();
	}
}