namespace Support.Coding.Serialization.System
{
	internal sealed class Int64Serializer : ConstantLengthSerializer<long>
	{
		internal Int64Serializer() : base(sizeof(long)) { }

		public override sealed unsafe void Serialize(long instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(long*)p = instance;
		}
		public override sealed unsafe long Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(long*)p;
		}
	}
}