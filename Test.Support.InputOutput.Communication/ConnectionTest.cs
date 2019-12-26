using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Cryptography;
using Support.Coding.Serialization.System;
using Support.InputOutput.Communication;
using System;
using System.Net;
using System.Net.Sockets;

namespace Test.Support.InputOutput.Communication
{
	[TestClass]
	public class ConnectionTest
	{
		static private readonly IPEndPoint _serverLocalEndPoint;
		static private readonly IPEndPoint _clientLocalEndPoint;
		static private readonly IPEndPoint _clientRemoteEndPoint;

		static ConnectionTest()
		{
			const int serverPort = 0x100;
			_serverLocalEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
			_clientLocalEndPoint = new IPEndPoint(IPAddress.Any, IPEndPoint.MinPort);
			_clientRemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort);
		}

		[TestMethod]
		public void ConstructorTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => _ = new Connection(null, _clientRemoteEndPoint));
			_ = Assert.ThrowsException<ArgumentNullException>(() => _ = new Connection(_clientLocalEndPoint, null));
			try { new Connection(_clientLocalEndPoint, _clientRemoteEndPoint).Close(); }
			catch (SocketException) { }
		}
		[TestMethod]
		public void AwaitTest()
		{
			using (ConnectionListener listener = new ConnectionListener(_serverLocalEndPoint))
			{
				listener.Listen(0x10);
				using (Connection connection = new Connection(_clientLocalEndPoint, _clientRemoteEndPoint))
				{
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => connection.Await(-0x1));
					connection.Await(0x10);
					_ = Assert.ThrowsException<InvalidOperationException>(() => connection.Await(0x10));
				}
			}
		}
		[TestMethod]
		public void DecryptTest()
		{
			using (ConnectionListener listener = new ConnectionListener(_serverLocalEndPoint))
			{
				listener.Listen(0x10);
				using (Connection connection = new Connection(_clientLocalEndPoint, _clientRemoteEndPoint))
					_ = Assert.ThrowsException<InvalidOperationException>(() => connection.Decrypt(null, 0x0, 0x0, ValueCipher.Cipher));
			}
		}
		[TestMethod]
		public void GetTest()
		{
			using (ConnectionListener listener = new ConnectionListener(_serverLocalEndPoint))
			{
				listener.Listen(0x10);
				using (Connection connection = new Connection(_clientLocalEndPoint, _clientRemoteEndPoint))
				{
					_ = Assert.ThrowsException<InvalidOperationException>(() => connection.Get(null, 0x0));
					_ = Assert.ThrowsException<InvalidOperationException>(() => connection.Get(Int32SerializerBuilder.Default));
				}
			}
		}
		[TestMethod]
		public void PostTest()
		{
			using (ConnectionListener listener = new ConnectionListener(_serverLocalEndPoint))
			{
				listener.Listen(0x10);
				const int length = 0x10;
				byte[] buffer = new byte[length];
				using (Connection connection = new Connection(_clientLocalEndPoint, _clientRemoteEndPoint))
				{
					_ = Assert.ThrowsException<ArgumentNullException>(() => connection.Post(0x0, null));
					connection.Post(0x0, Int32SerializerBuilder.Default);
					_ = Assert.ThrowsException<ArgumentNullException>(() => connection.Post(null, 0x0, length));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => connection.Post(buffer, -0x1, length));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => connection.Post(buffer, length + 0x1, length));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => connection.Post(buffer, 0x0, -0x1));
					_ = Assert.ThrowsException<ArgumentException>(() => connection.Post(buffer, 0x0, length + 0x1));
				}
			}
		}
		[TestMethod]
		public void EncryptTest()
		{
			using (ConnectionListener listener = new ConnectionListener(_serverLocalEndPoint))
			{
				listener.Listen(0x10);
				const int length = 0x10;
				byte[] buffer = new byte[length];
				byte[] keyBuffer = new byte[length];
				using (Connection connection = new Connection(_clientLocalEndPoint, _clientRemoteEndPoint))
				{
					_ = Assert.ThrowsException<ArgumentNullException>(() => connection.Encrypt(null, 0x0, length, ValueCipher.Cipher));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => connection.Encrypt(keyBuffer, -0x1, length, ValueCipher.Cipher));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => connection.Encrypt(keyBuffer, length + 0x1, length, ValueCipher.Cipher));
					_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => connection.Encrypt(keyBuffer, 0x0, -0x1, ValueCipher.Cipher));
					_ = Assert.ThrowsException<ArgumentException>(() => connection.Encrypt(keyBuffer, 0x0, length + 0x1, ValueCipher.Cipher));
					_ = Assert.ThrowsException<ArgumentNullException>(() => connection.Encrypt(keyBuffer, 0x0, length, null));
					connection.Encrypt(keyBuffer, 0x0, length, ValueCipher.Cipher);
				}
			}
		}
		[TestMethod]
		public void SendTest()
		{
			using (ConnectionListener listener = new ConnectionListener(_serverLocalEndPoint))
			{
				listener.Listen(0x10);
				using (Connection connection = new Connection(_clientLocalEndPoint, _clientRemoteEndPoint))
					connection.Send();
			}
		}
	}
}