namespace Support.Coding.Serialization.System
{
	internal sealed class DoubleSerializer : ConstantLengthSerializer<double>
	{
		internal DoubleSerializer() : base(sizeof(double)) { }

		public override sealed unsafe void Serialize(double instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(double*)p = instance;
		}
		public override sealed unsafe double Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(double*)p;
		}
	}
}