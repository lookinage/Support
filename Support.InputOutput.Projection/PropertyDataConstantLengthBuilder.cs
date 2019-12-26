using System.Reflection;

namespace Noname.IO.ObjectOrientedDatabase
{
	internal sealed class PropertyDataConstantLengthBuilder : PropertyDataBuilder
	{
		internal PropertyDataConstantLengthBuilder(PropertyInfo propertyInfo, object bitConverter) : base(propertyInfo, bitConverter) { }
	}
}