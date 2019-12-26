using System;

namespace Support.Coding.Serialization.System
{
	internal sealed class GuidSerializer : ConstantLengthSerializer<Guid>
	{
		internal unsafe GuidSerializer() : base(sizeof(Guid)) { }

		public override sealed unsafe void Serialize(Guid instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(Guid*)p = instance;
		}
		public override sealed unsafe Guid Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(Guid*)p;
		}
	}
}