namespace Support.Sets
{
	static internal class SubsetHelper
	{
		static internal string GetSubsetWouldBeOverflowed(object instance) => string.Format("The {0} would be overflowed.", instance);
		static internal string GetSubsetIsEmptyExceptionMessage(object instance) => string.Format("The {0} is empty.", instance);
		static internal string GetSubsetAlreadyContainsElementExceptionMessage(object instance, string elementName) => string.Format("The {0} already contains {1}.", instance, elementName);
		static internal string GetSubsetDoesNotContainElementExceptionMessage(object instance, string elementName) => string.Format("The {0} does not contain the {1}.", instance, elementName);
		static internal string GetSubsetDoesNotContainElementExceptionMessage(object instance) => GetSubsetDoesNotContainElementExceptionMessage(instance, "the element");
		static internal string GetSubsetDoesNotContainElementsAsManyAsItIsSpecifiedExceptionMessage(string instanceName) => string.Format("{0} does not contain elements as many as it is specified.", instanceName);
		static internal string GetSubsetDoesNotContainElementsAsManyAsItIsSpecifiedExceptionMessage() => GetSubsetDoesNotContainElementsAsManyAsItIsSpecifiedExceptionMessage("The subset");
	}
}