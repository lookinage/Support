namespace Support.Coding.Serialization.System
{
	internal sealed class Int32Serializer : ConstantLengthSerializer<int>
	{
		internal Int32Serializer() : base(sizeof(int)) { }

		public override sealed unsafe void Serialize(int instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(int*)p = instance;
		}
		public override sealed unsafe int Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(int*)p;
		}
	}
}