namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="ulong"/> type.
	/// </summary>
	static public class UInt64SerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="ulong"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<ulong> Default;

		static UInt64SerializerBuilder() => Default = new UInt64Serializer();
	}
}