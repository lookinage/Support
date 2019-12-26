using System.Reflection;

namespace Noname.IO.ObjectOrientedDatabase
{
	internal abstract class PropertyDataBuilder : PropertyBuilder
	{
		internal readonly object _bitConverter;

		internal PropertyDataBuilder(PropertyInfo propertyInfo, object bitConverter) : base(propertyInfo) => _bitConverter = bitConverter;
	}
}