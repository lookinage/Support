using System;
using System.Threading;

namespace Support.Threading
{
	/// <summary>
	/// Provides methods for a synchronization context manipulation.
	/// </summary>
	static public class SynchronizationContextHelper
	{
		static private readonly SendOrPostCallback _exceptionThrowing;

		static SynchronizationContextHelper() => _exceptionThrowing = ThrowException;

		static private void ThrowException(object state) => throw new Exception("A thrown exception has been received.", (Exception)state);
		/// <summary>
		/// Posts an exception to a specified <see cref="SynchronizationContext"/>.
		/// </summary>
		/// <param name="synchronizationContext">A <see cref="SynchronizationContext"/> where the exception is to be thrown.</param>
		/// <param name="exception">An <see cref="Exception"/> that is to be thrown.</param>
		/// <exception cref="ArgumentNullException"><paramref name="synchronizationContext"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
		static public void PostException(this SynchronizationContext synchronizationContext, Exception exception)
		{
			if (synchronizationContext == null)
				throw new ArgumentNullException(nameof(synchronizationContext));
			if (exception == null)
				throw new ArgumentNullException(nameof(exception));
			synchronizationContext.Post(_exceptionThrowing, exception);
		}
	}
}