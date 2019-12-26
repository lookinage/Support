//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Support;
//using Support.Sets;
//using System;

//namespace Test.Support.Sets
//{
//	[TestClass]
//	public class QueueTester
//	{
//		private const int _testCount = 0x1000;

//		[TestMethod]
//		public void ClearTest()
//		{
//			Ring<Integer> queue = new Ring<Integer>();
//			queue.Clear();
//			queue.AddLast(0x0);
//		}
//		[TestMethod]
//		public void InsertTest1()
//		{
//			Ring<Integer> queue = new Ring<Integer>();
//			Ring<Integer> queue1 = new Ring<Integer>();
//			queue1.AddLast(0x1);
//			queue1.AddLast(0x2);
//			queue1.AddLast(0x3);
//			SequenceHelper.ConvertedSequence<Relation<Integer, Integer>, Integer> sequence = queue1.GetConvertedSequence(a => a.Output);
//			queue.AddLast(sequence);
//			queue.AddLast(sequence);
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveLast();
//			_ = queue.RemoveLast();
//			queue1.Clear();
//			queue1.AddLast(0x4);
//			queue1.AddLast(0x5);
//			queue1.AddLast(0x6);
//			queue.InsertDirectly(0x0, sequence);
//		}
//		[TestMethod]
//		public void InsertTest2()
//		{
//			Ring<Integer> queue = new Ring<Integer>();
//			Ring<Integer> queue1 = new Ring<Integer>();
//			queue1.AddLast(0x1);
//			queue1.AddLast(0x2);
//			queue1.AddLast(0x3);
//			SequenceHelper.ConvertedSequence<Relation<Integer, Integer>, Integer> sequence = queue1.GetConvertedSequence(a => a.Output);
//			queue.AddLast(sequence);
//			queue.AddLast(sequence);
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			queue1.Clear();
//			queue1.AddLast(0x4);
//			queue1.AddLast(0x5);
//			queue1.AddLast(0x6);
//			queue.InsertDirectly(0x0, sequence);
//		}
//		[TestMethod]
//		public void InsertTest3()
//		{
//			Ring<Integer> queue = new Ring<Integer>();
//			Ring<Integer> queue1 = new Ring<Integer>();
//			queue1.AddLast(0x1);
//			queue1.AddLast(0x2);
//			queue1.AddLast(0x3);
//			SequenceHelper.ConvertedSequence<Relation<Integer, Integer>, Integer> sequence = queue1.GetConvertedSequence(a => a.Output);
//			queue.AddLast(sequence);
//			queue.AddLast(sequence);
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			queue1.Clear();
//			queue1.AddLast(0x4);
//			queue1.AddLast(0x5);
//			queue1.AddLast(0x6);
//			queue1.AddLast(0x7);
//			queue1.AddLast(0x8);
//			queue.InsertDirectly(0x0, sequence);
//		}
//		[TestMethod]
//		public void InsertTest4()
//		{
//			Ring<Integer> queue = new Ring<Integer>();
//			Ring<Integer> queue1 = new Ring<Integer>();
//			queue1.AddLast(0x1);
//			queue1.AddLast(0x2);
//			queue1.AddLast(0x3);
//			SequenceHelper.ConvertedSequence<Relation<Integer, Integer>, Integer> sequence = queue1.GetConvertedSequence(a => a.Output);
//			queue.AddLast(sequence);
//			queue.AddLast(sequence);
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			queue.AddLast(sequence);
//			queue1.Clear();
//			queue1.AddLast(0x4);
//			queue.InsertDirectly(0x0, sequence);
//		}
//		[TestMethod]
//		public void InsertTest5()
//		{
//			Ring<Integer> queue = new Ring<Integer>();
//			Ring<Integer> queue1 = new Ring<Integer>();
//			queue1.AddLast(0x1);
//			queue1.AddLast(0x2);
//			queue1.AddLast(0x3);
//			SequenceHelper.ConvertedSequence<Relation<Integer, Integer>, Integer> sequence = queue1.GetConvertedSequence(a => a.Output);
//			queue.AddLast(sequence);
//			queue.AddLast(sequence);
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			_ = queue.RemoveFirst();
//			queue.AddLast(sequence);
//			queue1.Clear();
//			queue1.AddLast(0x4);
//			queue1.AddLast(0x5);
//			queue1.AddLast(0x6);
//			queue.InsertDirectly(0x0, sequence);
//		}
//		[TestMethod]
//		public void EnqueueTest()
//		{
//			Ring<Integer> queue = new Ring<Integer>();
//			queue.AddLast(0x0);
//		}
//		[TestMethod]
//		public void EnqueueBackTest()
//		{
//			Ring<Integer> queue = new Ring<Integer>();
//			queue.AddFirst(0x0);
//		}
//		[TestMethod]
//		public void DequeueTest()
//		{
//			Ring<Integer> queue = new Ring<Integer>();
//			_ = Assert.ThrowsException<InvalidOperationException>(() => _ = queue.RemoveFirst());
//			queue.AddFirst(0x0);
//		}
//		[TestMethod]
//		public void DequeueBackTest()
//		{
//			Ring<Integer> queue = new Ring<Integer>();
//			_ = Assert.ThrowsException<InvalidOperationException>(() => _ = queue.RemoveLast());
//			queue.AddLast(0x0);
//		}
//		[TestMethod]
//		public void CommonTest()
//		{
//			Ring<Integer> queue = new Ring<Integer>();
//			int[] array = null;
//			int count = 0x0;
//			for (int testIndex = 0x0; testIndex != _testCount; testIndex++)
//			{
//				if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0 || queue.Count == 0x0)
//				{
//					int element = PseudoRandomManager.GetInt32Remainder(_testCount);
//					_ = ArrayHelper.EnsureLength(ref array, ++count);
//					if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
//					{
//						queue.AddLast(element);
//						array[count - 0x1] = element;
//					}
//					else
//					{
//						queue.AddFirst(element);
//						Array.Copy(array, 0x0, array, 0x1, count - 0x1);
//						array[0x0] = element;
//					}
//				}
//				else
//				{
//					if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
//						Assert.IsTrue(queue.RemoveLast() == array[--count]);
//					else
//					{
//						Assert.IsTrue(queue.RemoveFirst() == array[0x0]);
//						Array.Copy(array, 0x1, array, 0x0, --count);
//					}
//				}
//				Assert.IsTrue(queue.Count == count);
//				int index = 0x0;
//				queue.Handle(e =>
//				{
//					Assert.IsTrue(index != count);
//					int arrayElement = array[index];
//					Assert.IsTrue(queue[index] == arrayElement);
//					Assert.IsTrue(e.Output == arrayElement);
//					Assert.IsTrue(queue.TryGet(index, out e.Output));
//					Assert.IsTrue(e.Output == arrayElement);
//					index++;
//					return false;
//				});
//				Ring<Integer>.Enumerator enumerator = queue.GetEnumerator();
//				_ = Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current);
//				for (index = 0x0; index != count; index++)
//				{
//					int arrayElement = array[index];
//					Assert.IsTrue(queue[index] == arrayElement);
//					Assert.IsTrue(queue.TryGet(index, out Integer queueElement));
//					Assert.IsTrue(queueElement == arrayElement);
//					Assert.IsTrue(enumerator.MoveNext());
//					Assert.IsTrue(enumerator.Current.Output == arrayElement);
//				}
//				Assert.IsFalse(enumerator.MoveNext());
//				_ = Assert.ThrowsException<InvalidOperationException>(() => enumerator.Current);
//				enumerator.Dispose();
//			}
//		}
//	}
//}