namespace Support.Coding.Serialization.System
{
	internal sealed class BooleanSerializer : ConstantLengthSerializer<bool>
	{
		internal BooleanSerializer() : base(sizeof(bool)) { }

		public override sealed unsafe void Serialize(bool instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			buffer[index] = *(byte*)&instance;
		}
		public override sealed unsafe bool Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(bool*)p;
		}
	}
}