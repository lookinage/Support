using System;
using System.Text;

namespace Support.Coding.Serialization.System
{
	internal sealed class StringSerializer : Serializer<string>
	{
		private readonly Encoding _encoding;

		internal StringSerializer(Encoding encoding) => _encoding = encoding;

		public override sealed int Count(string instance)
		{
			ValidateCount(instance);
			try { return _encoding.GetByteCount(instance); }
			catch (Exception exception) { throw new ArgumentException("An error occurred.", exception); }
		}
		public override sealed void Serialize(string instance, byte[] buffer, ref int index)
		{
			ValidateSerialize(instance, buffer, index);
			try { index += _encoding.GetBytes(instance, 0, instance.Length, buffer, index); }
			catch (Exception exception) { throw new ArgumentException("An error occurred.", exception); }
		}
		public override sealed string Deserialize(int count, byte[] buffer, int index)
		{
			ValidateDeserialize(count, buffer, index);
			try { return _encoding.GetString(buffer, index, count); }
			catch (Exception exception) { throw new ArgumentException("An error occurred.", exception); }
		}
	}
}