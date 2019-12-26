using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Threading;
using System;
using System.Threading;

namespace Test.Support.Threading
{
	[TestClass]
	public class TaskManagerTest
	{
		private const int _taskCount = 0x100000;

		private readonly object _taskCounterLock;
		private volatile int _taskCounter;

		public TaskManagerTest() => _taskCounterLock = new object();

		private void Call(object state)
		{
			lock (_taskCounterLock)
				_taskCounter++;
		}
		private void ThrowException(object state)
		{
			try { throw new Exception(); }
			finally { _taskCounter++; }
		}
		[TestMethod]
		public void PostTest()
		{
			SendOrPostCallback callback = Call;
			_ = Assert.ThrowsException<ArgumentNullException>(() => TaskManager.Post(null, null));
			for (int taskIndex = 0x0; taskIndex != _taskCount; taskIndex++)
				TaskManager.Post(callback, null);
			while (_taskCounter != _taskCount)
				Thread.Sleep(0x1);
		}
		[TestMethod]
		public void PostYieldTest()
		{
			SendOrPostCallback callback = Call;
			_ = Assert.ThrowsException<ArgumentNullException>(() => TaskManager.PostYield(null, null));
			for (int taskIndex = 0x0; taskIndex != _taskCount; taskIndex++)
				TaskManager.PostYield(callback, null);
			while (_taskCounter != _taskCount)
				Thread.Sleep(0x1);
		}
		[TestMethod]
		public void ExceptionThrowingTest()
		{
			ManualSynchronizationContext synchronizationContext = new ManualSynchronizationContext();
			SynchronizationContext.SetSynchronizationContext(synchronizationContext);
			TaskManager.Post(ThrowException, null, synchronizationContext);
			do
				Thread.Sleep(0x1);
			while (_taskCounter == 0x0);
			_ = Assert.ThrowsException<AggregateException>(() => synchronizationContext.Execute());
		}
	}
}