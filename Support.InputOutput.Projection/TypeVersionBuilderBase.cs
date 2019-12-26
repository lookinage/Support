using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Noname.IO.ObjectOrientedDatabase
{
	/// <summary>
	/// Represents a base for a builder of a version of database`s reference type.
	/// </summary>
	public abstract class TypeVersionBuilderBase
	{
		private protected readonly DatabaseTypeBuilder _referenceTypeBuilder;
		internal readonly TypeVersionBuilderBase _baseReferenceTypeVersionBuilder;
		internal readonly Type _versionType;
		internal readonly List<PropertyBuilder> _propertyBuilders;
		internal MethodInfo _translateMethodInfo;

		internal TypeVersionBuilderBase(DatabaseTypeBuilder referenceTypeBuilder, TypeVersionBuilderBase baseReferenceTypeVersionBuilder)
		{
			_referenceTypeBuilder = referenceTypeBuilder;
			_baseReferenceTypeVersionBuilder = baseReferenceTypeVersionBuilder;
			_versionType = GetType().GetGenericArguments()[0];
			_propertyBuilders = new List<PropertyBuilder>();
		}

		internal IEnumerable<PropertyDataConstantLengthBuilder> DataConstantLengthBuilders => _propertyBuilders.Where(p => p is PropertyDataConstantLengthBuilder).Select(p => (PropertyDataConstantLengthBuilder)p).Concat(_baseReferenceTypeVersionBuilder != null ? _baseReferenceTypeVersionBuilder.DataConstantLengthBuilders : Enumerable.Empty<PropertyDataConstantLengthBuilder>());
		internal IEnumerable<PropertyDataVariableLengthBuilder> DataVariableLengthBuilders => _propertyBuilders.Where(p => p is PropertyDataVariableLengthBuilder).Select(p => (PropertyDataVariableLengthBuilder)p).Concat(_baseReferenceTypeVersionBuilder != null ? _baseReferenceTypeVersionBuilder.DataVariableLengthBuilders : Enumerable.Empty<PropertyDataVariableLengthBuilder>());
		internal IEnumerable<PropertyDatabaseTypeSingleBuilder> ReferenceSingleBuilders => _propertyBuilders.Where(p => p is PropertyDatabaseTypeSingleBuilder).Select(p => (PropertyDatabaseTypeSingleBuilder)p).Concat(_baseReferenceTypeVersionBuilder != null ? _baseReferenceTypeVersionBuilder.ReferenceSingleBuilders : Enumerable.Empty<PropertyDatabaseTypeSingleBuilder>());
		internal int ReferenceCount => _propertyBuilders.Count(p => p is PropertyDatabaseTypeBuilder);
	}
}