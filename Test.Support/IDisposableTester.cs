using System;

namespace Test.Support.Sets
{
	/// <summary>
	/// Represents the tester of <see cref="IDisposable"/> instances.
	/// </summary>
	static public class IDisposableTester
	{
		/// <summary>
		/// Tests an <see cref="IDisposable"/>.
		/// </summary>
		/// <param name="instance">The <see cref="IDisposable"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
		static public void TestIDisposable(this IDisposable instance)
		{
			if (instance == null)
				throw new ArgumentNullException(nameof(instance));
			instance.Dispose();
		}
	}
}