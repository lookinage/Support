using System;
using System.Collections.Generic;

namespace Noname.IO.ObjectOrientedDatabase
{
	internal sealed class DatabaseTypeBuilder
	{
		internal readonly ModelBuilder _modelBuilder;
		internal readonly string _name;
		internal readonly List<TypeVersionBuilderBase> _versionBuilders;

		internal DatabaseTypeBuilder(ModelBuilder modelBuilder, string name)
		{
			_modelBuilder = modelBuilder;
			_name = name;
			_versionBuilders = new List<TypeVersionBuilderBase>();
		}

		internal TypeVersionBuilderBase LastVersionBuilder => _versionBuilders[_versionBuilders.Count - 1];

		internal TypeVersionBuilder<T> DefineNextVersion<T>(TypeVersionBuilderBase baseReferenceTypeVersionBuilder) where T : class
		{
			Type type = typeof(T);
			if (!type.IsPublic && type.IsNotPublic)
				throw new ArgumentException("The specified type is not public.");
			if (!type.IsAbstract)
				throw new ArgumentException("The specified type is not abstract.");
			TypeVersionBuilder<T> versionBuilder = new TypeVersionBuilder<T>(this, baseReferenceTypeVersionBuilder);
			_versionBuilders.Add(versionBuilder);
			return versionBuilder;
		}
	}
}