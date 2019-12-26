namespace Support.Coding.Serialization.System
{
	internal sealed class SingleSerializer : ConstantLengthSerializer<float>
	{
		internal SingleSerializer() : base(sizeof(float)) { }

		public override sealed unsafe void Serialize(float instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(float*)p = instance;
		}
		public override sealed unsafe float Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(float*)p;
		}
	}
}