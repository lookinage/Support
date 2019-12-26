using Support.Coding.Serialization;
using Support.Coding.Serialization.System;

namespace Support.InputOutput.Communication
{
	internal sealed class IPv4EndPointSerializer : ConstantLengthSerializer<IPv4EndPoint>
	{
		internal IPv4EndPointSerializer() : base(UInt32SerializerBuilder.Default.Count + UInt16SerializerBuilder.Default.Count) { }

		public override sealed unsafe void Serialize(IPv4EndPoint instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			UInt32SerializerBuilder.Default.Serialize(instance.Address, buffer, index);
			UInt16SerializerBuilder.Default.Serialize(instance.Port, buffer, index + UInt32SerializerBuilder.Default.Count);
		}
		public override sealed unsafe IPv4EndPoint Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			return new IPv4EndPoint(UInt32SerializerBuilder.Default.Deserialize(buffer, index), UInt16SerializerBuilder.Default.Deserialize(buffer, index + UInt32SerializerBuilder.Default.Count));
		}
	}
}