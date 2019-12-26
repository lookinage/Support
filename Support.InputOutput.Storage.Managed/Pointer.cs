//using Support.Coding.Serialization;
//using Support.Coding.Serialization.System;

//namespace Support.InputOutput.Storage.Managed
//{
//	internal struct Pointer
//	{
//		internal sealed class Serializer : ConstantLengthSerializer<Pointer>
//		{
//			static internal readonly Serializer _default;

//			static Serializer() => _default = new Serializer();

//			internal Serializer() : base(sizeof(long) + sizeof(long)) { }

//			public override sealed void Serialize(Pointer instance, byte[] buffer, int index)
//			{
//				Int64SerializerBuilder.Default.Serialize(instance._size, buffer, ref index);
//				Int64SerializerBuilder.Default.Serialize(instance._address, buffer, ref index);
//			}
//			public override sealed Pointer Deserialize(byte[] buffer, int index) => new Pointer(Int64SerializerBuilder.Default.Deserialize(buffer, ref index), Int64SerializerBuilder.Default.Deserialize(buffer, ref index));
//		}

//		internal long _size;
//		internal long _address;

//		internal Pointer(long size, long address)
//		{
//			_size = size;
//			_address = address;
//		}
//	}
//}