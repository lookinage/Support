using System;
using System.Collections.Generic;
using System.Threading;

namespace Support.Threading
{
	/// <summary>
	/// Represents a custom synchronization context that needs for sometimes <see cref="Execute"/> call.
	/// </summary>
	public class ManualSynchronizationContext : SynchronizationContext
	{
		private struct Task
		{
			private readonly SendOrPostCallback _d;
			private readonly object _state;

			internal Task(SendOrPostCallback d, object state)
			{
				_d = d;
				_state = state;
			}

			internal void Execute() => _d(_state);
		}

		private readonly Thread _thread;
		private readonly List<Task> _tasks;
		private readonly List<Exception> _exceptions;
		private volatile int _executedCount;

		/// <summary>
		/// Initializes the <see cref="ManualSynchronizationContext"/>.
		/// </summary>
		public ManualSynchronizationContext()
		{
			_thread = Thread.CurrentThread;
			_tasks = new List<Task>();
			_exceptions = new List<Exception>();
		}

		/// <summary>
		/// Dispatches a synchronous message to a synchronization context.
		/// </summary>
		/// <param name="d">The <see cref="SendOrPostCallback"/> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		public override void Send(SendOrPostCallback d, object state)
		{
			if (d == null)
				return;
			int executionCount;
			lock (_tasks)
			{
				_tasks.Add(new Task(d, state));
				executionCount = _executedCount;
			}
			while (executionCount == _executedCount)
			{
				if (Thread.Yield())
					continue;
				Thread.Sleep(0x1);
			}
		}
		/// <summary>
		/// Dispatches an asynchronous message to a synchronization context.
		/// </summary>
		/// <param name="d">The <see cref="SendOrPostCallback"/> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		public override void Post(SendOrPostCallback d, object state)
		{
			if (d == null)
				return;
			lock (_tasks)
				_tasks.Add(new Task(d, state));
		}
		/// <summary>
		/// Executes actions which wait execution in the synchronization context.
		/// </summary>
		/// <exception cref="InvalidOperationException">The current thread does not equal to the thread of the synchronization context.</exception>
		/// <exception cref="AggregateException">Exceptions were thrown from the executed actions.</exception>
		public void Execute()
		{
			if (Thread.CurrentThread != _thread)
				throw new InvalidOperationException("The current thread does not equal to the thread of the synchronization context.");
			lock (_tasks)
			{
				foreach (Task task in _tasks)
					try { task.Execute(); }
					catch (Exception exception) { _exceptions.Add(exception); }
				_tasks.Clear();
				_executedCount++;
			}
			if (_exceptions.Count == 0x0)
				return;
			try { throw new AggregateException("Exceptions were thrown from the executed actions.", _exceptions); }
			finally { _exceptions.Clear(); }
		}
	}
}