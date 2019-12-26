namespace Support.Coding.Serialization.System
{
	internal sealed class SByteSerializer : ConstantLengthSerializer<sbyte>
	{
		internal SByteSerializer() : base(sizeof(sbyte)) { }

		public override sealed unsafe void Serialize(sbyte instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			buffer[index] = *(byte*)&instance;
		}
		public override sealed unsafe sbyte Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(sbyte*)p;
		}
	}
}