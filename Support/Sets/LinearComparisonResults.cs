namespace Support.Sets
{
	/// <summary>
	/// Specifies the results of linear set elements comparison.
	/// </summary>
	public enum LinearComparisonResults
	{
		/// <summary>
		/// Specifies that the coordinate of the element is less than the coordinate of another element.
		/// </summary>
		Default = 0x0,
		/// <summary>
		/// Specifies that the coordinate of the element equals to the coordinate of another element.
		/// </summary>
		Equals = 0x1,
		/// <summary>
		/// Specifies that the coordinate of the element is greater than the coordinate of another element.
		/// </summary>
		Greater = 0x2,
	}
}