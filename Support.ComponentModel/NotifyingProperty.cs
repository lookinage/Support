using System;
using System.Collections;
using System.Collections.Generic;

namespace Support.ComponentModel
{
	/// <summary>
	/// Represents a property that notify about its value change.
	/// </summary>
	public class NotifyingProperty<T> : NotifyingObject
	{
		static private readonly IEqualityComparer _equilityComparer;

		static NotifyingProperty() => _equilityComparer = EqualityComparer<T>.Default;

		private readonly Predicate<T> _valuePredicate;
		private T _value;

		/// <summary>
		/// Initializes a new instance of the <see cref="NotifyingProperty{T}"/> class.
		/// </summary>
		public NotifyingProperty() : this(null, default) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="NotifyingProperty{T}"/> class.
		/// </summary>
		/// <param name="valuePredicate">The method to define values the property can set.</param>
		/// <exception cref="ArgumentException">The default value does not satisfy <paramref name="valuePredicate"/>.</exception>
		public NotifyingProperty(Predicate<T> valuePredicate) : this(valuePredicate, default) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="NotifyingProperty{T}"/> class.
		/// </summary>
		/// <param name="value">The start value of the property.</param>
		public NotifyingProperty(T value) : this(null, value) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="NotifyingProperty{T}"/> class.
		/// </summary>
		/// <param name="value">The start value of the property.</param>
		/// <param name="valuePredicate">The method to define values the property can set.</param>
		/// <exception cref="ArgumentException"><paramref name="value"/> does not satisfy <paramref name="valuePredicate"/>.</exception>
		public NotifyingProperty(Predicate<T> valuePredicate, T value)
		{
			if (valuePredicate != null && !valuePredicate(value))
				throw new ArgumentException(string.Format("{0} does not satisfy {1}.", nameof(value), nameof(valuePredicate)));
			_valuePredicate = valuePredicate;
			_value = value;
		}

		/// <summary>
		/// Gets or sets the value of the property.
		/// </summary>
		public T Value
		{
			get => _value;
			set
			{
				if (_equilityComparer.Equals(value, _value))
					return;
				if (_valuePredicate != null && !_valuePredicate(value))
					return;
				T lastValue = _value;
				_value = value;
				OnPropertyChanged(nameof(Value));
				ValueChanged?.Invoke(this, new ValueChangedEventArgs<T>(lastValue, _value));
			}
		}

		/// <summary>
		/// Occurs when the value changes.
		/// </summary>
		public event EventHandler<ValueChangedEventArgs<T>> ValueChanged;

		/// <summary>
		/// Determines whether the specified object is equal to the <see cref="NotifyingProperty{T}"/>.
		/// </summary>
		/// <param name="obj">The object to compare with the <see cref="NotifyingProperty{T}"/>.</param>
		/// <returns>true if the specified object is equal to the <see cref="NotifyingProperty{T}"/>; otherwise, false.</returns>
		public override bool Equals(object obj) => _equilityComparer.Equals(_value, obj);
		/// <summary>
		/// Returns a hash code for the <see cref="NotifyingProperty{T}"/>.
		/// </summary>
		/// <returns>A hash code for the <see cref="NotifyingProperty{T}"/>.</returns>
		public override int GetHashCode() => _equilityComparer.GetHashCode(_value);
		/// <summary>
		/// Returns a string that represents the <see cref="NotifyingProperty{T}"/>.
		/// </summary>
		/// <returns>A string that represents the <see cref="NotifyingProperty{T}"/>.</returns>
		public override string ToString() => _value != null ? _value.ToString() : "null";
	}
}