//using System;

//namespace Support.Sets
//{
//	static internal class RedBlackBinaryTreeEnumeratorStackBuffer
//	{
//		[ThreadStatic]
//		static private Queue<Queue<int>> _stacks;

//		static internal Queue<int> Allocate()
//		{
//			if (_stacks == null)
//			{
//				_stacks = new Queue<Queue<int>>();
//				return new Queue<int>();
//			}
//			return _stacks.Count == 0x0 ? new Queue<int>() : _stacks.RemoveLast();
//		}
//		static internal void Free(Queue<int> stack)
//		{
//			stack.Clear();
//			_stacks.AddLast(stack);
//		}
//	}
//}