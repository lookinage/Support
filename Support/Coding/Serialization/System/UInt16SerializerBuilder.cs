namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="ushort"/> type.
	/// </summary>
	static public class UInt16SerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="ushort"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<ushort> Default;

		static UInt16SerializerBuilder() => Default = new UInt16Serializer();
	}
}