using System;

namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="TimeSpan"/> type.
	/// </summary>
	static public class TimeSpanSerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="TimeSpan"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<TimeSpan> Default;

		static TimeSpanSerializerBuilder() => Default = new TimeSpanSerializer();
	}
}