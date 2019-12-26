using System;

namespace Support.ComponentModel
{
	/// <summary>
	/// Provides data for events occurring when a value changes.
	/// </summary>
	public class ValueChangedEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValueChangedEventArgs{T}"/> class.
		/// </summary>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		public ValueChangedEventArgs(T oldValue, T newValue)
		{
			NewValue = newValue;
			OldValue = oldValue;
		}

		/// <summary>
		/// Gets the old value.
		/// </summary>
		public T OldValue { get; }
		/// <summary>
		/// Gets the new value.
		/// </summary>
		public T NewValue { get; }
	}
}
