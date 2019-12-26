//using System;
//using Noname.IO.ObjectOrientedDomain;

//namespace Noname.IO.ObjectOrientedDatabase
//{
//	/// <summary>
//	/// Reprsents a manager of an object-oriented database.
//	/// </summary>
//	/// <typeparam name="TStart">The type of the last version of the reference type of the start object.</typeparam>
//	public sealed class DatabaseManager<TStart> : IDisposable where TStart : class
//	{
//		private readonly ModelCompiler _modelCompiler;
//		private readonly DomainManager _domainManager;
//		/// <summary>
//		/// The start object of the database.
//		/// </summary>
//		public readonly TStart StartObject;

//		/// <summary>
//		/// Initializes a new instance of the <see cref="DatabaseManager{T}"/> class with default values.
//		/// </summary>
//		/// <param name="modelBuilder">The builder of the database model.</param>
//		/// <param name="path">The path to the database.</param>
//		/// <param name="create">If true the database creates; otherwise, the <see cref="DatabaseManager{T}"/> opens the database.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
//		public DatabaseManager(ModelBuilder modelBuilder, string path, bool create) : this(modelBuilder, path, create, StorageManager.DefaultMaxReadByteCount, StorageManager.DefaultMaxChangeBufferSize, StorageManager.DefaultMaxCachePieceByteCount, StorageManager.DefaultCachePieceCountFlushBound, StorageManager.DefaultCacheByteCountFlushBound, DomainManager.DefaultFreeSegmentCountDefragmentBound) { }
//		/// <summary>
//		/// Initializes a new instance of the <see cref="DatabaseManager{T}"/> class.
//		/// </summary>
//		/// <param name="modelBuilder">The builder of the database model.</param>
//		/// <param name="path">The path to the database.</param>
//		/// <param name="create">If true the database creates; otherwise, the <see cref="DatabaseManager{T}"/> opens the database.</param>
//		/// <param name="maxReadByteCount">The max number of bytes to read at a time.</param>
//		/// <param name="maxChangeBufferSize">The max size of the change buffer.</param>
//		/// <param name="maxCachePieceByteCount">The max number of bytes in a piece of the cache.</param>
//		/// <param name="cachePieceCountFlushBound">The bound of number of pieces of the cache for the auto cache flush.</param>
//		/// <param name="cacheByteCountFlushBound">The bound of number of bytes of the cache for the auto cache flush.</param>
//		/// <param name="freeSegmentCountDefragmentBound">The bound of number of free segments of the heap for the auto defragmentation.</param>
//		/// <exception cref="InvalidOperationException">The specified type is not a type of the last version of any reference type or the type of a reference property does not use a type of the last version of any reference type.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="maxCachePieceByteCount"/> is less than 1 or <paramref name="maxReadByteCount"/> is less than 2 or <paramref name="maxChangeBufferSize"/> is less than 12 or <paramref name="cachePieceCountFlushBound"/> is less than 1 or <paramref name="cacheByteCountFlushBound"/> is less than 1 or <paramref name="descriptorCountGarbageCollectBound"/> is less than 1 or <paramref name="freeSegmentCountDefragmentBound"/> is less than 1.</exception>
//		/// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
//		public DatabaseManager(ModelBuilder modelBuilder, string path, bool create, int maxReadByteCount, int maxChangeBufferSize, int maxCachePieceByteCount, int cachePieceCountFlushBound, long cacheByteCountFlushBound, int freeSegmentCountDefragmentBound)
//		{
//			Type _startType = typeof(TStart);
//			bool typeIsValid = false;
//			foreach (DatabaseTypeBuilder referenceTypeBuilder in modelBuilder._typeBuilders)
//			{
//				if (_startType != referenceTypeBuilder.LastVersionBuilder._versionType)
//					continue;
//				typeIsValid = true;
//				break;
//			}
//			if (!typeIsValid)
//				throw new InvalidOperationException(string.Format("{0} is not a type of a version of any reference type.", nameof(TStart)));
//			foreach (DatabaseTypeBuilder checkingReferenceTypeBuilder in modelBuilder._typeBuilders)
//				foreach (TypeVersionBuilderBase versionBuilder in checkingReferenceTypeBuilder._versionBuilders)
//					foreach (PropertyDatabaseTypeSingleBuilder singleReference in versionBuilder.ReferenceSingleBuilders)
//					{
//						Type singleReferenceType = singleReference._propertyInfo.PropertyType;
//						typeIsValid = false;
//						foreach (DatabaseTypeBuilder referenceTypeBuilder in modelBuilder._typeBuilders)
//						{
//							if (singleReferenceType != referenceTypeBuilder.LastVersionBuilder._versionType)
//								continue;
//							typeIsValid = true;
//							break;
//						}
//						if (!typeIsValid)
//							throw new InvalidOperationException(string.Format("The type of {0} property of {1} does not use a type of the last version of any reference type.", singleReference._propertyInfo.Name, singleReference._propertyInfo.DeclaringType.FullName));
//					}
//			_modelCompiler = new ModelCompiler(modelBuilder);
//			_domainManager = new DomainManager(path, create, maxReadByteCount, maxChangeBufferSize, maxCachePieceByteCount, cachePieceCountFlushBound, cacheByteCountFlushBound, freeSegmentCountDefragmentBound);
//			foreach (DatabaseTypeBuilder referenceTypeBuilder in modelBuilder._typeBuilders)
//			{
//				if (_startType != referenceTypeBuilder.LastVersionBuilder._versionType)
//					continue;
//				typeIsValid = true;
//				break;
//			}
//			if (!create)
//			{
//				StartObject = (TStart)_modelCompiler.OpenSingle(_startType, _domainManager.StartPointer);
//				return;
//			}
//			StartObject = (TStart)_modelCompiler.CreateSingle(_startType, _domainManager, out PointerManager pointerManager);
//			_domainManager.StartPointer = pointerManager;
//		}

//		/// <summary>
//		/// Creates an instance of the specified reference type.
//		/// </summary>
//		/// <typeparam name="T">The type of the last version of a reference type.</typeparam>
//		public T Create<T>() => (T)_modelCompiler.CreateSingle(typeof(T), _domainManager, out PointerManager pointerManager);
//		/// <summary>
//		/// Commits the made changes.
//		/// </summary>
//		public void Commit() => _domainManager.Commit();
//		/// <summary>
//		/// Releases all resources used by the <see cref="DatabaseManager{T}"/>.
//		/// </summary>
//		/// <exception cref="InvalidOperationException">The non-committed changes exist.</exception>
//		public void Dispose() => _domainManager.Dispose();
//	}
//}