namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="char"/> type.
	/// </summary>
	static public class CharSerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="char"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<char> Default;

		static CharSerializerBuilder() => Default = new CharSerializer();
	}
}