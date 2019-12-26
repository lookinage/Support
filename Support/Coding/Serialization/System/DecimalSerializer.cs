namespace Support.Coding.Serialization.System
{
	internal sealed class DecimalSerializer : ConstantLengthSerializer<decimal>
	{
		internal DecimalSerializer() : base(sizeof(decimal)) { }

		public override sealed unsafe void Serialize(decimal instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(decimal*)p = instance;
		}
		public override sealed unsafe decimal Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(decimal*)p;
		}
	}
}