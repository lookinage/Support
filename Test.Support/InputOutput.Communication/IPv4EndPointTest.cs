using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Serialization.System;
using Support.InputOutput.Communication;
using System.Net;

namespace Test.Support.InputOutput.Communication
{
	[TestClass]
	public class IPv4EndPointTest
	{
		[TestMethod]
		public void FromIPEndPointTest()
		{
			IPv4EndPoint endPoint1 = new IPv4EndPoint(0x13FF61B9, 0x100);
			IPv4EndPoint endPoint2 = IPv4EndPoint.FromIPEndPoint(new IPEndPoint(IPAddress.Parse("185.97.255.19"), 0x100));
			Assert.IsTrue(endPoint2.Address == endPoint1.Address && endPoint1.Port == endPoint2.Port);
		}
		[TestMethod]
		public void ConstructorTest()
		{
			IPv4EndPoint endPoint1 = new IPv4EndPoint(0x13FF61B9, 0x100);
			IPv4EndPoint endPoint2 = new IPv4EndPoint(0xB9, 0x61, 0xFF, 0x13, 0x100);
			Assert.IsTrue(endPoint2.Address == endPoint1.Address && endPoint1.Port == endPoint2.Port);
		}
		[TestMethod]
		public void CreateIPEndPointTest()
		{
			IPv4EndPoint endPoint1 = new IPv4EndPoint(0xB9, 0x61, 0xFF, 0x13, 0x64);
			IPEndPoint endPoint2 = endPoint1.CreateIPEndPoint();
			byte[] addressBytes = endPoint2.Address.GetAddressBytes();
			uint address = UInt32SerializerBuilder.Default.Deserialize(addressBytes, 0x0);
			Assert.IsTrue(address == endPoint1.Address);
			Assert.IsTrue(endPoint2.Port == endPoint1.Port);
		}
	}
}