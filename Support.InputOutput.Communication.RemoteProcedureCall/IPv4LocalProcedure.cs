using Support.Coding.Serialization;
using System;
using System.Threading;

namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Represents a local procedure that has no parameters.
	/// </summary>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	public class IPv4LocalProcedure<TData> : LocalProcedure<IPv4EndPoint, TData>
	{
		/// <summary>
		/// Initializes the <see cref="IPv4LocalProcedure{TData}"/>.
		/// </summary>
		/// <param name="sync">If <see langword="true"/> and <see cref="SynchronizationContext.Current"/> existed when a local endpoint initializes the procedure is invoked in the <see cref="SynchronizationContext"/>; otherwise, the procedure is invoked asynchronously.</param>
		/// <param name="action">An action that executes when the procedure is called.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
		public IPv4LocalProcedure(bool sync, LocalProcedureAction<IPv4EndPoint, TData> action) : base(sync, action) { }
	}
	/// <summary>
	/// Represents a local procedure that has one parameter.
	/// </summary>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	/// <typeparam name="T">The type of an argument that is passed for the procedure execution.</typeparam>
	public class IPv4LocalProcedure<TData, T> : LocalProcedure<IPv4EndPoint, TData, T>
	{
		/// <summary>
		/// Initializes the <see cref="IPv4LocalProcedure{TData, T}"/>.
		/// </summary>
		/// <param name="sync">If <see langword="true"/> and <see cref="SynchronizationContext.Current"/> existed when a local endpoint initializes the procedure is invoked in the <see cref="SynchronizationContext"/>; otherwise, the procedure is invoked asynchronously.</param>
		/// <param name="action">An action that executes when the procedure is called.</param>
		/// <param name="argumentSerializer">An <see cref="ISerializer{T}"/> to deserialize an argument for the local procedure execution.</param>
		/// <param name="argumentValidator">A method to define whether a received argument is valid. If <see langword="true"/> a connection remote endpoint sends an invalid argument the connection would be disconnected.</param>
		/// <param name="minArgumentLength">The minimum length of an argument to receive. If <see langword="true"/> a connection remote endpoint sends an argument which length is less than a specified value the connection would be disconnected.</param>
		/// <param name="maxArgumentLength">The maximum length of an argument to receive. If <see langword="true"/> a connection remote endpoint sends an argument which length is greater than a specified value the connection would be disconnected.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="argumentSerializer"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="minArgumentLength"/> is less than 0.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="maxArgumentLength"/> is less than <paramref name="minArgumentLength"/>.</exception>
		public IPv4LocalProcedure(bool sync, ParameterizedLocalProcedureAction<IPv4EndPoint, TData, T> action, ISerializer<T> argumentSerializer, Func<T, bool> argumentValidator, int minArgumentLength, int maxArgumentLength) : base(sync, action, argumentSerializer, argumentValidator, minArgumentLength, maxArgumentLength) { }
		/// <summary>
		/// Initializes the <see cref="IPv4LocalProcedure{TData, T}"/>.
		/// </summary>
		/// <param name="sync">If <see langword="true"/> and <see cref="SynchronizationContext.Current"/> existed when a local endpoint initializes the procedure is invoked in the <see cref="SynchronizationContext"/>; otherwise, the procedure is invoked asynchronously.</param>
		/// <param name="action">An action that executes when the procedure is called.</param>
		/// <param name="argumentSerializer">An <see cref="ISerializer{T}"/> to deserialize an argument for the local procedure execution.</param>
		/// <param name="argumentValidator">A method to define whether a received argument is valid. If <see langword="true"/> a connection remote endpoint sends an invalid argument the connection would be disconnected.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="argumentSerializer"/> is <see langword="null"/>.</exception>
		public IPv4LocalProcedure(bool sync, ParameterizedLocalProcedureAction<IPv4EndPoint, TData, T> action, ISerializer<T> argumentSerializer, Func<T, bool> argumentValidator) : this(sync, action, argumentSerializer, argumentValidator, 0, int.MaxValue) { }
		/// <summary>
		/// Initializes the <see cref="IPv4LocalProcedure{TData, T}"/>.
		/// </summary>
		/// <param name="sync">If <see langword="true"/> and <see cref="SynchronizationContext.Current"/> existed when a local endpoint initializes the procedure is invoked in the <see cref="SynchronizationContext"/>; otherwise, the procedure is invoked asynchronously.</param>
		/// <param name="action">An action that executes when the procedure is called.</param>
		/// <param name="argumentSerializer">An <see cref="ISerializer{T}"/> to deserialize an argument for the local procedure execution.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"> <paramref name="argumentSerializer"/> is <see langword="null"/>.</exception>
		public IPv4LocalProcedure(bool sync, ParameterizedLocalProcedureAction<IPv4EndPoint, TData, T> action, ISerializer<T> argumentSerializer) : this(sync, action, argumentSerializer, null, 0, int.MaxValue) { }
		/// <summary>
		/// Initializes the <see cref="IPv4LocalProcedure{TData, T}"/>.
		/// </summary>
		/// <param name="sync">If <see langword="true"/> and <see cref="SynchronizationContext.Current"/> existed when a local endpoint initializes the procedure is invoked in the <see cref="SynchronizationContext"/>; otherwise, the procedure is invoked asynchronously.</param>
		/// <param name="action">An action that executes when the procedure is called.</param>
		/// <param name="argumentSerializer">An <see cref="IConstantLengthSerializer{T}"/> to deserialize an argument for the local procedure execution.</param>
		/// <param name="argumentValidator">A method to define whether a received argument is valid. If <see langword="true"/> a connection remote endpoint sends an invalid argument the connection would be disconnected.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="argumentSerializer"/> is <see langword="null"/>.</exception>
		public IPv4LocalProcedure(bool sync, ParameterizedLocalProcedureAction<IPv4EndPoint, TData, T> action, IConstantLengthSerializer<T> argumentSerializer, Func<T, bool> argumentValidator) : this(sync, action, argumentSerializer, argumentValidator, argumentSerializer.Count, argumentSerializer.Count) { }
		/// <summary>
		/// Initializes the <see cref="IPv4LocalProcedure{TData, T}"/>.
		/// </summary>
		/// <param name="sync">If <see langword="true"/> and <see cref="SynchronizationContext.Current"/> existed when a local endpoint initializes the procedure is invoked in the <see cref="SynchronizationContext"/>; otherwise, the procedure is invoked asynchronously.</param>
		/// <param name="action">An action that executes when the procedure is called.</param>
		/// <param name="argumentSerializer">An <see cref="IConstantLengthSerializer{T}"/> to deserialize an argument for the local procedure execution.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="argumentSerializer"/> is <see langword="null"/>.</exception>
		public IPv4LocalProcedure(bool sync, ParameterizedLocalProcedureAction<IPv4EndPoint, TData, T> action, IConstantLengthSerializer<T> argumentSerializer) : this(sync, action, argumentSerializer, null, argumentSerializer.Count, argumentSerializer.Count) { }
	}
}