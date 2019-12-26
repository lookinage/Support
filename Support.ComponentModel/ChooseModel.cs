using System;
using System.Collections.Generic;
using System.Linq;

namespace Support.ComponentModel
{
	/// <summary>
	/// Represents a model to choose some property value by an associated text.
	/// </summary>
	/// <typeparam name="T">The type of the property value that must be set.</typeparam>
	public class ChooseModel<T> : ModelBase
	{
		private readonly IEnumerable<T> _values;
		private readonly Func<T, string> _getAssociatedText;

		/// <summary>
		/// Initializes a new instance of the <see cref="ChooseModel{T}"/> class.
		/// </summary>
		/// <param name="property">A property which value must be set.</param>
		/// <param name="values">A collection of available values.</param>
		/// <param name="getAssociatedText">A method that returns an associated text of the specified value.</param>
		/// <exception cref="ArgumentNullException"><paramref name="property"/> is null.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="getAssociatedText"/> is null.</exception>
		public ChooseModel(NotifyingProperty<T> property, IEnumerable<T> values, Func<T, string> getAssociatedText)
		{
			Property = property ?? throw new ArgumentNullException();
			_values = values ?? throw new ArgumentNullException();
			_getAssociatedText = getAssociatedText ?? throw new ArgumentNullException();
			FilteredValuesProperty = new NotifyingProperty<IEnumerable<T>>();
			Add(FilterTextProperty = new ReversibleProperty<string>());
			FilterTextProperty.ValueChanged += FilterTextProperty_ValueChanged;
			SynchronizeFilteredValues();
		}

		/// <summary>
		/// Gets the property which value must be set.
		/// </summary>
		public NotifyingProperty<T> Property { get; }
		/// <summary>
		/// Gets the collection of filtered values.
		/// </summary>
		public NotifyingProperty<IEnumerable<T>> FilteredValuesProperty { get; }
		/// <summary>
		/// Gets the property containing the filter text.
		/// </summary>
		public ReversibleProperty<string> FilterTextProperty { get; }

		private void SynchronizeFilteredValues()
		{
			string filterText = FilterTextProperty.Value;
			if (string.IsNullOrEmpty(filterText))
				FilteredValuesProperty.Value = _values;
			else
			{
				filterText = filterText.ToLower();
				FilteredValuesProperty.Value = _values.Where(value => _getAssociatedText(value).ToLower().Contains(filterText));
			}
		}
		private void FilterTextProperty_ValueChanged(object sender, ValueChangedEventArgs<string> e) => SynchronizeFilteredValues();
	}
}