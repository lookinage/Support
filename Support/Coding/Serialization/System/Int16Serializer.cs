namespace Support.Coding.Serialization.System
{
	internal sealed class Int16Serializer : ConstantLengthSerializer<short>
	{
		internal Int16Serializer() : base(sizeof(short)) { }

		public override sealed unsafe void Serialize(short instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(short*)p = instance;
		}
		public override sealed unsafe short Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(short*)p;
		}
	}
}