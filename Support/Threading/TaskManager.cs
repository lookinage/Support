using System;
using System.Collections.Generic;
using System.Threading;

namespace Support.Threading
{
	/// <summary>
	/// Provides methods for asynchronous task execution.
	/// </summary>
	static public class TaskManager
	{
		private struct Task
		{
			private readonly SendOrPostCallback _d;
			private readonly object _state;
			internal readonly SynchronizationContext _synchronizationContext;

			internal Task(SendOrPostCallback d, object state, SynchronizationContext synchronizationContext)
			{
				_d = d;
				_state = state;
				_synchronizationContext = synchronizationContext;
			}

			internal void Execute()
			{
				try { _d(_state); }
				catch (ThreadAbortException) { }
				catch (Exception exception) { _synchronizationContext?.PostException(exception); }
			}
		}

		static private readonly Queue<Task> _tasks;
		static private readonly Queue<Task> _delayedTasks;
		[ThreadStatic]
		static private Task _task;

		static TaskManager()
		{
			_tasks = new Queue<Task>();
			_delayedTasks = new Queue<Task>();
			int threadCount = Environment.ProcessorCount;
			ThreadStart execution = Execute;
			for (int threadIndex = 0x0; threadIndex != threadCount; threadIndex++)
				new Thread(execution).Start();
		}

		static private void Execute()
		{
		Sleep:
			Thread.Sleep(0x1);
		Work:
			lock (_tasks)
			{
				if (_tasks.Count == 0x0)
				{
					lock (_delayedTasks)
					{
						foreach (Task delayedTask in _delayedTasks)
							_tasks.Enqueue(delayedTask);
						_delayedTasks.Clear();
					}
					goto Sleep;
				}
				_task = _tasks.Dequeue();
			}
			_task.Execute();
			goto Work;
		}
		/// <summary>
		/// Posts a task for asynchronous execution.
		/// </summary>
		/// <param name="d">The <see cref="SendOrPostCallback"/> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		/// <param name="synchronizationContext">A <see cref="SynchronizationContext"/> to receive an exception that can be thrown.</param>
		/// <exception cref="ArgumentNullException"><paramref name="d"/> is <see langword="null"/>.</exception>
		static public void Post(SendOrPostCallback d, object state, SynchronizationContext synchronizationContext)
		{
			if (d == null)
				throw new ArgumentNullException(nameof(d));
			lock (_tasks)
				_tasks.Enqueue(new Task(d, state, synchronizationContext));
		}
		/// <summary>
		/// Posts a task for asynchronous execution.
		/// </summary>
		/// <param name="d">The <see cref="SendOrPostCallback"/> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		static public void Post(SendOrPostCallback d, object state) => Post(d, state, _task._synchronizationContext);
		/// <summary>
		/// Posts a task for asynchronous execution. The task executes only when there are no other tasks to execute.
		/// </summary>
		/// <param name="d">The <see cref="SendOrPostCallback"/> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		/// <param name="synchronizationContext">A <see cref="SynchronizationContext"/> to receive an exception that can be thrown.</param>
		static public void PostYield(SendOrPostCallback d, object state, SynchronizationContext synchronizationContext)
		{
			if (d == null)
				throw new ArgumentNullException(nameof(d));
			lock (_delayedTasks)
				_delayedTasks.Enqueue(new Task(d, state, synchronizationContext));
		}
		/// <summary>
		/// Posts a task for asynchronous execution. The task executes only when there are no other tasks to execute.
		/// </summary>
		/// <param name="d">The <see cref="SendOrPostCallback"/> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		static public void PostYield(SendOrPostCallback d, object state) => PostYield(d, state, _task._synchronizationContext);
	}
}