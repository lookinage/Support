using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Cryptography;
using Support.Coding.Serialization.System;
using Support.InputOutput.Communication;
using Support.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Test.Support.InputOutput.Communication
{
	[TestClass]
	public class LocalEndPointTest
	{
		private sealed class ServerManager : IDisposable
		{
			private readonly LocalEndPoint _point;

			internal ServerManager()
			{
				_point = new LocalEndPoint(_serverLocalEndPoint);
				_point.Received += Point_Received;
			}

			private void Point_Received(LocalEndPoint source)
			{
				_point.Post(_point.Get(Int32SerializerBuilder.Default), Int32SerializerBuilder.Default);
				_point.Send(_point.RemoteIPEndPoint);
			}

			public void Dispose() => _point.Close();
		}
		private sealed class ClientManager : IDisposable
		{
			private readonly LocalEndPoint _point;
			private readonly HashSet<int> _hashSet;
			private readonly SendOrPostCallback _request;

			internal ClientManager()
			{
				_point = new LocalEndPoint(_clientLocalEndPoint);
				_point.Received += Point_Received;
				_hashSet = new HashSet<int>();
				for (int i = 0x0; i < 0x64; i++)
					_ = _hashSet.Add(i);
				_request = Request;
				TaskManager.Post(_request, null);
			}

			internal bool Completed => _hashSet.Count == 0x0;

			private void Point_Received(LocalEndPoint source)
			{
				lock (_hashSet)
					_ = _hashSet.Remove(_point.Get(Int32SerializerBuilder.Default));
			}
			private void Request(object state)
			{
				lock (_hashSet)
				{
					if (_hashSet.Count != 0x0)
					{
						_point.Post(_hashSet.First(), Int32SerializerBuilder.Default);
						_point.Send(_clientRemoteEndPoint);
					}
				}
				TaskManager.PostYield(_request, null);
			}
			public void Dispose() => _point.Close();
		}

		static private readonly IPEndPoint _serverLocalEndPoint;
		static private readonly IPEndPoint _clientLocalEndPoint;
		static private readonly IPEndPoint _clientRemoteEndPoint;

		static LocalEndPointTest()
		{
			const int serverPort = 0x100;
			_serverLocalEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
			_clientLocalEndPoint = new IPEndPoint(IPAddress.Any, IPEndPoint.MinPort);
			_clientRemoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), serverPort);
		}

		[TestMethod]
		public void ConstructorTest()
		{
			_ = Assert.ThrowsException<ArgumentNullException>(() => new LocalEndPoint(null));
			new LocalEndPoint(_serverLocalEndPoint).Close();
		}
		[TestMethod]
		public void GetTest()
		{
			using (LocalEndPoint point = new LocalEndPoint(_serverLocalEndPoint))
			{
				_ = Assert.ThrowsException<InvalidOperationException>(() => point.Get(null, 0x0));
				_ = Assert.ThrowsException<InvalidOperationException>(() => point.Get(StringSerializerBuilder.Default));
				_ = Assert.ThrowsException<InvalidOperationException>(() => point.Get(Int32SerializerBuilder.Default));
				_ = Assert.ThrowsException<InvalidOperationException>(() => point.Get(null, 0x0, null, 0x0, 0x0, ValueCipher.Cipher));
				_ = Assert.ThrowsException<InvalidOperationException>(() => point.Get(StringSerializerBuilder.Default, null, 0x0, 0x0, ValueCipher.Cipher));
				_ = Assert.ThrowsException<InvalidOperationException>(() => point.Get(Int32SerializerBuilder.Default, null, 0x0, 0x0, ValueCipher.Cipher));
			}
		}
		[TestMethod]
		public void SkipTest()
		{
			using (LocalEndPoint point = new LocalEndPoint(_serverLocalEndPoint))
				_ = Assert.ThrowsException<InvalidOperationException>(() => point.Skip(0x0));
		}
		[TestMethod]
		public void PostTest()
		{
			const int length = 0x10;
			byte[] buffer = new byte[length];
			byte[] keyBuffer = new byte[length];
			using (LocalEndPoint point = new LocalEndPoint(_serverLocalEndPoint))
			{
				_ = Assert.ThrowsException<ArgumentNullException>(() => point.Post(0x0, null));
				point.Post(0x0, Int32SerializerBuilder.Default);
				_ = Assert.ThrowsException<ArgumentNullException>(() => point.Post(null, 0x0, length));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(buffer, -0x1, length));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(buffer, length + 0x1, length));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(buffer, 0x0, -0x1));
				_ = Assert.ThrowsException<ArgumentException>(() => point.Post(buffer, 0x0, length + 0x1));
				_ = Assert.ThrowsException<ArgumentNullException>(() => point.Post(0x0, null, keyBuffer, 0x0, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentNullException>(() => point.Post(0x0, Int32SerializerBuilder.Default, null, 0x0, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(0x0, Int32SerializerBuilder.Default, keyBuffer, -0x1, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(0x0, Int32SerializerBuilder.Default, keyBuffer, length + 0x1, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(0x0, Int32SerializerBuilder.Default, keyBuffer, 0x0, -0x1, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentException>(() => point.Post(0x0, Int32SerializerBuilder.Default, keyBuffer, 0x0, length + 1, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentNullException>(() => point.Post(0x0, Int32SerializerBuilder.Default, keyBuffer, 0x0, length, null));
				point.Post(0x0, Int32SerializerBuilder.Default, keyBuffer, 0x0, length, ValueCipher.Cipher);
				_ = Assert.ThrowsException<ArgumentNullException>(() => point.Post(null, 0x0, length, keyBuffer, 0x0, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(buffer, -0x1, length, keyBuffer, 0x0, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(buffer, length + 0x1, length, keyBuffer, 0x0, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(buffer, 0x0, -0x1, keyBuffer, 0x0, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentException>(() => point.Post(buffer, 0x0, length + 0x1, keyBuffer, 0x0, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentNullException>(() => point.Post(buffer, 0x0, length, null, 0x0, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(buffer, 0x0, length, keyBuffer, -0x1, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(buffer, 0x0, length, keyBuffer, length + 0x1, length, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => point.Post(buffer, 0x0, length, keyBuffer, 0x0, -0x1, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentException>(() => point.Post(buffer, 0x0, length, keyBuffer, 0x0, length + 0x1, ValueCipher.Cipher));
				_ = Assert.ThrowsException<ArgumentNullException>(() => point.Post(buffer, 0x0, length, keyBuffer, 0x0, length, null));
				point.Post(buffer, 0x0, length, keyBuffer, 0x0, length, ValueCipher.Cipher);
			}
		}
		[TestMethod]
		public void SendTest()
		{
			using (LocalEndPoint point = new LocalEndPoint(_clientLocalEndPoint))
			{
				_ = Assert.ThrowsException<ArgumentNullException>(() => point.Send(null));
				point.Send(_clientRemoteEndPoint);
			}
		}
		[TestMethod]
		public void CommonTest()
		{
			ManualSynchronizationContext synchronizationContext = new ManualSynchronizationContext();
			SynchronizationContext.SetSynchronizationContext(synchronizationContext);
			using (ServerManager serverManager = new ServerManager())
			{
				List<ClientManager> clientManagers = new List<ClientManager>();
				for (int i = 0x0; i < 0x10; i++)
					clientManagers.Add(new ClientManager());
				while (clientManagers.Any(x => !x.Completed))
				{
					synchronizationContext.Execute();
					Thread.Sleep(0x1);
				}
				foreach (ClientManager clientManager in clientManagers)
					clientManager.Dispose();
			}
			synchronizationContext.Execute();
		}
	}
}