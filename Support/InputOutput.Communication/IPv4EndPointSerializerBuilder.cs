using Support.Coding.Serialization;

namespace Support.InputOutput.Communication
{
	/// <summary>
	/// Represents a builder of an <see cref="IConstantLengthSerializer{T}"/> of the <see cref="IPv4EndPoint"/> type.
	/// </summary>
	static public class IPv4EndPointSerializerBuilder
	{
		/// <summary>
		/// The default serializer of the <see cref="IPv4EndPoint"/> type.
		/// </summary>
		static public readonly IConstantLengthSerializer<IPv4EndPoint> Default;

		static IPv4EndPointSerializerBuilder() => Default = new IPv4EndPointSerializer();
	}
}