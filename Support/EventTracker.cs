namespace Support
{
	/// <summary>
	/// References a method to be executed when the event occurs.
	/// </summary>
	/// <typeparam name="TSource">The type of the source of the event.</typeparam>
	/// <param name="source">The source of the event.</param>
	public delegate void EventTracker<in TSource>(TSource source);
	/// <summary>
	/// References a method to be executed when the event occurs.
	/// </summary>
	/// <typeparam name="TSource">The type of the source of the event.</typeparam>
	/// <typeparam name="TArgument">The type of the argument of the event.</typeparam>
	/// <param name="source">The source of the event.</param>
	/// <param name="argument">The argument of the event.</param>
	public delegate void EventTracker<in TSource, in TArgument>(TSource source, TArgument argument);
}