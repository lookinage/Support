using System;

namespace Support.Coding.Serialization.System
{
	internal sealed class DateTimeSerializer : ConstantLengthSerializer<DateTime>
	{
		internal unsafe DateTimeSerializer() : base(sizeof(DateTime)) { }

		public override sealed unsafe void Serialize(DateTime instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(DateTime*)p = instance;
		}
		public override sealed unsafe DateTime Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(DateTime*)p;
		}
	}
}