using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Threading;
using System;

namespace Test.Support.Threading
{
	[TestClass]
	public class SynchronizationContextHelperTest
	{
		[TestMethod]
		public void PostExceptionTest()
		{
			ManualSynchronizationContext synchronizationContext = new ManualSynchronizationContext();
			Exception exception = new Exception();
			_ = Assert.ThrowsException<ArgumentNullException>(() => SynchronizationContextHelper.PostException(null, exception));
			_ = Assert.ThrowsException<ArgumentNullException>(() => synchronizationContext.PostException(null));
			synchronizationContext.PostException(exception);
		}
	}
}