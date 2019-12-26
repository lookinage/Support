//using System;
//using System.Collections.Generic;
//using Noname.BitConversion;
//using Noname.BitConversion.System;

//namespace Noname.IO.ObjectOrientedDomain.Collections
//{
//	/// <summary>
//	/// Provides methods to initialize instances of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class.
//	/// </summary>
//	static public class DictionaryManagerActivator
//	{
//		/// <summary>
//		/// Initializes a new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.
//		/// </summary>
//		/// <param name="domainManager">The <see cref="DomainManager"/> of the domain of the dictionary.</param>
//		/// <param name="keyBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the keys in the dictionary.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <param name="pointerCount">The number of pointers of an element.</param>
//		/// <param name="comparer">The <see cref= "IEqualityComparer{TKey}" /> implementation to use when comparing keys, or null to use the default <see cref="EqualityComparer{TData}" /> for the type of the key.</param>
//		/// <param name="shardCount">The number of shards of the dictionary.</param>
//		/// <returns>A new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="domainManager"/> is null or <paramref name="keyBitConverter"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="pointerCount"/> is less than 0 or <paramref name="shardCount"/> is less than 0.</exception>
//		static public DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> Create<TKey, TDataConstantLength, TDataVariableLength>(DomainManager domainManager, ConstantLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter, int pointerCount, IEqualityComparer<TKey> comparer, short shardCount) where TKey : struct where TDataConstantLength : struct where TDataVariableLength : struct
//		{
//			if (domainManager == null)
//				throw new ArgumentNullException(nameof(domainManager));
//			if (dataConstantLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataConstantLengthBitConverter));
//			if (dataVariableLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataVariableLengthBitConverter));
//			if (pointerCount < 0)
//				throw new ArgumentNullException(nameof(pointerCount));
//			if (shardCount < 0)
//				throw new ArgumentNullException(nameof(shardCount));
//			DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength>.BitConverter entryConstantLengthBitConverter = DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength>.BitConverter.GetInstance(keyBitConverter, dataConstantLengthBitConverter);
//			DictionaryEntryConstantLengthVariableLength<TDataVariableLength>.BitConverter entryVariableLengthBitConverter = DictionaryEntryConstantLengthVariableLength<TDataVariableLength>.BitConverter.GetInstance(dataVariableLengthBitConverter);
//			ValueManager<DictionaryMeta, EmptyData> metaManager = ValueManagerActivator.Create(domainManager, DictionaryMeta.BitConverter._instance, EmptyData.VariableLengthBitConverterInstance, 2);
//			ArrayManager<EmptyData, EmptyData> bucketShardManager = ArrayManagerActivator.Create(domainManager, EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance, 1, shardCount);
//			ArrayManager<EmptyData, EmptyData> entryShardManager = ArrayManagerActivator.Create(domainManager, EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance, 1, shardCount);
//			ArrayManager<int, EmptyData>[] bucketShards = new ArrayManager<int, EmptyData>[shardCount];
//			ArrayManager<DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength>, DictionaryEntryConstantLengthVariableLength<TDataVariableLength>>[] entryShards = new ArrayManager<DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength>, DictionaryEntryConstantLengthVariableLength<TDataVariableLength>>[shardCount];
//			metaManager.DataConstantLength = new DictionaryMeta(0, 0, -1);
//			metaManager[0] = bucketShardManager.Pointer;
//			metaManager[1] = entryShardManager.Pointer;
//			return new DictionaryManagerConstantLength<TKey, TDataConstantLength, TDataVariableLength>(metaManager, entryConstantLengthBitConverter, entryVariableLengthBitConverter, pointerCount, comparer ?? EqualityComparer<TKey>.Default, bucketShards, entryShards);
//		}
//		/// <summary>
//		/// Initializes a new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.
//		/// </summary>
//		/// <param name="domainManager">The <see cref="DomainManager"/> of the domain of the dictionary.</param>
//		/// <param name="keyBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the keys in the dictionary.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <param name="pointerCount">The number of pointers of an element.</param>
//		/// <param name="shardCount">The number of shards of the dictionary.</param>
//		/// <returns>A new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="domainManager"/> is null or <paramref name="keyBitConverter"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="pointerCount"/> is less than 0 or <paramref name="shardCount"/> is less than 0.</exception>
//		static public DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> Create<TKey, TDataConstantLength, TDataVariableLength>(DomainManager domainManager, ConstantLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter, int pointerCount, short shardCount) where TKey : struct where TDataConstantLength : struct where TDataVariableLength : struct => Create(domainManager, keyBitConverter, dataConstantLengthBitConverter, dataVariableLengthBitConverter, pointerCount, null, shardCount);
//		/// <summary>
//		/// Initializes a new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.
//		/// </summary>
//		/// <param name="domainManager">The <see cref="DomainManager"/> of the domain of the dictionary.</param>
//		/// <param name="keyBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the keys in the dictionary.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <param name="pointerCount">The number of pointers of an element.</param>
//		/// <returns>A new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="domainManager"/> is null or <paramref name="keyBitConverter"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="pointerCount"/> is less than 0.</exception>
//		static public DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> Create<TKey, TDataConstantLength, TDataVariableLength>(DomainManager domainManager, ConstantLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter, int pointerCount) where TKey : struct where TDataConstantLength : struct where TDataVariableLength : struct => Create(domainManager, keyBitConverter, dataConstantLengthBitConverter, dataVariableLengthBitConverter, pointerCount, null, 0);
//		/// <summary>
//		/// Initializes a new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.
//		/// </summary>
//		/// <param name="domainManager">The <see cref="DomainManager"/> of the domain of the dictionary.</param>
//		/// <param name="keyBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the keys in the dictionary.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <param name="pointerCount">The number of pointers of an element.</param>
//		/// <param name="comparer">The <see cref= "IEqualityComparer{TKey}" /> implementation to use when comparing keys, or null to use the default <see cref="EqualityComparer{TData}" /> for the type of the key.</param>
//		/// <param name="shardCount">The number of shards of the dictionary.</param>
//		/// <returns>A new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="domainManager"/> is null or <paramref name="keyBitConverter"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="pointerCount"/> is less than 0 or <paramref name="shardCount"/> is less than 0.</exception>
//		static public DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> Create<TKey, TDataConstantLength, TDataVariableLength>(DomainManager domainManager, VariableLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter, int pointerCount, IEqualityComparer<TKey> comparer, short shardCount) where TDataConstantLength : struct where TDataVariableLength : struct
//		{
//			if (domainManager == null)
//				throw new ArgumentNullException(nameof(domainManager));
//			if (dataConstantLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataConstantLengthBitConverter));
//			if (dataVariableLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataVariableLengthBitConverter));
//			if (pointerCount < 0)
//				throw new ArgumentNullException(nameof(pointerCount));
//			if (shardCount < 0)
//				throw new ArgumentNullException(nameof(shardCount));
//			DictionaryEntryVariableLengthConstantLength<TDataConstantLength>.BitConverter entryConstantLengthBitConverter = DictionaryEntryVariableLengthConstantLength<TDataConstantLength>.BitConverter.GetInstance(dataConstantLengthBitConverter);
//			DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>.BitConverter entryVariableLengthBitConverter = DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>.BitConverter.GetInstance(keyBitConverter, dataVariableLengthBitConverter);
//			ValueManager<DictionaryMeta, EmptyData> metaManager = ValueManagerActivator.Create(domainManager, DictionaryMeta.BitConverter._instance, EmptyData.VariableLengthBitConverterInstance, 2);
//			ArrayManager<EmptyData, EmptyData> bucketShardManager = ArrayManagerActivator.Create(domainManager, EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance, 1, shardCount);
//			ArrayManager<EmptyData, EmptyData> entryShardManager = ArrayManagerActivator.Create(domainManager, EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance, 1, shardCount);
//			ArrayManager<int, EmptyData>[] bucketShards = new ArrayManager<int, EmptyData>[shardCount];
//			ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>>[] entryShards = new ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>>[shardCount];
//			metaManager.DataConstantLength = new DictionaryMeta(0, 0, -1);
//			metaManager[0] = bucketShardManager.Pointer;
//			metaManager[1] = entryShardManager.Pointer;
//			return new DictionaryManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength>(metaManager, entryConstantLengthBitConverter, entryVariableLengthBitConverter, pointerCount, comparer ?? EqualityComparer<TKey>.Default, bucketShards, entryShards);
//		}
//		/// <summary>
//		/// Initializes a new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.
//		/// </summary>
//		/// <param name="domainManager">The <see cref="DomainManager"/> of the domain of the dictionary.</param>
//		/// <param name="keyBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the keys in the dictionary.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <param name="pointerCount">The number of pointers of an element.</param>
//		/// <param name="shardCount">The number of shards of the dictionary.</param>
//		/// <returns>A new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="domainManager"/> is null or <paramref name="keyBitConverter"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="pointerCount"/> is less than 0 or <paramref name="shardCount"/> is less than 0.</exception>
//		static public DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> Create<TKey, TDataConstantLength, TDataVariableLength>(DomainManager domainManager, VariableLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter, int pointerCount, short shardCount) where TDataConstantLength : struct where TDataVariableLength : struct => Create(domainManager, keyBitConverter, dataConstantLengthBitConverter, dataVariableLengthBitConverter, pointerCount, null, shardCount);
//		/// <summary>
//		/// Initializes a new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.
//		/// </summary>
//		/// <param name="domainManager">The <see cref="DomainManager"/> of the domain of the dictionary.</param>
//		/// <param name="keyBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the keys in the dictionary.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <param name="pointerCount">The number of pointers of an element.</param>
//		/// <returns>A new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to create and manage the dictionary in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="domainManager"/> is null or <paramref name="keyBitConverter"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="pointerCount"/> is less than 0.</exception>
//		static public DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> Create<TKey, TDataConstantLength, TDataVariableLength>(DomainManager domainManager, VariableLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter, int pointerCount) where TDataConstantLength : struct where TDataVariableLength : struct => Create(domainManager, keyBitConverter, dataConstantLengthBitConverter, dataVariableLengthBitConverter, pointerCount, null, 0);
//		/// <summary>
//		/// Initializes a new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to manage the existing dictionary in the domain.
//		/// </summary>
//		/// <param name="pointerManager">The <see cref="PointerManager"/> that points to the dictionary.</param>
//		/// <param name="keyBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the keys in the dictionary.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <param name="comparer">The <see cref= "IEqualityComparer{TKey}" /> implementation to use when comparing keys, or null to use the default <see cref="EqualityComparer{TData}" /> for the type of the key.</param>
//		/// <returns>A new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to manage the existing dictionary in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="pointerManager"/> is null or <paramref name="keyBitConverter"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="PointerNullException"><paramref name="pointerManager"/> does not point to an object.</exception>
//		static public DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> Open<TKey, TDataConstantLength, TDataVariableLength>(PointerManager pointerManager, ConstantLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter, IEqualityComparer<TKey> comparer) where TKey : struct where TDataConstantLength : struct where TDataVariableLength : struct
//		{
//			if (pointerManager == null)
//				throw new ArgumentNullException(nameof(pointerManager));
//			if (dataConstantLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataConstantLengthBitConverter));
//			if (dataVariableLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataVariableLengthBitConverter));
//			DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength>.BitConverter entryConstantLengthBitConverter = DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength>.BitConverter.GetInstance(keyBitConverter, dataConstantLengthBitConverter);
//			DictionaryEntryConstantLengthVariableLength<TDataVariableLength>.BitConverter entryVariableLengthBitConverter = DictionaryEntryConstantLengthVariableLength<TDataVariableLength>.BitConverter.GetInstance(dataVariableLengthBitConverter);
//			ValueManager<DictionaryMeta, EmptyData> metaManager = ValueManagerActivator.Open(pointerManager, DictionaryMeta.BitConverter._instance, EmptyData.VariableLengthBitConverterInstance);
//			ArrayManager<EmptyData, EmptyData> bucketShardManager = ArrayManagerActivator.Open(metaManager[0], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance);
//			ArrayManager<EmptyData, EmptyData> entryShardManager = ArrayManagerActivator.Open(metaManager[1], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance);
//			int shardCount = bucketShardManager.Length;
//			ArrayManager<int, EmptyData>[] bucketShards = new ArrayManager<int, EmptyData>[shardCount];
//			ArrayManager<DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength>, DictionaryEntryConstantLengthVariableLength<TDataVariableLength>>[] entryShards = new ArrayManager<DictionaryEntryConstantLengthConstantLength<TKey, TDataConstantLength>, DictionaryEntryConstantLengthVariableLength<TDataVariableLength>>[shardCount];
//			for (int shardIndex = 0; shardIndex < shardCount; shardIndex++)
//			{
//				PointerManager bucketShardPointer = bucketShardManager[shardIndex][0];
//				if (bucketShardPointer.IsNull)
//					break;
//				bucketShards[shardIndex] = ArrayManagerActivator.Open(bucketShardPointer, Int32BitConverterBuilder.Instance, EmptyData.VariableLengthBitConverterInstance);
//				entryShards[shardIndex] = ArrayManagerActivator.Open(entryShardManager[shardIndex][0], entryConstantLengthBitConverter, entryVariableLengthBitConverter);
//			}
//			return new DictionaryManagerConstantLength<TKey, TDataConstantLength, TDataVariableLength>(metaManager, entryConstantLengthBitConverter, entryVariableLengthBitConverter, entryShards[0].PointerCount, comparer ?? EqualityComparer<TKey>.Default, bucketShards, entryShards);
//		}
//		/// <summary>
//		/// Initializes a new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to manage the existing dictionary in the domain.
//		/// </summary>
//		/// <param name="pointerManager">The <see cref="PointerManager"/> that points to the dictionary.</param>
//		/// <param name="keyBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the keys in the dictionary.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the constant length data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <returns>A new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to manage the existing dictionary in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="pointerManager"/> is null or <paramref name="keyBitConverter"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="PointerNullException"><paramref name="pointerManager"/> does not point to an object.</exception>
//		static public DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> Open<TKey, TDataConstantLength, TDataVariableLength>(PointerManager pointerManager, ConstantLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter) where TKey : struct where TDataConstantLength : struct where TDataVariableLength : struct => Open(pointerManager, keyBitConverter, dataConstantLengthBitConverter, dataVariableLengthBitConverter, null);
//		/// <summary>
//		/// Initializes a new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to manage the existing dictionary in the domain.
//		/// </summary>
//		/// <param name="pointerManager">The <see cref="PointerManager"/> that points to the dictionary.</param>
//		/// <param name="keyBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the keys in the dictionary.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <param name="comparer">The <see cref= "IEqualityComparer{TKey}" /> implementation to use when comparing keys, or null to use the default <see cref="EqualityComparer{TData}" /> for the type of the key.</param>
//		/// <returns>A new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to manage the existing dictionary in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="pointerManager"/> is null or <paramref name="keyBitConverter"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="PointerNullException"><paramref name="pointerManager"/> does not point to an object.</exception>
//		static public DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> Open<TKey, TDataConstantLength, TDataVariableLength>(PointerManager pointerManager, VariableLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter, IEqualityComparer<TKey> comparer) where TDataConstantLength : struct where TDataVariableLength : struct
//		{
//			if (pointerManager == null)
//				throw new ArgumentNullException(nameof(pointerManager));
//			if (dataConstantLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataConstantLengthBitConverter));
//			if (dataVariableLengthBitConverter == null)
//				throw new ArgumentNullException(nameof(dataVariableLengthBitConverter));
//			DictionaryEntryVariableLengthConstantLength<TDataConstantLength>.BitConverter entryConstantLengthBitConverter = DictionaryEntryVariableLengthConstantLength<TDataConstantLength>.BitConverter.GetInstance(dataConstantLengthBitConverter);
//			DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>.BitConverter entryVariableLengthBitConverter = DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>.BitConverter.GetInstance(keyBitConverter, dataVariableLengthBitConverter);
//			ValueManager<DictionaryMeta, EmptyData> metaManager = ValueManagerActivator.Open(pointerManager, DictionaryMeta.BitConverter._instance, EmptyData.VariableLengthBitConverterInstance);
//			ArrayManager<EmptyData, EmptyData> bucketShardManager = ArrayManagerActivator.Open(metaManager[0], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance);
//			ArrayManager<EmptyData, EmptyData> entryShardManager = ArrayManagerActivator.Open(metaManager[1], EmptyData.ConstantLengthBitConverterInstance, EmptyData.VariableLengthBitConverterInstance);
//			int shardCount = bucketShardManager.Length;
//			ArrayManager<int, EmptyData>[] bucketShards = new ArrayManager<int, EmptyData>[shardCount];
//			ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>>[] entryShards = new ArrayManager<DictionaryEntryVariableLengthConstantLength<TDataConstantLength>, DictionaryEntryVariableLengthVariableLength<TKey, TDataVariableLength>>[shardCount];
//			for (int shardIndex = 0; shardIndex < shardCount; shardIndex++)
//			{
//				PointerManager bucketShardPointer = bucketShardManager[shardIndex][0];
//				if (bucketShardPointer.IsNull)
//					break;
//				bucketShards[shardIndex] = ArrayManagerActivator.Open(bucketShardPointer, Int32BitConverterBuilder.Instance, EmptyData.VariableLengthBitConverterInstance);
//				entryShards[shardIndex] = ArrayManagerActivator.Open(entryShardManager[shardIndex][0], entryConstantLengthBitConverter, entryVariableLengthBitConverter);
//			}
//			return new DictionaryManagerVariableLength<TKey, TDataConstantLength, TDataVariableLength>(metaManager, entryConstantLengthBitConverter, entryVariableLengthBitConverter, entryShards[0].PointerCount, comparer ?? EqualityComparer<TKey>.Default, bucketShards, entryShards);
//		}
//		/// <summary>
//		/// Initializes a new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to manage the existing dictionary in the domain.
//		/// </summary>
//		/// <param name="pointerManager">The <see cref="PointerManager"/> that points to the dictionary.</param>
//		/// <param name="keyBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the keys in the dictionary.</param>
//		/// <param name="dataConstantLengthBitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the data of elements.</param>
//		/// <param name="dataVariableLengthBitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the variable length data of elements.</param>
//		/// <returns>A new instance of the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> class to manage the existing dictionary in the domain.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="pointerManager"/> is null or <paramref name="keyBitConverter"/> is null or <paramref name="dataConstantLengthBitConverter"/> is null or <paramref name="dataVariableLengthBitConverter"/> is null.</exception>
//		/// <exception cref="PointerNullException"><paramref name="pointerManager"/> does not point to an object.</exception>
//		static public DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> Open<TKey, TDataConstantLength, TDataVariableLength>(PointerManager pointerManager, VariableLengthBitConverter<TKey> keyBitConverter, ConstantLengthBitConverter<TDataConstantLength> dataConstantLengthBitConverter, VariableLengthBitConverter<TDataVariableLength> dataVariableLengthBitConverter) where TDataConstantLength : struct where TDataVariableLength : struct => Open(pointerManager, keyBitConverter, dataConstantLengthBitConverter, dataVariableLengthBitConverter, null);
//	}
//}