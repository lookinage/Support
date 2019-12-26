//using System;
//using Noname.BitConversion;

//namespace Noname.IO.ObjectOrientedDomain.Collections
//{
//	/// <summary>
//	/// Provides methods to initialize instances of the <see cref="CollectionManager"/> class.
//	/// </summary>
//	static public class CollectionManagerActivator
//	{
//		internal const int _defaultShardCapacity = 0x4;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="CollectionManager"/> class to create and manage the collection in the domain.
//		/// </summary>
//		/// <param name="domainManager">The <see cref="DomainManager"/> of the domain of the collection.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <param name="pointerCount">The number of pointers of an element.</param>
//		/// <param name="shardCount">The number of shards of the collection.</param>
//		/// <returns>A new instance of the <see cref="CollectionManager"/> class to create and manage the collection in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="domainManager"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="pointerCount"/> is less than 0 or <paramref name="shardCount"/> is less than 0.</exception>
//		static public CollectionManager Create(DomainManager domainManager, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter, int pointerCount, short shardCount) where TDataConstantLength : struct where TDataVariableLength : struct
//		{
//			if (domainManager == null)
//				throw new ArgumentNullException(nameof(domainManager));
//			if (dataConstantLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataConstantLengthBitConverter));
//			if (dataVariableLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataVariableLengthBitConverter));
//			if (pointerCount < 0)
//				throw new ArgumentOutOfRangeException(nameof(pointerCount));
//			if (shardCount < 0)
//				throw new ArgumentNullException(nameof(shardCount));
//			CollectionEntryConstantLength<TDataConstantLength>.BitConverter entryConstantLengthBitConverter = CollectionEntryConstantLength<TDataConstantLength>.BitConverter.GetInstance(dataConstantLengthBitConverter);
//			CollectionEntryVariableLength<TDataVariableLength>.BitConverter entryVariableLengthBitConverter = CollectionEntryVariableLength<TDataVariableLength>.BitConverter.GetInstance(dataVariableLengthBitConverter);
//			ValueManager<CollectionMeta, EmptyData> metaManager = ValueManagerActivator.Create(domainManager, CollectionMeta.BitConverter._instance, EmptyData.VariableLengthBitConverterInstance, 1);
//			ArrayManager<EmptyData, EmptyData> entryShardManager = ArrayManagerActivator.Create(domainManager, EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance, 1, shardCount);
//			ArrayManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>>[] entryShards = new ArrayManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>>[shardCount];
//			metaManager[0] = entryShardManager.Pointer;
//			metaManager.DataConstantLength = new CollectionMeta(0, 0, -1);
//			return new CollectionManager(metaManager, entryConstantLengthBitConverter, entryVariableLengthBitConverter, pointerCount, entryShards);
//		}
//		/// <summary>
//		/// Initializes a new instance of the <see cref="CollectionManager"/> class to create and manage the collection in the domain.
//		/// </summary>
//		/// <param name="domainManager">The <see cref="DomainManager"/> of the domain of the collection.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <param name="pointerCount">The number of pointers of an element.</param>
//		/// <returns>A new instance of the <see cref="CollectionManager"/> class to create and manage the collection in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="domainManager"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="pointerCount"/> is less than 1.</exception>
//		static public CollectionManager Create(DomainManager domainManager, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter, int pointerCount) where TDataConstantLength : struct where TDataVariableLength : struct => Create(domainManager, dataConstantLengthBitConverter, dataVariableLengthBitConverter, pointerCount, 0);
//		/// <summary>
//		/// Initializes a new instance of the <see cref="CollectionManager"/> class to manage the existing collection in the domain.
//		/// </summary>
//		/// <param name="pointerManager">The <see cref="PointerManager"/> that points to the collection.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the constant length data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <returns>A new instance of the <see cref="CollectionManager"/> class to manage the existing collection in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="pointerManager"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="PointerNullException"><paramref name="pointerManager"/> does not point to an object.</exception>
//		static public CollectionManager Open(PointerManager pointerManager, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter) where TDataConstantLength : struct where TDataVariableLength : struct
//		{
//			if (pointerManager == null)
//				throw new ArgumentNullException(nameof(pointerManager));
//			if (dataConstantLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataConstantLengthBitConverter));
//			if (dataVariableLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataVariableLengthBitConverter));
//			CollectionEntryConstantLength<TDataConstantLength>.BitConverter entryConstantLengthBitConverter = CollectionEntryConstantLength<TDataConstantLength>.BitConverter.GetInstance(dataConstantLengthBitConverter);
//			CollectionEntryVariableLength<TDataVariableLength>.BitConverter entryVariableLengthBitConverter = CollectionEntryVariableLength<TDataVariableLength>.BitConverter.GetInstance(dataVariableLengthBitConverter);
//			ValueManager<CollectionMeta, EmptyData> metaManager = ValueManagerActivator.Open(pointerManager, CollectionMeta.BitConverter._instance, EmptyData.VariableLengthBitConverterInstance);
//			ArrayManager<EmptyData, EmptyData> entryShardManager = ArrayManagerActivator.Open(metaManager[0], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance);
//			int shardCount = entryShardManager.Length;
//			ArrayManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>>[] entryShards = new ArrayManager<CollectionEntryConstantLength<TDataConstantLength>, CollectionEntryVariableLength<TDataVariableLength>>[shardCount];
//			for (int shardIndex = 0; shardIndex < shardCount; shardIndex++)
//			{
//				PointerManager entryShardPointer = entryShardManager[shardIndex][0];
//				if (entryShardPointer.IsNull)
//					break;
//				entryShards[shardIndex] = ArrayManagerActivator.Open(entryShardPointer, entryConstantLengthBitConverter, entryVariableLengthBitConverter);
//			}
//			return new CollectionManager(metaManager, entryConstantLengthBitConverter, entryVariableLengthBitConverter, entryShards[0].PointerCount, entryShards);
//		}
//	}
//}