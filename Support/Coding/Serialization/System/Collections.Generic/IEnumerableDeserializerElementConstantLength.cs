using System;
using System.Collections;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal struct IEnumerableDeserializerElementConstantLength<T> : IEnumerator<T>
	{
		private readonly IConstantLengthSerializer<T> _elementSerializer;
		private readonly byte[] _buffer;
		private int _index;
		private int _length;
		private T _current;

		internal IEnumerableDeserializerElementConstantLength(IConstantLengthSerializer<T> elementSerializer, int count, byte[] buffer, int index, out int length)
		{
			_elementSerializer = elementSerializer;
			_buffer = buffer;
			_index = index;
			_length = length = Math.DivRem(count, _elementSerializer.Count, out int rem);
			if (rem != 0x0)
				throw new ArgumentException(string.Format("{0} has invalid value.", nameof(count)));
			_current = default;
		}

		public T Current => _current;
		object IEnumerator.Current => Current;

		public bool MoveNext()
		{
			if (_length == 0x0)
				return false;
			_current = _elementSerializer.Deserialize(_buffer, ref _index);
			_length--;
			return true;
		}
		void IDisposable.Dispose() { }
		void IEnumerator.Reset() => throw new NotSupportedException();
	}
}