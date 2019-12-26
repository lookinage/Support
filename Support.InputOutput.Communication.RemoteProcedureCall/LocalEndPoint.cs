using Support.Coding.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Represents a local endpoint that consists of local procedures which would be invoked by a remote endpoint.
	/// </summary>
	/// <typeparam name="TIPEndPoint">The type of IP endpoints.</typeparam>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	public abstract class LocalEndPoint<TIPEndPoint, TData> where TIPEndPoint : struct, IIPEndPoint
	{
		internal readonly Func<IPEndPoint, TIPEndPoint> _ipEndPointConverter;
		internal readonly IConstantLengthSerializer<TIPEndPoint> _ipEndPointSerializer;
		internal readonly Dictionary<int, LocalProcedureBase<TIPEndPoint, TData>> _localProcedures;
		internal readonly SynchronizationContext _synchronizationContext;

		/// <summary>
		/// Initializes the <see cref="LocalEndPoint{TIPEndPoint, TData}"/>.
		/// </summary>
		/// <param name="ipEndPointConverter">A method to convert <see cref="IPEndPoint"/> instances to <typeparamref name="TIPEndPoint"/> type.</param>
		/// <param name="ipEndPointSerializer">An <see cref="IConstantLengthSerializer{T}"/> of IP endpoints.</param>
		/// <exception cref="ArgumentNullException"><paramref name="ipEndPointConverter"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="ipEndPointSerializer"/> is <see langword="null"/>.</exception>
		public LocalEndPoint(Func<IPEndPoint, TIPEndPoint> ipEndPointConverter, IConstantLengthSerializer<TIPEndPoint> ipEndPointSerializer)
		{
			_ipEndPointConverter = ipEndPointConverter ?? throw new ArgumentNullException(nameof(ipEndPointConverter));
			_ipEndPointSerializer = ipEndPointSerializer ?? throw new ArgumentNullException(nameof(ipEndPointSerializer));
			_localProcedures = new Dictionary<int, LocalProcedureBase<TIPEndPoint, TData>>();
			_synchronizationContext = SynchronizationContext.Current;
		}

		/// <summary>
		/// Defines a local procedure that would be invoked by a remote endpoint.
		/// </summary>
		/// <param name="key">The key of the local procedure.</param>
		/// <param name="localProcedure">The local procedure.</param>
		/// <exception cref="ArgumentNullException"><paramref name="localProcedure"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">A local procedure with a specified key already exists.</exception>
		public void DefineLocalProcedure(int key, LocalProcedureBase<TIPEndPoint, TData> localProcedure)
		{
			if (localProcedure == null)
				throw new ArgumentNullException(nameof(localProcedure));
			if (_localProcedures.ContainsKey(key))
				throw new InvalidOperationException("A local procedure with a specified key already exists.");
			_localProcedures.Add(key, localProcedure);
		}
	}
}