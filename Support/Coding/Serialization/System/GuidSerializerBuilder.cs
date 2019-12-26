using System;

namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="Guid"/> type.
	/// </summary>
	static public class GuidSerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="Guid"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<Guid> Default;

		static GuidSerializerBuilder() => Default = new GuidSerializer();
	}
}