namespace Support.Coding.Serialization.System
{
	internal sealed class UInt64Serializer : ConstantLengthSerializer<ulong>
	{
		internal UInt64Serializer() : base(sizeof(ulong)) { }

		public override sealed unsafe void Serialize(ulong instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(ulong*)p = instance;
		}
		public override sealed unsafe ulong Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(ulong*)p;
		}
	}
}