namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="double"/> type.
	/// </summary>
	static public class DoubleSerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="double"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<double> Default;

		static DoubleSerializerBuilder() => Default = new DoubleSerializer();
	}
}