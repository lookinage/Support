using System;

namespace Support.ComponentModel
{
	/// <summary>
	/// Defines a type that can be undone and redone.
	/// </summary>
	public interface IReversible
	{
		/// <summary>
		/// Provides access to the value indicating whether the object can be undone.
		/// </summary>
		NotifyingProperty<bool> CanUndoProperty { get; }
		/// <summary>
		/// Provides access to the value indicating whether the object can be redone.
		/// </summary>
		NotifyingProperty<bool> CanRedoProperty { get; }

		/// <summary>
		/// Occurs when a change was remembered.
		/// </summary>
		event EventHandler ChangeRemembered;

		/// <summary>
		/// Undoes the object state.
		/// </summary>
		/// <exception cref="InvalidOperationException">The object cannot be undone.</exception>
		void Undo();
		/// <summary>
		/// Redoes the object state.
		/// </summary>
		/// <exception cref="InvalidOperationException">The object cannot be redone.</exception>
		void Redo();
		/// <summary>
		/// Removes the remembered changes.
		/// </summary>
		void ForgetChanges();
	}
}