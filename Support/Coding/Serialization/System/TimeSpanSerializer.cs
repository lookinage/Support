using System;

namespace Support.Coding.Serialization.System
{
	internal sealed class TimeSpanSerializer : ConstantLengthSerializer<TimeSpan>
	{
		internal unsafe TimeSpanSerializer() : base(sizeof(TimeSpan)) { }

		public override sealed unsafe void Serialize(TimeSpan instance, byte[] buffer, int index)
		{
			ValidateSerialize(instance, buffer, index);
			fixed (byte* p = &buffer[index])
				*(TimeSpan*)p = instance;
		}
		public override sealed unsafe TimeSpan Deserialize(byte[] buffer, int index)
		{
			ValidateDeserialize(buffer, index);
			fixed (byte* p = &buffer[index])
				return *(TimeSpan*)p;
		}
	}
}