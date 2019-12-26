using System.Reflection;

namespace Noname.IO.ObjectOrientedDatabase
{
	internal sealed class PropertyDataVariableLengthBuilder : PropertyDataBuilder
	{
		internal PropertyDataVariableLengthBuilder(PropertyInfo propertyInfo, object bitConverter) : base(propertyInfo, bitConverter) { }
	}
}