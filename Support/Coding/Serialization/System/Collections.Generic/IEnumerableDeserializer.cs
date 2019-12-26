using System;
using System.Collections;
using System.Collections.Generic;

namespace Support.Coding.Serialization.System.Collections.Generic
{
	internal struct IEnumerableDeserializer<T> : IEnumerator<T>
	{
		private readonly ISerializer<T> _elementSerializer;
		private readonly int _count;
		private readonly byte[] _buffer;
		private readonly int _startIndex;
		private int _index;
		private int _length;
		private T _current;

		internal IEnumerableDeserializer(ISerializer<T> elementSerializer, int count, byte[] buffer, int index, out int length)
		{
			_elementSerializer = elementSerializer;
			_count = count;
			_buffer = buffer;
			_index = _startIndex = index;
			if ((_length = length = Int32SerializerBuilder.Default.Deserialize(buffer, ref _index)) < 0x0)
				throw new ArgumentException("The deserialized number of elements is less than 0.");
			_current = default;
		}

		public T Current => _current;
		object IEnumerator.Current => Current;

		public bool MoveNext()
		{
			if (_length == 0x0)
				return false;
			_current = _elementSerializer.Deserialize(_length != 0x1 ? Int32SerializerBuilder.Default.Deserialize(_buffer, ref _index) : _count - _index + _startIndex, _buffer, ref _index);
			_length--;
			return true;
		}
		void IDisposable.Dispose() { }
		void IEnumerator.Reset() => throw new NotSupportedException();
	}
}