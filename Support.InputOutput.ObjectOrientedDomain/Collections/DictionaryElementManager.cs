//using System;
//using Noname.BitConversion;

//namespace Noname.IO.ObjectOrientedDomain.Collections
//{
//	/// <summary>
//	/// Represents a manager of an element of a dictionary in a domain. The element references to objects and has the data.
//	/// </summary>
//	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
//	/// <typeparam name="TDataConstantLength">The type of the data that converts by <see cref="ConstantLengthBitConverter{T}"/>.</typeparam>
//	/// <typeparam name="TDataVariableLength">The type of the data that converts by <see cref="VariableLengthBitConverter{T}"/>.</typeparam>
//	public abstract class DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> : IValueManager where TDataConstantLength : struct where TDataVariableLength : struct
//	{
//		/// <summary>
//		/// Determines whether two specified <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> instances are of the same element.
//		/// </summary>
//		/// <param name="a">The first <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> to compare.</param>
//		/// <param name="b">The second <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> to compare.</param>
//		/// <returns>true if the element of <paramref name="a"/> is the same as the element of <paramref name="b"/>; otherwise, false.</returns>
//		static public bool operator ==(DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> a, DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> b) => a is null ? b is null : b is null ? false : a._dictionaryManager == b._dictionaryManager && a._index == b._index;
//		/// <summary>
//		/// Determines whether two specified <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> instances are of different elements.
//		/// </summary>
//		/// <param name="a">The first <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> to compare.</param>
//		/// <param name="b">The second <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> to compare.</param>
//		/// <returns>true if the element of <paramref name="a"/> differs from the element of <paramref name="b"/>; otherwise, false.</returns>
//		static public bool operator !=(DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> a, DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> b) => !(a == b);

//		private protected readonly int _index;
//		private readonly DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> _dictionaryManager;

//		internal DictionaryElementManager(int index, DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> dictionaryManager)
//		{
//			_index = index;
//			_dictionaryManager = dictionaryManager;
//		}

//		/// <summary>
//		/// Gets or sets the variable length data.
//		/// </summary>
//		/// <exception cref="InvalidOperationException">The element does not contain an item.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public abstract TDataVariableLength DataVariableLength { get; set; }
//		/// <summary>
//		/// Gets or sets the constant length data.
//		/// </summary>
//		/// <exception cref="InvalidOperationException">The element does not contain an item.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		public abstract TDataConstantLength DataConstantLength { get; set; }
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
//		public abstract PointerManager this[int pointerIndex] { get; set; }
//		/// <summary>
//		/// Gets the key of the element in the dictionary.
//		/// </summary>
//		public abstract TKey Key { get; }
//		/// <summary>
//		/// Gets the value meaning whether the element contains an item.
//		/// </summary>
//		public abstract bool HasItem { get; }
//		/// <summary>
//		/// Gets the <see cref="DictionaryManager{TKey, TDataConstantLength, TDataVariableLength}"/> of the dictionary of the element.
//		/// </summary>
//		public DictionaryManager<TKey, TDataConstantLength, TDataVariableLength> Dictionary => _dictionaryManager;

//		/// <summary>
//		/// Determines whether the specified <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> and the current <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> are of the same element.
//		/// </summary>
//		/// <param name="obj">The <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> to compare with the current <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/>.</param>
//		/// <returns>true if the specified <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> and the current <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/> are of the same element; otherwise, false.</returns>
//		public override sealed bool Equals(object obj) => obj is DictionaryElementManager<TKey, TDataConstantLength, TDataVariableLength> dictionaryElementManager ? dictionaryElementManager._dictionaryManager == _dictionaryManager && dictionaryElementManager._index == _index : false;
//		/// <summary>
//		/// Serves as the hash function.
//		/// </summary>
//		/// <returns>A hash code of the current <see cref="DictionaryElementManager{TKey, TDataConstantLength, TDataVariableLength}"/>.</returns>
//		public override sealed int GetHashCode() => _dictionaryManager.GetHashCode() * _index;
//	}
//}