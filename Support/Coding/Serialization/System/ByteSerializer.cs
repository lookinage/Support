namespace Support.Coding.Serialization.System
{
	internal sealed class ByteSerializer : ConstantLengthSerializer<byte>
	{
		internal ByteSerializer() : base(sizeof(byte)) { }

		public override sealed void Serialize(byte instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			buffer[index] = instance;
		}
		public override sealed byte Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			return buffer[index];
		}
	}
}