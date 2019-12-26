namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="bool"/> type.
	/// </summary>
	static public class BooleanSerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="bool"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<bool> Default;

		static BooleanSerializerBuilder() => Default = new BooleanSerializer();
	}
}