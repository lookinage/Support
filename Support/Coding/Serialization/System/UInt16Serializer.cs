namespace Support.Coding.Serialization.System
{
	internal sealed class UInt16Serializer : ConstantLengthSerializer<ushort>
	{
		internal UInt16Serializer() : base(sizeof(ushort)) { }

		public override sealed unsafe void Serialize(ushort instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(ushort*)p = instance;
		}
		public override sealed unsafe ushort Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(ushort*)p;
		}
	}
}