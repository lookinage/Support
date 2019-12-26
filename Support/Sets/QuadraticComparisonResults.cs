namespace Support.Sets
{
	/// <summary>
	/// Specifies the results of quadratic set elements comparison.
	/// </summary>
	public enum QuadraticComparisonResults
	{
		/// <summary>
		/// Specifies that the all coordinates of the element is less than the all coordinates of another element respectively.
		/// </summary>
		Default = 0x0,
		/// <summary>
		/// Specifies that the coordinates of the element equals to the coordinates of another element.
		/// </summary>
		Equals = 0x5,
		/// <summary>
		/// Specifies that the X coordinate of the element equals to the X coordinate of another element.
		/// </summary>
		XEquals = 0x1,
		/// <summary>
		/// Specifies that the X coordinate of the element is greater than the X coordinate of another element.
		/// </summary>
		XGreater = 0x2,
		/// <summary>
		/// Specifies that the Y coordinate of the element equals to the Y coordinate of another element.
		/// </summary>
		YEquals = 0x4,
		/// <summary>
		/// Specifies that the Y coordinate of the element is greater than the Y coordinate of another element.
		/// </summary>
		YGreater = 0x8,
	}
}