using Support.Coding.Serialization;
using Support.Coding.Serialization.System;
using System;
using System.Threading;

namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Represents a local procedure that has no parameters.
	/// </summary>
	/// <typeparam name="TIPEndPoint">The type of IP endpoints.</typeparam>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	public class LocalProcedure<TIPEndPoint, TData> : LocalProcedureBase<TIPEndPoint, TData> where TIPEndPoint : struct, IIPEndPoint
	{
		private readonly LocalProcedureAction<TIPEndPoint, TData> _action;

		/// <summary>
		/// Initializes the <see cref="LocalProcedure{TIPEndPoint, TConnection}"/>.
		/// </summary>
		/// <param name="sync">If <see langword="true"/> and <see cref="SynchronizationContext.Current"/> existed when a local endpoint initializes the procedure is invoked in the <see cref="SynchronizationContext"/>; otherwise, the procedure is invoked asynchronously.</param>
		/// <param name="action">An action that executes when the procedure is called.</param>
		/// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
		public LocalProcedure(bool sync, LocalProcedureAction<TIPEndPoint, TData> action) : base(sync) => _action = action ?? throw new ArgumentNullException(nameof(action));

		private void Execute(object argument) => _action((Connection<TIPEndPoint, TData>)argument);
		internal override sealed bool Invoke(Connection<TIPEndPoint, TData> connection, SynchronizationContext synchronizationContext)
		{
			if (_sync && synchronizationContext != null)
			{
				synchronizationContext.Post(Execute, connection);
				return true;
			}
			_action(connection);
			return true;
		}
	}
	/// <summary>
	/// Represents a local procedure that has one parameter.
	/// </summary>
	/// <typeparam name="TIPEndPoint">The type of IP endpoints.</typeparam>
	/// <typeparam name="TData">The type of connection data.</typeparam>
	/// <typeparam name="T">The type of an argument that is passed for the procedure execution.</typeparam>
	public class LocalProcedure<TIPEndPoint, TData, T> : LocalProcedureBase<TIPEndPoint, TData> where TIPEndPoint : struct, IIPEndPoint
	{
		private protected struct Arguments
		{
			internal readonly Connection<TIPEndPoint, TData> _connection;
			internal readonly T _argument;

			internal Arguments(Connection<TIPEndPoint, TData> connection, T argument)
			{
				_connection = connection;
				_argument = argument;
			}
		}

		private protected readonly ParameterizedLocalProcedureAction<TIPEndPoint, TData, T> _action;
		private protected readonly ISerializer<T> _argumentSerializer;
		private protected readonly Func<T, bool> _argumentValidator;
		private protected readonly int _minArgumentLength;
		private protected readonly int _maxArgumentLength;

		/// <summary>
		/// Initializes the <see cref="LocalProcedure{TIPEndPoint, TConnection, T}"/>.
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
		public LocalProcedure(bool sync, ParameterizedLocalProcedureAction<TIPEndPoint, TData, T> action, ISerializer<T> argumentSerializer, Func<T, bool> argumentValidator, int minArgumentLength, int maxArgumentLength) : base(sync)
		{
			_action = action ?? throw new ArgumentNullException(nameof(action));
			_argumentSerializer = argumentSerializer ?? throw new ArgumentNullException(nameof(argumentSerializer));
			if (minArgumentLength < 0x0)
				throw new ArgumentOutOfRangeException(nameof(minArgumentLength));
			if (maxArgumentLength < minArgumentLength)
				throw new ArgumentOutOfRangeException(nameof(maxArgumentLength));
			_argumentValidator = argumentValidator;
			_minArgumentLength = minArgumentLength;
			_maxArgumentLength = maxArgumentLength;
		}

		private protected void Execute(object argument)
		{
			Arguments arguments = (Arguments)argument;
			_action(arguments._connection, arguments._argument);
		}
		internal override bool Invoke(Connection<TIPEndPoint, TData> connection, SynchronizationContext synchronizationContext)
		{
			if (connection._localProcedureArgumentLength == null)
			{
				connection._connection.Await(Int32SerializerBuilder.Default.Count);
				connection._localProcedureArgumentLength = -0x1;
				return false;
			}
			if (connection._localProcedureArgumentLength == -0x1)
			{
				connection._localProcedureArgumentLength = connection._connection.Get(Int32SerializerBuilder.Default);
				if (connection._localProcedureArgumentLength < _minArgumentLength || connection._localProcedureArgumentLength > _maxArgumentLength)
				{
					connection.InvokeLost(ConnectionLostError.LocalProcedureArgumentLength);
					return false;
				}
				connection._connection.Await(connection._localProcedureArgumentLength.Value);
				return false;
			}
			T argument;
			try { argument = connection._connection.Get(_argumentSerializer); }
			catch
			{
				connection.InvokeLost(ConnectionLostError.LocalProcedureArgumentDeserialization);
				return false;
			}
			connection._localProcedureArgumentLength = null;
			if (_argumentValidator != null && !_argumentValidator(argument))
			{
				connection.InvokeLost(ConnectionLostError.LocalProcedureArgumentValidation);
				return false;
			}
			if (_sync && synchronizationContext != null)
			{
				synchronizationContext.Post(Execute, new Arguments(connection, argument));
				return true;
			}
			_action(connection, argument);
			return true;
		}
	}
}