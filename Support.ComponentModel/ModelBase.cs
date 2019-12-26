using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Support.ComponentModel
{
	/// <summary>
	/// Represents a model that consists of the <see cref="NotifyingObject"/> instances.
	/// </summary>
	public class ModelBase : NotifyingObject, IReversible
	{
		private readonly ObservableCollection<NotifyingObject> _reversibleObjectCollection;
		private readonly Stack<IReversible> _previousRememberedReversibles;
		private readonly Stack<IReversible> _subsequentRememberedReversibles;

		/// <summary>
		/// Initializes a new instance of the <see cref="ModelBase"/> class.
		/// </summary>
		public ModelBase()
		{
			_reversibleObjectCollection = new ObservableCollection<NotifyingObject>();
			_previousRememberedReversibles = new Stack<IReversible>();
			_subsequentRememberedReversibles = new Stack<IReversible>();
			CanUndoProperty = new NotifyingProperty<bool>();
			CanRedoProperty = new NotifyingProperty<bool>();
		}

		/// <summary>
		/// Provides access to the value indicating whether the model can be undone.
		/// </summary>
		public NotifyingProperty<bool> CanUndoProperty { get; }
		/// <summary>
		/// Provides access to the value indicating whether the model can be redone.
		/// </summary>
		public NotifyingProperty<bool> CanRedoProperty { get; }

		/// <summary>
		/// Occurs when a change was remembered.
		/// </summary>
		public event EventHandler ChangeRemembered;

		/// <summary>
		/// Adds the specified object to the model.
		/// </summary>
		/// <param name="notifyingObject">An adding object.</param>
		/// <exception cref="ArgumentException">The model already contains the specified object.</exception>
		protected void Add(NotifyingObject notifyingObject)
		{
			if (_reversibleObjectCollection.Contains(notifyingObject))
				throw new ArgumentException("The model already contains the specified object.");
			_reversibleObjectCollection.Add(notifyingObject);
			if (notifyingObject is IReversible reversible)
				reversible.ChangeRemembered += ModelBase_ChangeRemembered;
		}
		/// <summary>
		/// Undoes the model state.
		/// </summary>
		/// <exception cref="InvalidOperationException">The property cannot be undone.</exception>
		public void Undo()
		{
			if (_previousRememberedReversibles.Count == 0)
				throw new InvalidOperationException("The property cannot be undone.");
			IReversible reversible;
			_subsequentRememberedReversibles.Push(reversible = _previousRememberedReversibles.Pop());
			CanRedoProperty.Value = true;
			if (_previousRememberedReversibles.Count == 0)
				CanUndoProperty.Value = false;
			reversible.Undo();
		}
		/// <summary>
		/// Redoes the model state.
		/// </summary>
		/// <exception cref="InvalidOperationException">The property cannot be redone.</exception>
		public void Redo()
		{
			if (_subsequentRememberedReversibles.Count == 0)
				throw new InvalidOperationException("The property cannot be redone.");
			IReversible reversible;
			_previousRememberedReversibles.Push(reversible = _subsequentRememberedReversibles.Pop());
			CanUndoProperty.Value = true;
			if (_subsequentRememberedReversibles.Count == 0)
				CanRedoProperty.Value = false;
			reversible.Undo();
		}
		/// <summary>
		/// Removes the remembered changes.
		/// </summary>
		public void ForgetChanges()
		{
			foreach (IReversible reversible in _previousRememberedReversibles)
				reversible.ForgetChanges();
			_previousRememberedReversibles.Clear();
			foreach (IReversible reversible in _subsequentRememberedReversibles)
				reversible.ForgetChanges();
			_subsequentRememberedReversibles.Clear();
			CanUndoProperty.Value = false;
			CanRedoProperty.Value = false;
		}
		private void ModelBase_ChangeRemembered(object sender, EventArgs e)
		{
			if (!(sender is IReversible reversible))
				throw new InvalidOperationException();
			_previousRememberedReversibles.Push(reversible);
			_subsequentRememberedReversibles.Clear();
			CanUndoProperty.Value = true;
			ChangeRemembered?.Invoke(this, EventArgs.Empty);
		}
	}
}