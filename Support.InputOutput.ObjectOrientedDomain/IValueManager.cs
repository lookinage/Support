//using System;
//using Noname.BitConversion;

//namespace Noname.IO.ObjectOrientedDomain
//{
//	/// <summary>
//	/// Represents a manager of a value that is in a domain.
//	/// </summary>
//	public interface IValueManager
//	{
//		/// <summary>
//		/// Gets or sets a reference to an array at the specified index.
//		/// </summary>
//		/// <param name="index">The zero-based index of the <see cref="ArrayManager"/> of the array to get or set.</param>
//		/// <returns>The <see cref="ArrayManager"/> of the array at the specified index.</returns>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index of the pointers of the element.</exception>
//		/// <exception cref="ArgumentNullException">The specified <see cref="ArrayManager"/> is null.</exception>
//		/// <exception cref="ArgumentException">The array is lost or the specified <see cref="ArrayManager"/> is of another <see cref="DomainManager"/> or the array of the specified <see cref="ArrayManager"/> is lost.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		ArrayManager this[int index] { get; set; }
//		/// <summary>
//		/// Writes bytes to the data of the value.
//		/// </summary>
//		/// <param name="buffer">The buffer to contain the bytes.</param>
//		/// <param name="index">The index at which the writing from the buffer begins.</param>
//		/// <param name="count">The number of the bytes.</param>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be written.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/> or <paramref name="count"/> is less than 0 or <paramref name="offset"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The array is lost or <paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/> or <paramref name="count"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		void Write(byte[] buffer, int index, int count, int offset);
//		/// <summary>
//		/// Converts the specified value to bytes and writes the bytes to the data of the value.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="value">The value.</param>
//		/// <param name="bitConverter">The <see cref="ConstantLengthBitConverter{T}"/> to convert the value to the bytes.</param>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be written.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="bitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The array is lost or the number of bytes of <paramref name="value"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		void Write<T>(T value, ConstantLengthBitConverter<T> bitConverter, int offset);
//		/// <summary>
//		/// Converts the specified value to bytes and writes the bytes to the data of the value.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="value">The value.</param>
//		/// <param name="bitConverter">The <see cref="VariableLengthBitConverter{T}"/> to convert the value to the bytes.</param>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be written.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="bitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The array is lost or the number of bytes of <paramref name="value"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		void Write<T>(T value, VariableLengthBitConverter<T> bitConverter, int offset);
//		/// <summary>
//		/// Reads bytes from the data of the value.
//		/// </summary>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be read.</param>
//		/// <param name="count">The number of the bytes.</param>
//		/// <param name="buffer">The buffer to receive the bytes.</param>
//		/// <param name="index">The index at which the reading to the buffer begins.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is outside the range of valid indices of the <paramref name="buffer"/> or <paramref name="count"/> is less than 0 or <paramref name="offset"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The array is lost or <paramref name="count"/> is greater than the number of bytes from <paramref name="index"/> to the end of <paramref name="buffer"/> or <paramref name="count"/> is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		void Read(int offset, int count, byte[] buffer, int index);
//		/// <summary>
//		/// Reads bytes from the data of the value and converts it to a value.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be read.</param>
//		/// <param name="bitConverter">The <see cref="ConstantLengthBitConverter{T}"/> to convert read bytes to the value.</param>
//		/// <returns>A read value.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="bitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The number of bytes of the value is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		T Read<T>(int offset, ConstantLengthBitConverter<T> bitConverter);
//		/// <summary>
//		/// Reads bytes from the data of the value and converts it to a value.
//		/// </summary>
//		/// <typeparam name="T">The type of the value.</typeparam>
//		/// <param name="offset">The zero-based offset in the data at which the bytes are to be read.</param>
//		/// <param name="count">The number of the bytes.</param>
//		/// <param name="bitConverter">The <see cref="ConstantLengthBitConverter{T}"/> to convert read bytes to the value.</param>
//		/// <returns>A read value.</returns>
//		/// <exception cref="ArgumentNullException"><paramref name="bitConverter"/> is null.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is less than 0 or greater than the size of the data or <paramref name="count"/> is less than 0 or greater than the size of the data.</exception>
//		/// <exception cref="ArgumentException">The number of bytes of the value is greater than the number of bytes from <paramref name="offset"/> to the end of the data.</exception>
//		/// <exception cref="ObjectDisposedException">The <see cref="DomainManager"/> is disposed.</exception>
//		T Read<T>(int offset, int count, VariableLengthBitConverter<T> bitConverter);
//	}
//}