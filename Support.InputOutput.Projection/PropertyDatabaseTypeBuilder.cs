using System.Reflection;

namespace Noname.IO.ObjectOrientedDatabase
{
	internal abstract class PropertyDatabaseTypeBuilder : PropertyBuilder
	{
		internal PropertyDatabaseTypeBuilder(PropertyInfo propertyInfo) : base(propertyInfo) { }
	}
}