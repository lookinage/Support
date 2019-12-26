using System;
using System.Collections.Generic;

namespace Support.ComponentModel
{
	/// <summary>
	/// Represents a <see cref="NotifyingProperty{T}"/> modification that can be undone or redone. Such property does not remember its old value automatically.
	/// </summary>
	public class ReversibleProperty<T> : NotifyingProperty<T>, IReversible
	{
		private readonly Stack<T> _previousValues;
		private readonly Stack<T> _subsequentValues;
		private bool _isStateUpdate;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReversibleProperty{T}"/> class.
		/// </summary>
		public ReversibleProperty() : this(null, default) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="ReversibleProperty{T}"/> class.
		/// </summary>
		/// <param name="valuePredicate">The method to define values the property can set.</param>
		public ReversibleProperty(Predicate<T> valuePredicate) : this(valuePredicate, default) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="ReversibleProperty{T}"/> class.
		/// </summary>
		/// <param name="value">The start value of the property.</param>
		public ReversibleProperty(T value) : this(null, value) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="ReversibleProperty{T}"/> class.
		/// </summary>
		/// <param name="valuePredicate">The method to define values the property can set.</param>
		/// <param name="value">The start value of the property.</param>
		public ReversibleProperty(Predicate<T> valuePredicate, T value) : base(valuePredicate, value)
		{
			_previousValues = new Stack<T>();
			_subsequentValues = new Stack<T>();
			_isStateUpdate = false;
			CanUndoProperty = new NotifyingProperty<bool>();
			CanRedoProperty = new NotifyingProperty<bool>();
			ValueChanged += ReversibleProperty_ValueChanged;
		}

		/// <summary>
		/// Provides access to the value indicating whether the collection can be undone.
		/// </summary>
		public NotifyingProperty<bool> CanUndoProperty { get; }
		/// <summary>
		/// Provides access to the value indicating whether the collection can be redone.
		/// </summary>
		public NotifyingProperty<bool> CanRedoProperty { get; }

		/// <summary>
		/// Occurs when a change was remembered.
		/// </summary>
		public event EventHandler ChangeRemembered;

		/// <summary>
		/// Undoes the value of the property.
		/// </summary>
		/// <exception cref="InvalidOperationException">The property cannot be undone.</exception>
		public void Undo()
		{
			if (_previousValues.Count == 0)
				throw new InvalidOperationException("The object cannot be undone.");
			_subsequentValues.Push(Value);
			_isStateUpdate = true;
			Value = _previousValues.Pop();
			_isStateUpdate = false;
			CanRedoProperty.Value = true;
			if (_previousValues.Count == 0)
				CanUndoProperty.Value = false;
		}
		/// <summary>
		/// Redoes the value of the property.
		/// </summary>
		/// <exception cref="InvalidOperationException">The property cannot be redone.</exception>
		public void Redo()
		{
			if (_subsequentValues.Count == 0)
				throw new InvalidOperationException("The object cannot be redone.");
			_previousValues.Push(Value);
			_isStateUpdate = true;
			Value = _subsequentValues.Pop();
			_isStateUpdate = false;
			CanUndoProperty.Value = true;
			if (_subsequentValues.Count == 0)
				CanRedoProperty.Value = false;
		}
		/// <summary>
		/// Removes the remembered changes.
		/// </summary>
		public void ForgetChanges()
		{
			_previousValues.Clear();
			_subsequentValues.Clear();
			CanUndoProperty.Value = false;
			CanRedoProperty.Value = false;
		}
		private void ReversibleProperty_ValueChanged(object sender, ValueChangedEventArgs<T> e)
		{
			if (_isStateUpdate)
				return;
			_previousValues.Push(e.OldValue);
			_subsequentValues.Clear();
			CanUndoProperty.Value = true;
			ChangeRemembered?.Invoke(this, EventArgs.Empty);
		}
	}
}