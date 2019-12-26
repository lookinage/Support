namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="uint"/> type.
	/// </summary>
	static public class UInt32SerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="uint"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<uint> Default;

		static UInt32SerializerBuilder() => Default = new UInt32Serializer();
	}
}