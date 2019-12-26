using Support.Coding.Serialization;
using System;

namespace Support.InputOutput.Communication.RemoteProcedureCall
{
	/// <summary>
	/// Represents a remote procedure that has no parameters.
	/// </summary>
	public class RemoteProcedure
	{
		/// <summary>
		/// The key to call the remote procedure.
		/// </summary>
		public readonly int Key;

		/// <summary>
		/// Initializes the <see cref="RemoteProcedure"/>.
		/// </summary>
		/// <param name="key">The key of the remote procedure.</param>
		public RemoteProcedure(int key) => Key = key;
	}
	/// <summary>
	/// Represents a remote procedure that has one parameter.
	/// </summary>
	/// <typeparam name="T">The type of an argument that is passed for the procedure execution.</typeparam>
	public class RemoteProcedure<T>
	{
		/// <summary>
		/// The key to call the remote procedure.
		/// </summary>
		public readonly int Key;
		/// <summary>
		/// The <see cref="ISerializer{T}"/> to serialize an argument for the remote procedure call.
		/// </summary>
		public readonly ISerializer<T> ArgumentSerializer;

		/// <summary>
		/// Initializes the <see cref="RemoteProcedure{T}"/>.
		/// </summary>
		/// <param name="key">The key of the remote procedure.</param>
		/// <param name="argumentSerializer">An <see cref="ISerializer{T}"/> to serialize an argument for the remote procedure call.</param>
		/// <exception cref="ArgumentNullException"><paramref name="argumentSerializer"/> is <see langword="null"/>.</exception>
		public RemoteProcedure(int key, ISerializer<T> argumentSerializer)
		{
			Key = key;
			ArgumentSerializer = argumentSerializer ?? throw new ArgumentNullException(nameof(argumentSerializer));
		}
	}
}