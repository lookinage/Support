using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Coding.Serialization.System;
using Support.InputOutput.Communication;
using Support.InputOutput.Communication.RemoteProcedureCall;

namespace Test.Support.InputOutput.Communication.RemoteProcedureCall
{
	[TestClass]
	public class CommonTest
	{
		static private class ServerProcedureKeys
		{
			internal const int _test = 23;
		}
		static private class ClientProcedureKeys
		{
			internal const int _test = 56;
		}
		static private class ServerRemoteProcedures
		{
			static internal readonly RemoteProcedure<string> _test;

			static ServerRemoteProcedures() => _test = new RemoteProcedure<string>(ServerProcedureKeys._test, StringSerializerBuilder.UTF8);
		}
		static private class ClientRemoteProcedures
		{
			static internal readonly RemoteProcedure _test;

			static ClientRemoteProcedures() => _test = new RemoteProcedure(ClientProcedureKeys._test);
		}

		private readonly IPv4EndPoint _serverLocalEndPoint;
		private readonly IPv4EndPoint _clientRemoteEndPoint;

		public CommonTest()
		{
			const int serverPort = 0x100;
			_serverLocalEndPoint = new IPv4EndPoint(0x0, serverPort);
			_clientRemoteEndPoint = new IPv4EndPoint(0x7F, 0x0, 0x0, 0x1, serverPort);
		}

		private void Listener_Accepted(Listener<IPv4EndPoint, object> source, ConnectionAcceptedEventArgument<IPv4EndPoint, object> argument)
		{
			Connection<IPv4EndPoint, object> connection = argument.Connection;
			connection.Receive();
		}
		private void ServerTest(Connection<IPv4EndPoint, object> connection, string s) => connection.Call(ClientRemoteProcedures._test);
		private void ClientTest(Connection<IPv4EndPoint, object> connection)
		{
		}
		[TestMethod]
		public void ConnectionTest()
		{
			IPv4LocalEndPoint<object> serverLocalEndPoint = new IPv4LocalEndPoint<object>();
			serverLocalEndPoint.DefineLocalProcedure(ServerProcedureKeys._test, new IPv4LocalProcedure<object, string>(true, ServerTest, StringSerializerBuilder.UTF8));
			IPv4Listener<object> listener = new IPv4Listener<object>(serverLocalEndPoint, _serverLocalEndPoint);
			listener.Accepted += Listener_Accepted;
			listener.Listen(0x10);
			IPv4LocalEndPoint<object> clientLocalEndPoint = new IPv4LocalEndPoint<object>();
			clientLocalEndPoint.DefineLocalProcedure(ClientProcedureKeys._test, new IPv4LocalProcedure<object>(true, ClientTest));
			Connection<IPv4EndPoint, object> connection = new Connection<IPv4EndPoint, object>(clientLocalEndPoint, default, _clientRemoteEndPoint);
			connection.Receive();
			connection.Call(ServerRemoteProcedures._test, "poshel nahui!");
		}
	}
}