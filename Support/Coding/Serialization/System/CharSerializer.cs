namespace Support.Coding.Serialization.System
{
	internal sealed class CharSerializer : ConstantLengthSerializer<char>
	{
		internal CharSerializer() : base(sizeof(char)) { }

		public override sealed unsafe void Serialize(char instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(char*)p = instance;
		}
		public override sealed unsafe char Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(char*)p;
		}
	}
}