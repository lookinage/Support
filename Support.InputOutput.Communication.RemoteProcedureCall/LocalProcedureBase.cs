using System.Threading;

namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Represents a local procedure that can be invoked remotely.
	/// </summary>
	/// <typeparam name="TIPEndPoint">The type of IP endpoints.</typeparam>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	public abstract class LocalProcedureBase<TIPEndPoint, TData> where TIPEndPoint : struct, IIPEndPoint
	{
		private protected readonly bool _sync;

		internal LocalProcedureBase(bool sync) => _sync = sync;

		internal abstract bool Invoke(Connection<TIPEndPoint, TData> connection, SynchronizationContext synchronizationContext);
	}
}