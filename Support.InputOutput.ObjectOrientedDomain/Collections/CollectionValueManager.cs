//using System;
//using Noname.BitConversion.System;

//namespace Noname.IO.ObjectOrientedDomain.Collections
//{
//	/// <summary>
//	/// Represents a manager of a value of an element of a collection in an object-oriented domain.
//	/// </summary>
//	public struct CollectionValueManager : IValueManager
//	{
//		private const int _nextFreeIndexHasItem = -2;

//		private readonly CollectionManager _collectionManager;
//		private readonly int _index;
//		private ValueManager _entryManager;

//		internal CollectionValueManager(CollectionManager collectionManager, int index, ValueManager entryManager)
//		{
//			_collectionManager = collectionManager;
//			_index = index;
//			_entryManager = entryManager;
//		}

//		internal int NextFreeIndex { get => _entryManager.Read(0, Int32BitConverterBuilder.Instance); set => _entryManager.Write(_nextFreeIndexHasItem, Int32BitConverterBuilder.Instance, 0); }
//		/// <summary>
//		/// Gets the <see cref="CollectionManager"/> of the collection of the element.
//		/// </summary>
//		public CollectionManager Collection => _collectionManager;
//		/// <summary>
//		/// Gets the key of the element in the collection.
//		/// </summary>
//		public int Key => _index;
//		/// <summary>
//		/// Gets the value meaning whether the element contains an item.
//		/// </summary>
//		public bool HasItem { get => NextFreeIndex == _nextFreeIndexHasItem; internal set => NextFreeIndex = _nextFreeIndexHasItem; }
//		/// <summary>
//		/// Gets or sets a <see cref="PointerManager"/> at the specified index.
//		/// </summary>
//		/// <param name="pointerIndex">The zero-based index of the <see cref="PointerManager"/> to get or set.</param>
//		/// <returns>The <see cref="PointerManager"/> at the specified index.</returns>
//		/// <exception cref="InvalidOperationException">The element does not contain an item.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="pointerIndex"/> is not a valid index of the pointers of the element.</exception>
//		/// <exception cref="ArgumentNullException">The specified <see cref="PointerManager"/> is null.</exception>
//		/// <exception cref="DomainMismatchException">The specified <see cref="PointerManager"/> is of another <see cref="DomainManager"/>.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public PointerManager this[int pointerIndex]
//		{
//			get
//			{
//				Check();
//				return _entryManager[pointerIndex];
//			}
//			set
//			{
//				Check();
//				_collectionManager.IncreaseVersion();
//				_entryManager[pointerIndex] = value;
//			}
//		}

//		private void Check()
//		{
//			if (!HasItem)
//				throw new InvalidOperationException("The element does not contain an item.");
//			if (_collectionManager.CheckEntryShard(_index, _entryManager._arrayManager))
//				return;
//			_entryManager = _collectionManager.GetEntryManager(_index);
//		}
//		/// <summary>
//		/// Determines whether the specified <see cref="CollectionValueManager"/> and the current <see cref="CollectionValueManager"/> are of the same element.
//		/// </summary>
//		/// <param name="obj">The <see cref="CollectionValueManager"/> to compare with the current <see cref="CollectionValueManager"/>.</param>
//		/// <returns>true if the specified <see cref="CollectionValueManager"/> and the current <see cref="CollectionValueManager"/> are of the same element; otherwise, false.</returns>
//		public override sealed bool Equals(object obj) => obj is CollectionValueManager collectionElementManager ? collectionElementManager._collectionManager == _collectionManager && collectionElementManager._index == _index : false;
//		/// <summary>
//		/// Serves as the hash function.
//		/// </summary>
//		/// <returns>A hash code of the current <see cref="CollectionValueManager"/>.</returns>
//		public override sealed int GetHashCode() => _collectionManager.GetHashCode() * _index;
//		/// <summary>
//		/// Gets or sets the constant length data of the element.
//		/// </summary>
//		/// <exception cref="InvalidOperationException">The element does not contain an item.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public TDataConstantLength DataConstantLength
//		{
//			get
//			{
//				Check();
//				return _entryManager.DataConstantLength._dataConstantLength;
//			}
//			set
//			{
//				Check();
//				_collectionManager.IncreaseVersion();
//				CollectionEntryConstantLength<TDataConstantLength> entryConstantLength = _entryManager.DataConstantLength;
//				entryConstantLength._dataConstantLength = value;
//				_entryManager.DataConstantLength = entryConstantLength;
//			}
//		}
//		/// <summary>
//		/// Gets or sets the variable length data of the element.
//		/// </summary>
//		/// <exception cref="InvalidOperationException">The element does not contain an item.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public TDataVariableLength DataVariableLength
//		{
//			get
//			{
//				Check();
//				return _entryManager.DataVariableLength._dataVariableLength;
//			}
//			set
//			{
//				Check();
//				_collectionManager.IncreaseVersion();
//				CollectionEntryVariableLength<TDataVariableLength> entryVariableLength = _entryManager.DataVariableLength;
//				entryVariableLength._dataVariableLength = value;
//				_entryManager.DataVariableLength = entryVariableLength;
//			}
//		}
//	}
//}