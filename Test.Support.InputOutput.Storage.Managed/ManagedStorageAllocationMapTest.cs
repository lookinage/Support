//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Support.InputOutput.Storage.Managed;

//namespace Test.Support.InputOutput.Storage.Managed
//{
//	[TestClass]
//	public class ManagedStorageAllocationMapTest
//	{
//		[TestMethod]
//		public void CommonTest()
//		{
//			ManagedStorageAllocationMap map = new ManagedStorageAllocationMap(100);
//			long size = 0x100;
//			long address1 = map.Allocate(size);
//			long address2 = map.Allocate(size);
//			long address3 = map.Allocate(size);
//			map.Free(address2, size);
//			address2 = map.Allocate(size / 0x2);
//		}
//	}
//}