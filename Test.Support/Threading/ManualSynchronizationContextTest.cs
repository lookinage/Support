using Microsoft.VisualStudio.TestTools.UnitTesting;
using Support.Threading;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Test.Support.Threading
{
	[TestClass]
	public class ManualSynchronizationContextTest
	{
		private sealed class ExoticException : Exception { }

		private const int _executeCount = 0x1000;
		private const int _exoticExceptionCount = 0x10;

		static private readonly ManualSynchronizationContext _synchronizationContext;
		static private readonly HashSet<int> _sendSet;
		static private readonly HashSet<int> _postSet;

		static ManualSynchronizationContextTest()
		{
			_synchronizationContext = new ManualSynchronizationContext();
			_sendSet = new HashSet<int>();
			_postSet = new HashSet<int>();
		}

		private readonly object _executionLock;
		private volatile int _executedCount;

		public ManualSynchronizationContextTest() => _executionLock = new object();

		private void ExecuteAsync(object state)
		{
			_ = Assert.ThrowsException<InvalidOperationException>(() => _synchronizationContext.Execute());
			if (_sendSet.Contains((int)state))
				_synchronizationContext.PostException(new AssertFailedException());
			_synchronizationContext.Send(ExecuteSend, state);
			if (!_sendSet.Contains((int)state))
				_synchronizationContext.PostException(new AssertFailedException());
			if (_postSet.Contains((int)state))
				_synchronizationContext.PostException(new AssertFailedException());
			_synchronizationContext.Post(ExecutePost, state);
			lock (_executionLock)
				_executedCount++;
		}
		private void ExecuteSend(object state)
		{
			int executionIndex = (int)state;
			_ = _sendSet.Add(executionIndex);
			if (executionIndex < _exoticExceptionCount)
				throw new ExoticException();
		}
		private void ExecutePost(object state) => _ = _postSet.Add((int)state);
		[TestMethod]
		public void SendTest() => _synchronizationContext.Send(null, null);
		[TestMethod]
		public void PostTest() => _synchronizationContext.Post(null, null);
		[TestMethod]
		public void ExecuteTest() => _synchronizationContext.Execute();
		[TestMethod()]
		public void CommonTest()
		{
			for (int executionIndex = 0x0; executionIndex != _executeCount; executionIndex++)
				TaskManager.Post(ExecuteAsync, executionIndex);
			int exoticExceptionCount = 0x0;
			while (_executedCount != _executeCount)
			{
				try { _synchronizationContext.Execute(); }
				catch (AggregateException aggregateException)
				{
					foreach (Exception exception in aggregateException.InnerExceptions)
					{
						if (!(exception is ExoticException))
							throw;
						exoticExceptionCount++;
					}
				}
				if (Thread.Yield())
					continue;
				Thread.Sleep(0x1);
			}
			try { _synchronizationContext.Execute(); }
			catch (AggregateException aggregateException)
			{
				foreach (Exception exception in aggregateException.InnerExceptions)
				{
					if (!(exception is ExoticException))
						throw;
					exoticExceptionCount++;
				}
			}
			Assert.IsTrue(exoticExceptionCount == _exoticExceptionCount);
			for (int executionIndex = 0x0; executionIndex != _executeCount; executionIndex++)
			{
				Assert.IsTrue(_sendSet.Contains(executionIndex));
				Assert.IsTrue(_postSet.Contains(executionIndex));
			}
		}
	}
}