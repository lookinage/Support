namespace Support.Coding.Serialization.System
{
	internal sealed class UInt32Serializer : ConstantLengthSerializer<uint>
	{
		internal UInt32Serializer() : base(sizeof(uint)) { }

		public override sealed unsafe void Serialize(uint instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(uint*)p = instance;
		}
		public override sealed unsafe uint Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(uint*)p;
		}
	}
}