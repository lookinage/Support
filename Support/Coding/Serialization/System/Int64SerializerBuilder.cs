namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="long"/> type.
	/// </summary>
	static public class Int64SerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="long"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<long> Default;

		static Int64SerializerBuilder() => Default = new Int64Serializer();
	}
}