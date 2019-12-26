namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="int"/> type.
	/// </summary>
	static public class Int32SerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="int"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<int> Default;

		static Int32SerializerBuilder() => Default = new Int32Serializer();
	}
}