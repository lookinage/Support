using Support.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Support.ComponentModel.Collections
{
	/// <summary>
	/// Represents an <see cref="ObservableCollection{T}"/> modification that can be undone or redone.
	/// </summary>
	public class ReversibleObservableCollection<T> : ObservableCollection<T>, IReversible
	{
		private abstract class Change
		{
			private protected int _count;

			private protected Change(int count) => _count = count;

			internal abstract void Undo(ReversibleObservableCollection<T> collection);
			internal abstract void Redo(ReversibleObservableCollection<T> collection);
		}
		private class Adding : Change
		{
			private readonly int _startingIndex;
			private readonly IList _items;

			internal Adding(int startingIndex, IList items) : base(items.Count)
			{
				_startingIndex = startingIndex;
				_items = items;
			}

			internal override void Undo(ReversibleObservableCollection<T> collection)
			{
				for (int removeIndex = _startingIndex + _count - 1; removeIndex >= _startingIndex; removeIndex--)
					collection.RemoveItem(removeIndex);
			}
			internal override void Redo(ReversibleObservableCollection<T> collection)
			{
				int insertIndex = _startingIndex;
				foreach (T item in _items)
					collection.InsertItem(insertIndex++, item);
			}
		}
		private class Removing : Change
		{
			private readonly int _startingIndex;
			private readonly IList _items;

			internal Removing(int startingIndex, IList items) : base(items.Count)
			{
				_startingIndex = startingIndex;
				_items = items;
			}

			internal override void Undo(ReversibleObservableCollection<T> collection)
			{
				int insertIndex = _startingIndex;
				foreach (T item in _items)
					collection.InsertItem(insertIndex++, item);
			}
			internal override void Redo(ReversibleObservableCollection<T> collection)
			{
				for (int removeIndex = _startingIndex + _count - 1; removeIndex >= _startingIndex; removeIndex--)
					collection.RemoveItem(removeIndex);
			}
		}
		private class Replacing : Change
		{
			private readonly int _startingIndex;
			private readonly IList _oldItems;
			private readonly IList _newItems;

			internal Replacing(int startingIndex, IList oldItems, IList newItems) : base(oldItems.Count)
			{
				_startingIndex = startingIndex;
				if (oldItems.Count != newItems.Count)
					throw new ArgumentException();
				_oldItems = oldItems;
				_newItems = newItems;
			}

			internal override void Undo(ReversibleObservableCollection<T> collection)
			{
				for (int i = 0, replaceIndex = _startingIndex; i < _count; i++)
					collection.SetItem(replaceIndex++, (T)_oldItems[i]);
			}
			internal override void Redo(ReversibleObservableCollection<T> collection)
			{
				for (int i = 0, replaceIndex = _startingIndex; i < _count; i++)
					collection.SetItem(replaceIndex++, (T)_newItems[i]);
			}
		}
		private class Moving : Change
		{
			private readonly int _oldStartingIndex;
			private readonly int _newStartingIndex;

			internal Moving(int oldStartingIndex, int newStartingIndex, IList items) : base(items.Count)
			{
				_oldStartingIndex = oldStartingIndex;
				_newStartingIndex = newStartingIndex;
			}

			internal override void Undo(ReversibleObservableCollection<T> collection)
			{
				int removeIndex = _newStartingIndex;
				int insertIndex = _oldStartingIndex;
				for (int i = 0; i < _count; i++)
					collection.MoveItem(removeIndex++, insertIndex++);
			}
			internal override void Redo(ReversibleObservableCollection<T> collection)
			{
				int removeIndex = _oldStartingIndex;
				int insertIndex = _newStartingIndex;
				for (int i = 0; i < _count; i++)
					collection.MoveItem(removeIndex++, insertIndex++);
			}
		}

		private readonly Stack<Change> _previousChanges;
		private readonly Stack<Change> _subsequentChanges;
		private bool _isStateUpdate;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReversibleObservableCollection{T}"/> class.
		/// </summary>
		public ReversibleObservableCollection() : this(Enumerable.Empty<T>()) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="ReversibleObservableCollection{T}"/> class.
		/// </summary>
		/// <param name="collection">The collection from which the elements are copied.</param>
		public ReversibleObservableCollection(IEnumerable<T> collection) : base(collection)
		{
			_previousChanges = new Stack<Change>();
			_subsequentChanges = new Stack<Change>();
			_isStateUpdate = false;
			CanUndoProperty = new NotifyingProperty<bool>();
			CanRedoProperty = new NotifyingProperty<bool>();
			CollectionChanged += ReversibleObservableCollection_CollectionChanged;
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
		/// Undoes the state of this collection.
		/// </summary>
		/// <exception cref="InvalidOperationException">The object cannot be undone.</exception>
		public void Undo()
		{
			if (_previousChanges.Count == 0)
				throw new InvalidOperationException("The object cannot be undone.");
			Change change;
			_subsequentChanges.Push(change = _previousChanges.Pop());
			CanRedoProperty.Value = true;
			if (_previousChanges.Count == 0)
				CanUndoProperty.Value = false;
			_isStateUpdate = true;
			change.Undo(this);
			_isStateUpdate = false;
		}
		/// <summary>
		/// Redoes the state of this collection.
		/// </summary>
		/// <exception cref="InvalidOperationException">The object cannot be redone.</exception>
		public void Redo()
		{
			if (_subsequentChanges.Count == 0)
				throw new InvalidOperationException("The object cannot be redone.");
			Change change;
			_previousChanges.Push(change = _subsequentChanges.Pop());
			CanUndoProperty.Value = true;
			if (_subsequentChanges.Count == 0)
				CanRedoProperty.Value = false;
			_isStateUpdate = true;
			change.Redo(this);
			_isStateUpdate = false;
		}
		/// <summary>
		/// Removes the remembered changes.
		/// </summary>
		public void ForgetChanges()
		{
			_previousChanges.Clear();
			_subsequentChanges.Clear();
			CanUndoProperty.Value = false;
			CanRedoProperty.Value = false;
		}
		private void ReversibleObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (_isStateUpdate)
				return;
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					_previousChanges.Push(new Adding(e.NewStartingIndex, e.NewItems));
					break;
				case NotifyCollectionChangedAction.Remove:
					_previousChanges.Push(new Removing(e.OldStartingIndex, e.OldItems));
					break;
				case NotifyCollectionChangedAction.Replace:
					_previousChanges.Push(new Replacing(e.OldStartingIndex, e.OldItems, e.NewItems));
					break;
				case NotifyCollectionChangedAction.Move:
					_previousChanges.Push(new Moving(e.OldStartingIndex, e.NewStartingIndex, e.OldItems));
					break;
				case NotifyCollectionChangedAction.Reset:
					_previousChanges.Clear();
					break;
				default:
					throw new ArgumentException();
			}
			_subsequentChanges.Clear();
			CanUndoProperty.Value = true;
			ChangeRemembered?.Invoke(this, EventArgs.Empty);
		}
	}
}