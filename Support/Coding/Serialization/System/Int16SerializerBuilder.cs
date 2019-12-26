namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="short"/> type.
	/// </summary>
	static public class Int16SerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="short"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<short> Default;

		static Int16SerializerBuilder() => Default = new Int16Serializer();
	}
}