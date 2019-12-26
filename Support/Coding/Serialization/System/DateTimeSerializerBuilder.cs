using System;

namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="DateTime"/> type.
	/// </summary>
	static public class DateTimeSerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="DateTime"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<DateTime> Default;

		static DateTimeSerializerBuilder() => Default = new DateTimeSerializer();
	}
}