using Noname.BitConversion;
using Noname.BitConversion.System;

namespace Noname.IO.ObjectOrientedDomain.Collections
{
	internal struct CollectionMeta
	{
		internal sealed class BitConverter : ConstantLengthBitConverter<CollectionMeta>
		{
			static internal readonly BitConverter _instance;

			static BitConverter() => _instance = new BitConverter();

			public BitConverter() : base(Int32BitConverterBuilder.Instance.ByteCount + Int32BitConverterBuilder.Instance.ByteCount + Int32BitConverterBuilder.Instance.ByteCount) { }

			public override sealed void GetBytes(CollectionMeta value, byte[] bytes, int index)
			{
				Int32BitConverterBuilder.Instance.GetBytes(value._count, bytes, ref index);
				Int32BitConverterBuilder.Instance.GetBytes(value._usedCount, bytes, ref index);
				Int32BitConverterBuilder.Instance.GetBytes(value._freeIndex, bytes, ref index);
			}
			public override sealed CollectionMeta GetInstance(byte[] bytes, int index) => new CollectionMeta(Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index), Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index), Int32BitConverterBuilder.Instance.GetInstance(bytes, ref index));
		}

		internal int _count;
		internal int _usedCount;
		internal int _freeIndex;

		public CollectionMeta(int count, int usedCount, int freeIndex)
		{
			_count = count;
			_usedCount = usedCount;
			_freeIndex = freeIndex;
		}
	}
}