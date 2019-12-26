namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="decimal"/> type.
	/// </summary>
	static public class DecimalSerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="decimal"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<decimal> Default;

		static DecimalSerializerBuilder() => Default = new DecimalSerializer();
	}
}