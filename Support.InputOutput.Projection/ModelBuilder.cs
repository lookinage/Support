using System;
using System.Collections.Generic;

namespace Noname.IO.ObjectOrientedDatabase
{
	/// <summary>
	/// Represents a builder of a model of an object-oriented database.
	/// </summary>
	public sealed class ModelBuilder
	{
		internal readonly List<DatabaseTypeBuilder> _typeBuilders;

		/// <summary>
		/// Initializes a new instance of the <see cref="ModelBuilder"/> class.
		/// </summary>
		public ModelBuilder() => _typeBuilders = new List<DatabaseTypeBuilder>();

		/// <summary>
		/// Defines a reference type.
		/// </summary>
		/// <typeparam name="T">The type representing the first version of the reference type.</typeparam>
		/// <param name="name">The name of the entity type.</param>
		/// <returns>A <see cref="TypeVersionBuilder{T}"/> to build the first version of the reference type.</returns>
		/// <exception cref="ArgumentException">The specified type is not public and abstract or another type with the specified name already exists.</exception>
		public TypeVersionBuilder<T> DefineReferenceType<T>(string name) where T : class => DefineReferenceType<T, object>(name, null);
		/// <summary>
		/// Defines a reference type.
		/// </summary>
		/// <typeparam name="T">The type representing the first version of the reference type.</typeparam>
		/// <typeparam name="TBase">The type representing the base type of the specified type.</typeparam>
		/// <param name="name">The name of the entity type.</param>
		/// <param name="baseReferenceTypeVersionBuilder">The <see cref="TypeVersionBuilder{T}"/> of the base type of the specified type.</param>
		/// <returns>A <see cref="TypeVersionBuilder{T}"/> to build the first version of the reference type.</returns>
		/// <exception cref="ArgumentException">The specified type is not public and abstract or another type with the specified name already exists.</exception>
		public TypeVersionBuilder<T> DefineReferenceType<T, TBase>(string name, TypeVersionBuilder<TBase> baseReferenceTypeVersionBuilder) where T : class, TBase where TBase : class
		{
			foreach (DatabaseTypeBuilder checkingTypeBuilder in _typeBuilders)
				if (name == checkingTypeBuilder._name)
					throw new ArgumentException("Another type with the specified name already exists.");
			DatabaseTypeBuilder typeBuilder = new DatabaseTypeBuilder(this, name);
			_typeBuilders.Add(typeBuilder);
			return typeBuilder.DefineNextVersion<T>(baseReferenceTypeVersionBuilder);
		}
	}
}