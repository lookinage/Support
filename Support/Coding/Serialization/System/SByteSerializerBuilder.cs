namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="sbyte"/> type.
	/// </summary>
	static public class SByteSerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="sbyte"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<sbyte> Default;

		static SByteSerializerBuilder() => Default = new SByteSerializer();
	}
}