namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="byte"/> type.
	/// </summary>
	static public class ByteSerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="byte"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<byte> Default;

		static ByteSerializerBuilder() => Default = new ByteSerializer();
	}
}