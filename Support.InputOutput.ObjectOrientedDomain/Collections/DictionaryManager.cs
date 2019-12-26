//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Noname.BitConversion;

//namespace Noname.IO.ObjectOrientedDomain.Collections
//{
//	/// <summary>
//	/// Represents a manager of a collection of key/value pairs that is in a domain.
//	/// </summary>
//	/// <typeparam name="TKey">The type of keys.</typeparam>
//	/// <typeparam name="TDataConstantLength">The type of the data that converts by <see cref="ConstantLengthBitConverter{T}"/>.</typeparam>
//	/// <typeparam name="TDataVariableLength">The type of the data that converts by <see cref="VariableLengthBitConverter{T}"/>.</typeparam>
//	public abstract class DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> : ICollection<DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength>> where TDataConstantLength : struct where TDataVariableLength : struct
//	{
//		private protected readonly ValueManager<DictionaryMeta, EmptyData> _metaManager;

//		private protected DictionaryManager(ValueManager<DictionaryMeta, EmptyData> metaManager) => _metaManager = metaManager;

//		bool ICollection<DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength>>.IsReadOnly => false;

//		/// <summary>
//		/// Gets the <see cref="PointerManager"/> of the pointer that points to the dictionary.
//		/// </summary>
//		public PointerManager Pointer => _metaManager.Pointer;
//		/// <summary>
//		/// Gets the number of pointers of elements.
//		/// </summary>
//		public abstract int PointerCount { get; }
//		/// <summary>
//		/// Gets the number of elements contained in the dictionary.
//		/// </summary>
//		public int Count => _metaManager.DataConstantLength._count;
//		/// <summary>
//		/// Gets a <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> of an item with the specified key.
//		/// </summary>
//		/// <param name="key">The key of the item to get.</param>
//		/// <returns>A <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> of an item with the specified key.</returns>
//		/// <exception cref="ArgumentException">The dictionary does not contain an item with the specified key.</exception>
//		public abstract DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> this[TKey key] { get; }

//		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//		void ICollection<DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength>>.Add(DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> item) => throw new NotSupportedException();
//		bool ICollection<DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength>>.Remove(DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> item) => throw new NotSupportedException();
//		bool ICollection<DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength>>.Contains(DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> item) => throw new NotSupportedException();

//		internal abstract void IncreaseVersion();
//		/// <summary>
//		/// Returns an enumerator that iterates through the dictionary.
//		/// </summary>
//		/// <returns><see cref = "IEnumerator{TData}" /> that can be used to iterate through the dictionary.</returns>
//		public abstract IEnumerator<DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength>> GetEnumerator();
//		/// <summary>
//		/// Determines whether the dictionary contains an item with the specified key.
//		/// </summary>
//		/// <param name="key">The key to locate in the dictionary.</param>
//		/// <returns>true if the dictionary contains an element with the key; otherwise, false.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
//		public abstract bool ContainsKey(TKey key);
//		/// <summary>
//		/// Copies the elements of the dictionary to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
//		/// </summary>
//		/// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from the dictionary. The <see cref="Array"/> must have zero-based indexing.</param>
//		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is not a valid index of <paramref name="array"/>.</exception>
//		/// <exception cref="ArgumentException">The number of elements in the dictionary is greater than the available space from the <paramref name="arrayIndex"/> to the end of the array.</exception>
//		public abstract void CopyTo(DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength>[] array, int arrayIndex);
//		/// <summary>
//		/// Gets the <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> of the element associated with the specified key.
//		/// </summary>
//		/// <param name="key">The key whose <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> to get.</param>
//		/// <param name="dictionaryElementManager">The <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> of the element associated with the specified key.</param>
//		/// <returns>true if the dictionary contains an element with the specified key; otherwise, false.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
//		public abstract bool TryGetValue(TKey key, out DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> dictionaryElementManager);
//		/// <summary>
//		/// Adds an item into the dictionary.
//		/// </summary>
//		/// <param name="key">The key of the item to add.</param>
//		/// <param name="dataConstantLength">The <typeparamref name="TDataConstantLength"/> of the item to add.</param>
//		/// <param name="dataVariableLength">The <typeparamref name="TDataVariableLength"/> of the item to add.</param>
//		/// <param name="pointers">The pointers of the item to add.</param>
//		/// <returns>A <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> of the element of the added item.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="key"/> is null or <paramref name="pointers"/> is null.</exception>
//		/// <exception cref="ArgumentException">The dictionary already contains an item with the specified key or the number of the specified pointers is not equal to the number of pointers of an element of the collection.</exception>
//		public abstract DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> Add(TKey key, TDataConstantLength dataConstantLength, TDataVariableLength dataVariableLength, params PointerManager[] pointers);
//		/// <summary>
//		/// Removes the item with the specified key from the dictionary.
//		/// </summary>
//		/// <param name="key">The key of the item to remove.</param>
//		/// <returns>true if item was found and removed from the dictionary; otherwise, false.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
//		public abstract bool Remove(TKey key);
//		/// <summary>
//		/// Removes all items from the dictionary.
//		/// </summary>
//		public abstract void Clear();
//	}
//}