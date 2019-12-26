using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.InputOutput.Communication;
using System;
using System.Net;

namespace Test.Support.InputOutput.Communication
{
	[TestClass]
	public class ConnectionListenerTest
	{
		static private readonly IPEndPoint _anyIPEndPoint;

		static ConnectionListenerTest() => _anyIPEndPoint = new IPEndPoint(IPAddress.Any, IPEndPoint.MinPort);

		[TestMethod]
		public void ConstructorTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => new ConnectionListener(null));
			new ConnectionListener(_anyIPEndPoint).Close();
		}
		[TestMethod]
		public void ListenTest()
		{
			ConnectionListener listener;
			using (listener = new ConnectionListener(_anyIPEndPoint))
				listener.Listen(0x0);
		}
	}
}