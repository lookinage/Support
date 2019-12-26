namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="float"/> type.
	/// </summary>
	static public class SingleSerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="float"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<float> Default;

		static SingleSerializerBuilder() => Default = new SingleSerializer();
	}
}