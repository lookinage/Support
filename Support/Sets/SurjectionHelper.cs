namespace Support.Sets
{
	static internal class SurjectionHelper
	{
		static internal string GetSurjectionAlreadyContainsRelationWithInputExceptionMessage(object surjection, string inputName) => string.Format("The {0} already contains a relation with {1}.", surjection, inputName);
		static internal string GetSurjectionDoesNotContainRelationWithInputExceptionMessage(object surjection, string inputName) => string.Format("The {0} does contain a relation with {1}.", surjection, inputName);
	}
}