using System;
using System.Collections.Generic;
using System.Text;

namespace Support.Coding.Serialization.System
{
	/// <summary>
	/// Represents a builder of <see cref="ISerializer{T}"/> instances of the <see cref="string"/> type.
	/// </summary>
	static public class StringSerializerBuilder
	{
		static private readonly Dictionary<Encoding, StringSerializer> _serializers;
		/// <summary>
		/// The serializer of the <see cref="string"/> type. The <see cref="Serializer{T}"/> uses <see cref="Encoding.UTF8"/> encoding.
		/// </summary>
		static public readonly ISerializer<string> UTF8;
		/// <summary>
		/// The serializer of the <see cref="string"/> type. The <see cref="Serializer{T}"/> uses <see cref="Encoding.UTF7"/> encoding.
		/// </summary>
		static public readonly ISerializer<string> UTF7;
		/// <summary>
		/// The serializer of the <see cref="string"/> type. The <see cref="Serializer{T}"/> uses <see cref="Encoding.UTF32"/> encoding.
		/// </summary>
		static public readonly ISerializer<string> UTF32;
		/// <summary>
		/// The serializer of the <see cref="string"/> type. The <see cref="Serializer{T}"/> uses <see cref="Encoding.Unicode"/> encoding.
		/// </summary>
		static public readonly ISerializer<string> Unicode;
		/// <summary>
		/// The serializer of the <see cref="string"/> type. The <see cref="Serializer{T}"/> uses <see cref="Encoding.BigEndianUnicode"/> encoding.
		/// </summary>
		static public readonly ISerializer<string> BigEndianUnicode;
		/// <summary>
		/// The serializer of the <see cref="string"/> type. The <see cref="Serializer{T}"/> uses <see cref="Encoding.ASCII"/> encoding.
		/// </summary>
		static public readonly ISerializer<string> ASCII;
		/// <summary>
		/// The serializer of the <see cref="string"/> type. The <see cref="Serializer{T}"/> uses <see cref="Encoding.Default"/> encoding.
		/// </summary>
		static public readonly ISerializer<string> Default;

		static StringSerializerBuilder()
		{
			_serializers = new Dictionary<Encoding, StringSerializer>();
			UTF8 = CreateSerializer(Encoding.UTF8);
			UTF7 = CreateSerializer(Encoding.UTF7);
			UTF32 = CreateSerializer(Encoding.UTF32);
			Unicode = CreateSerializer(Encoding.Unicode);
			BigEndianUnicode = CreateSerializer(Encoding.BigEndianUnicode);
			ASCII = CreateSerializer(Encoding.ASCII);
			Default = CreateSerializer(Encoding.Default);
		}

		/// <summary>
		/// Builds an <see cref="ISerializer{T}"/> of the <see cref="string"/> type.
		/// </summary>
		/// <param name="encoding">An <see cref="Encoding"/> to code strings.</param>
		/// <returns>An <see cref="ISerializer{T}"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="encoding"/> is <see langword="null"/>.</exception>
		static public ISerializer<string> CreateSerializer(Encoding encoding)
		{
			if (encoding == null)
				throw new ArgumentNullException(nameof(encoding));
			StringSerializer serializer;
			lock (_serializers)
			{
				if (_serializers.TryGetValue(encoding, out serializer))
					return serializer;
				_serializers.Add(encoding, serializer = new StringSerializer(encoding));
			}
			return serializer;
		}
	}
}