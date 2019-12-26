using System;
using System.Linq.Expressions;
using System.Reflection;
using Noname.BitConversion;

namespace Noname.IO.ObjectOrientedDatabase
{
	/// <summary>
	/// Represents a builder of a version of database`s reference type.
	/// </summary>
	/// <typeparam name="T">The type representing a version of database`s reference type.</typeparam>
	public sealed class TypeVersionBuilder<T> : TypeVersionBuilderBase where T : class
	{
		static private readonly Type _type;

		static TypeVersionBuilder() => _type = typeof(T);

		private bool _continued;

		internal TypeVersionBuilder(DatabaseTypeBuilder referenceTypeBuilder, TypeVersionBuilderBase baseReferenceTypeVersionBuilder) : base(referenceTypeBuilder, baseReferenceTypeVersionBuilder) { }

		private void CheckPropertyInfo(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
				throw new ArgumentNullException(nameof(propertyInfo));
			Type declaringType = propertyInfo.DeclaringType;
			for (Type baseType = _type; baseType != null; baseType = baseType.BaseType)
			{
				if (declaringType == baseType)
					continue;
				declaringType = null;
				break;
			}
			if (declaringType != null)
				throw new ArgumentException("The property is not a property of the specified type.");
			MethodInfo getMethod = propertyInfo.GetGetMethod();
			MethodInfo setMethod = propertyInfo.GetSetMethod();
			if (getMethod.IsStatic || setMethod.IsStatic)
				throw new ArgumentException("The property is static.");
			if (getMethod == null || !getMethod.IsPublic || !getMethod.IsAbstract || setMethod == null || !setMethod.IsPublic || !setMethod.IsAbstract)
				throw new ArgumentException("The property does not access to an abstract property of the type with public get and set accessor.");
			foreach (PropertyBuilder propertyBuilder in _propertyBuilders)
				if (propertyBuilder._propertyInfo == propertyInfo)
					throw new ArgumentException("The type already has the property.");
		}
		private PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
		{
			if (propertyExpression == null)
				throw new ArgumentNullException(nameof(propertyExpression));
			if (!(propertyExpression.Body is MemberExpression memberExpression) || !(memberExpression.Member is PropertyInfo propertyInfo))
				throw new ArgumentException(string.Format("{0} does not access to a property.", propertyExpression));
			CheckPropertyInfo(propertyInfo);
			return propertyInfo;
		}
		/// <summary>
		/// Defines data of the database`s reference type.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="propertyExpression">The expression to define the data property. Example: t => t.Property.</param>
		/// <param name="bitConverter">The <see cref="ConstantLengthBitConverter{T}"/> of the type of the property.</param>
		/// <exception cref="ArgumentNullException"><paramref name="propertyExpression"/> is null or <paramref name="bitConverter"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="propertyExpression"/> does not access to an abstract property of the type with public get and set accessors or the type already has the property.</exception>
		public void DefineData<TProperty>(Expression<Func<T, TProperty>> propertyExpression, ConstantLengthBitConverter<TProperty> bitConverter) => _propertyBuilders.Add(new PropertyDataConstantLengthBuilder(GetPropertyInfo(propertyExpression), bitConverter ?? throw new ArgumentNullException(nameof(bitConverter))));
		/// <summary>
		/// Defines data of the database`s reference type.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="propertyExpression">The expression to define the data property. Example: t => t.Property.</param>
		/// <param name="bitConverter">The <see cref="VariableLengthBitConverter{T}"/> of the type of the property.</param>
		/// <exception cref="ArgumentNullException"><paramref name="propertyExpression"/> is null or <paramref name="bitConverter"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="propertyExpression"/> does not access to an abstract property of the type with public get and set accessors or the type already has the property.</exception>
		public void DefineData<TProperty>(Expression<Func<T, TProperty>> propertyExpression, VariableLengthBitConverter<TProperty> bitConverter) => _propertyBuilders.Add(new PropertyDataVariableLengthBuilder(GetPropertyInfo(propertyExpression), bitConverter ?? throw new ArgumentNullException(nameof(bitConverter))));
		/// <summary>
		/// Defines data of the database`s reference type.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="propertyExpression">The expression to define the data property. Example: t => t.Property.</param>
		/// <exception cref="InvalidOperationException">Faild to build a bit converter for the type of the property.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="propertyExpression"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="propertyExpression"/> does not access to an abstract property of the type with public get and set accessors or the type already has the property.</exception>
		public void DefineData<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
		{
			ConstantLengthBitConverter<TProperty> bitConverterConstantLength = ConstantLengthBitConverterBuilder<TProperty>.Default;
			if (bitConverterConstantLength != null)
			{
				DefineData(propertyExpression, bitConverterConstantLength);
				return;
			}
			DefineData(propertyExpression, VariableLengthBitConverterBuilder<TProperty>.Default ?? throw new InvalidOperationException("Faild to build a bit converter for the type of the property."));
		}
		/// <summary>
		/// Defines reference to a single object.
		/// </summary>
		/// <typeparam name="TProperty">The type of the final version of the database`s reference type.</typeparam>
		/// <param name="propertyExpression">The expression to define the reference property. Example: t => t.Property.</param>
		/// <exception cref="ArgumentNullException"><paramref name="propertyExpression"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="propertyExpression"/> does not access to an abstract property of the type with public get and set accessors or the type already has the property.</exception>
		public void DefineReference<TProperty>(Expression<Func<T, TProperty>> propertyExpression) => _propertyBuilders.Add(new PropertyDatabaseTypeSingleBuilder(GetPropertyInfo(propertyExpression)));
		/// <summary>
		/// Defines the next version of the reference type.
		/// </summary>
		/// <typeparam name="TNext">The type representing the next version of the database`s reference type.</typeparam>
		/// <returns>A <see cref="TypeVersionBuilder{T}"/> to build the next version of the reference type.</returns>
		/// <exception cref="InvalidOperationException">The version already has the continuation.</exception>
		/// <exception cref="ArgumentException">The specified type is not public and abstract or does not have a public parameterless constructor.</exception>
		public TypeVersionBuilder<TNext> DefineNextVersion<TNext>(Action<T, TNext> translator) where TNext : class => DefineNextVersion<TNext, object>(translator, null);
		/// <summary>
		/// Defines the next version of the reference type.
		/// </summary>
		/// <typeparam name="TNext">The type representing the next version of the database`s reference type.</typeparam>
		/// <typeparam name="TNextBase">The base type of the next version of the database`s reference type.</typeparam>
		/// <returns>A <see cref="TypeVersionBuilder{T}"/> to build the next version of the reference type.</returns>
		/// <exception cref="InvalidOperationException">The version already has the continuation.</exception>
		/// <exception cref="ArgumentException">The specified type is not public and abstract.</exception>
		public TypeVersionBuilder<TNext> DefineNextVersion<TNext, TNextBase>(Action<T, TNext> translator, TypeVersionBuilder<TNextBase> baseReferenceTypeVersionBuilder) where TNext : class, TNextBase where TNextBase : class
		{
			if (_continued)
				throw new InvalidOperationException("The version already has the continuation.");
			_continued = true;
			TypeVersionBuilder<TNext> nextVersionBuilder = _referenceTypeBuilder.DefineNextVersion<TNext>(baseReferenceTypeVersionBuilder);
			nextVersionBuilder._translateMethodInfo = translator.Method;
			return nextVersionBuilder;
		}
	}
}