//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Support;
//using Support.Sets;
//using System;
//using System.Collections.Generic;

//namespace Test.Support.Sets
//{
//	[TestClass]
//	public class RedBlackBinaryTreeSurjectionTest
//	{
//		private const int _testCount = 0x10000;

//		[TestMethod]
//		public void AddTest()
//		{
//			RedBlackBinaryTreeSurjection<string, string> tree = new RedBlackBinaryTreeSurjection<string, string>();
//			_ = Assert.ThrowsException<ArgumentNullException>(() => tree.Add(null, null));
//			tree.Add(string.Empty, null);
//			_ = Assert.ThrowsException<ArgumentException>(() => tree.Add(string.Empty, null));
//		}
//		[TestMethod]
//		public void RemoveTest()
//		{
//			RedBlackBinaryTreeSurjection<string, string> tree = new RedBlackBinaryTreeSurjection<string, string>();
//			_ = Assert.ThrowsException<ArgumentNullException>(() => tree.Remove(null));
//			_ = tree.Remove(string.Empty);
//		}
//		[TestMethod]
//		public void ClearTest()
//		{
//			RedBlackBinaryTreeSurjection<string, string> tree = new RedBlackBinaryTreeSurjection<string, string>();
//			tree.Clear();
//		}
//		[TestMethod]
//		public void CommonTest()
//		{
//			RedBlackBinaryTreeSurjection<int, int> tree = new RedBlackBinaryTreeSurjection<int, int>();
//			Dictionary<int, int> copy = new Dictionary<int, int>();
//			for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
//			{
//				if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
//				{
//					int key = PseudoRandomManager.GetInt32Remainder(_testCount);
//					int value = PseudoRandomManager.GetInt32Remainder(_testCount);
//					if (copy.ContainsKey(key))
//					{
//						Assert.IsTrue(tree.ContainsKey(key));
//						_ = Assert.ThrowsException<ArgumentException>(() => tree.Add(key, value));
//						Assert.IsTrue(tree[key] == copy[key]);
//						continue;
//					}
//					else
//					{
//						Assert.IsFalse(copy.ContainsKey(key));
//						tree.Add(key, value);
//						copy.Add(key, value);
//					}
//				}
//			}
//		}
//	}
//}