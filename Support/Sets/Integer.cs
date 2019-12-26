namespace Support.Sets
{
	/// <summary>
	/// Represents an element of the set integers.
	/// </summary>
	public struct Integer : ILinearSetElement<Integer>, ISurjectiveSetElement<int>
	{
		/// <summary>
		/// Converts an <see cref="int"/> to an <see cref="Integer"/>.
		/// </summary>
		/// <param name="value">The <see cref="int"/>.</param>
		static public implicit operator Integer(int value) => new Integer(value);
		/// <summary>
		/// Converts an <see cref="Integer"/> to an <see cref="int"/>.
		/// </summary>
		/// <param name="value">The <see cref="Integer"/>.</param>
		static public implicit operator int(Integer value) => value._value;

		private readonly int _value;

		/// <summary>
		/// Initializes the <see cref="Integer"/>.
		/// </summary>
		/// <param name="value">The value of the <see cref="Integer"/>.</param>
		public Integer(int value) => _value = value;

		/// <summary>
		/// Gets the <see cref="int"/> the <see cref="Integer"/> is related to.
		/// </summary>
		public int RelatedElement => _value;

		/// <summary>
		/// Compares the <see cref="Integer"/> with an <see cref="object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to compare with the <see cref="Integer"/>.</param>
		/// <returns><see langword="true"/> whether <paramref name="obj"/> is an <see cref="Integer"/> structure and the <see cref="Integer"/> and <paramref name="obj"/> are equal.</returns>
		public override bool Equals(object obj) => obj is Integer integer && _value == integer._value;
		/// <summary>
		/// Calculates the hash code for the <see cref="Integer"/>.
		/// </summary>
		/// <returns>The hash code for the <see cref="Integer"/>.</returns>
		public override int GetHashCode() => _value.GetHashCode();
		/// <summary>
		/// Makes a <see cref="string"/> that representats the <see cref="Integer"/>.
		/// </summary>
		/// <returns>A <see cref="string"/> that representats the <see cref="Integer"/>.</returns>
		public override string ToString() => _value.ToString();
		/// <summary>
		/// Compares the <see cref="Integer"/> with another <see cref="Integer"/>.
		/// </summary>
		/// <param name="element">Another <see cref="Integer"/> to compare with the <see cref="Integer"/>.</param>
		/// <returns>A <see cref="LinearComparisonResults"/> of these elements comparison.</returns>
		public LinearComparisonResults Compare(Integer element) => _value == element._value ? LinearComparisonResults.Equals : _value > element._value ? LinearComparisonResults.Greater : LinearComparisonResults.Default;
		bool ISetElement<Integer>.Compare(Integer element) => Compare(element) == LinearComparisonResults.Equals;
	}
}