using System.Reflection;

namespace Noname.IO.ObjectOrientedDatabase
{
	internal abstract class PropertyBuilder
	{
		internal readonly PropertyInfo _propertyInfo;

		internal PropertyBuilder(PropertyInfo propertyInfo) => _propertyInfo = propertyInfo;
	}
}