namespace Support.Sets
{
	static internal class OrderHelper
	{
		static internal string GetIndexIsOutsideRangeOfValidIndices(object order, string indexName) => string.Format("{0} is outside the range of valid indices of the {1}.", indexName, order);
	}
}