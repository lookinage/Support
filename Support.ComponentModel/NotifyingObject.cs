using System.ComponentModel;

namespace Support.ComponentModel
{
	/// <summary>
	/// Represents base class for all objects that notify of its properties change.
	/// </summary>
	public abstract class NotifyingObject : INotifyPropertyChanged
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NotifyingObject"/> class.
		/// </summary>
		protected NotifyingObject() { }

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Invokes <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">The name of the changed property.</param>
		protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}