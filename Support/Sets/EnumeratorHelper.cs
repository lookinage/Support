namespace Support.Sets
{
	static internal class EnumeratorHelper
	{
		static internal string GetEnumeratorIsNotStartedExceptionMessage(object enumerator) => string.Format("The {0} is not started.", enumerator);
		static internal string GetEnumeratorIsOverEnumeratingExceptionMessage(object enumerator) => string.Format("The {0} is over.", enumerator);
	}
}