//using System;
//using System.Diagnostics;

//namespace Support.Sets
//{
//	/// <summary>
//	/// Represents an <see cref="ISortedSurjection{TInput, TOutput}"/> that is implemented as a red black binary tree.
//	/// </summary>
//	/// <typeparam name="TInput">The type of elements of the input set.</typeparam>
//	/// <typeparam name="TOutput">The type of elements of the output set.</typeparam>
//	public class RedBlackBinaryTreeSurjection<TInput, TOutput> : ISortedSurjection<TInput, TOutput>
//	{
//		private struct Slot
//		{
//			internal TInput _input;
//			internal TOutput _output;
//			internal bool _isRed;
//			internal int _left;
//			internal int _right;
//		}

//		private const int _defaultCapacity = 0x4;

//		static private int RotateLeft(Slot[] slots, int slotIndex)
//		{
//			int pivotSlotIndex = slots[slotIndex]._right;
//			slots[slotIndex]._right = slots[pivotSlotIndex]._left;
//			slots[pivotSlotIndex]._left = slotIndex;
//			return pivotSlotIndex;
//		}
//		static private int RotateLeftRight(Slot[] slots, int slotIndex)
//		{
//			int childSlotIndex = slots[slotIndex]._left;
//			int grandChildSlotIndex = slots[childSlotIndex]._right;
//			slots[slotIndex]._left = slots[grandChildSlotIndex]._right;
//			slots[grandChildSlotIndex]._right = slotIndex;
//			slots[childSlotIndex]._right = slots[grandChildSlotIndex]._left;
//			slots[grandChildSlotIndex]._left = childSlotIndex;
//			return grandChildSlotIndex;
//		}
//		static private int RotateRight(Slot[] slots, int slotIndex)
//		{
//			int pivotSlotIndex = slots[slotIndex]._left;
//			slots[slotIndex]._left = slots[pivotSlotIndex]._right;
//			slots[pivotSlotIndex]._right = slotIndex;
//			return pivotSlotIndex;
//		}
//		static private int RotateRightLeft(Slot[] slots, int slotIndex)
//		{
//			int childSlotIndex = slots[slotIndex]._right;
//			int grandChildSlotIndex = slots[childSlotIndex]._left;
//			slots[slotIndex]._right = slots[grandChildSlotIndex]._left;
//			slots[grandChildSlotIndex]._left = slotIndex;
//			slots[childSlotIndex]._left = slots[grandChildSlotIndex]._right;
//			slots[grandChildSlotIndex]._right = childSlotIndex;
//			return grandChildSlotIndex;
//		}

//		private readonly IBinaryComparator<TInput> _inputElementComparator;
//		private readonly IEqualityComparator<TOutput> _outputElementComparator;
//		private Slot[] _slots;
//		private int _rootSlotIndex;
//		private int _freeSlotIndex;
//		private int _usedSlotCount;
//		private int _count;

//		/// <summary>
//		/// Initializes the <see cref="RedBlackBinaryTreeSurjection{TKey, TValue}"/>.
//		/// </summary>
//		/// <param name="inputElementComparator">An <see cref="IBinaryComparator{T}"/> of the elements of the input set.</param>
//		/// <param name="outputElementComparator">An <see cref="IEqualityComparator{T}"/> of the elements of the output set.</param>
//		/// <param name="capacity">The minimum number of relations of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="inputElementComparator"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentNullException"><paramref name="outputElementComparator"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
//		public RedBlackBinaryTreeSurjection(IBinaryComparator<TInput> inputElementComparator, IEqualityComparator<TOutput> outputElementComparator, int capacity)
//		{
//			_inputElementComparator = inputElementComparator ?? throw new ArgumentNullException(nameof(inputElementComparator));
//			_outputElementComparator = outputElementComparator ?? throw new ArgumentNullException(nameof(outputElementComparator));
//			if (capacity < 0x0)
//				throw new ArgumentOutOfRangeException(nameof(capacity));
//			if (capacity < _defaultCapacity)
//				capacity = _defaultCapacity;
//			_slots = new Slot[capacity];
//			_count = 0x0;
//			_usedSlotCount = 0x0;
//			_freeSlotIndex = -0x1;
//			_rootSlotIndex = -0x1;
//		}
//		/// <summary>
//		/// Initializes the <see cref="RedBlackBinaryTreeSurjection{TKey, TValue}"/>.
//		/// </summary>
//		/// <param name="inputElementComparator">An <see cref="IBinaryComparator{T}"/> of the elements of the input set.</param>
//		/// <param name="capacity">The minimum number of relations of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="inputElementComparator"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
//		public RedBlackBinaryTreeSurjection(IBinaryComparator<TInput> inputElementComparator, int capacity) : this(inputElementComparator, DefaultEqualityComparator<TOutput>.Instance, capacity) { }
//		/// <summary>
//		/// Initializes the <see cref="RedBlackBinaryTreeSurjection{TKey, TValue}"/>.
//		/// </summary>
//		/// <param name="inputElementComparator">An <see cref="IBinaryComparator{T}"/> of the elements of the input set.</param>
//		/// <param name="outputElementComparator">An <see cref="IEqualityComparator{T}"/> of the elements of the output set.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="inputElementComparator"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentNullException"><paramref name="outputElementComparator"/> is <see langword="null"/>.</exception>
//		public RedBlackBinaryTreeSurjection(IBinaryComparator<TInput> inputElementComparator, IEqualityComparator<TOutput> outputElementComparator) : this(inputElementComparator, outputElementComparator, _defaultCapacity) { }
//		/// <summary>
//		/// Initializes the <see cref="RedBlackBinaryTreeSurjection{TKey, TValue}"/>.
//		/// </summary>
//		/// <param name="inputElementComparator">An <see cref="IBinaryComparator{T}"/> of the elements of the input set.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="inputElementComparator"/> is <see langword="null"/>.</exception>
//		public RedBlackBinaryTreeSurjection(IBinaryComparator<TInput> inputElementComparator) : this(inputElementComparator, DefaultEqualityComparator<TOutput>.Instance, _defaultCapacity) { }

//		/// <summary>
//		/// Gets the number of elements contained in the <see cref="RedBlackBinaryTreeSurjection{TKey, TValue}"/>.
//		/// </summary>
//		public int Count => _count;

//		private void Balance(int slotIndex, ref int parentSlotIndex, int grandParentSlotIndex, int greatGrandparentSlotIndex)
//		{
//			Debug.Assert(grandParentSlotIndex != -0x1);
//			bool parentIsOnRight = _slots[grandParentSlotIndex]._right == parentSlotIndex;
//			bool currentIsOnRight = _slots[parentSlotIndex]._right == slotIndex;
//			int newChildOfGreatGrandParentSlotIndex;
//			if (parentIsOnRight == currentIsOnRight)
//				newChildOfGreatGrandParentSlotIndex = currentIsOnRight ? RotateLeft(_slots, grandParentSlotIndex) : RotateRight(_slots, grandParentSlotIndex);
//			else
//			{
//				newChildOfGreatGrandParentSlotIndex = currentIsOnRight ? RotateLeftRight(_slots, grandParentSlotIndex) : RotateRightLeft(_slots, grandParentSlotIndex);
//				parentSlotIndex = greatGrandparentSlotIndex;
//			}
//			_slots[grandParentSlotIndex]._isRed = true;
//			_slots[newChildOfGreatGrandParentSlotIndex]._isRed = false;
//			if (greatGrandparentSlotIndex != -0x1)
//				if (_slots[greatGrandparentSlotIndex]._left == grandParentSlotIndex)
//					_slots[greatGrandparentSlotIndex]._left = newChildOfGreatGrandParentSlotIndex;
//				else
//					_slots[greatGrandparentSlotIndex]._right = newChildOfGreatGrandParentSlotIndex;
//			else
//				_rootSlotIndex = newChildOfGreatGrandParentSlotIndex;
//		}
//		private void IncreaseCapacity()
//		{
//			int capacity = _slots.Length << 0x1;
//			if (capacity <= _slots.Length)
//				capacity = int.MaxValue;
//			Slot[] newNodes = new Slot[capacity];
//			Array.Copy(_slots, newNodes, _slots.Length);
//			_slots = newNodes;
//		}
//		private int TakeNode()
//		{
//			int slotIndex;
//			if (_freeSlotIndex != -0x1)
//			{
//				Debug.Assert(_count < _usedSlotCount);
//				slotIndex = _freeSlotIndex;
//				_freeSlotIndex = _slots[slotIndex]._left;
//			}
//			else
//			{
//				Debug.Assert(_count == _usedSlotCount);
//				if (_usedSlotCount == _slots.Length)
//					IncreaseCapacity();
//				slotIndex = _usedSlotCount++;
//			}
//			_count++;
//			return slotIndex;
//		}
//		private void FreeNode(int slotIndex)
//		{
//			_slots[slotIndex]._input = default;
//			_slots[slotIndex]._output = default;
//			_slots[slotIndex]._left = _freeSlotIndex;
//			_freeSlotIndex = slotIndex;
//			_count--;
//		}
//		/// <summary>
//		/// Handles each relation of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> with a specified <see cref="IElementHandler{T}"/>.
//		/// </summary>
//		/// <param name="handler">The handler of the relations.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
//		public void HandleEach(IElementHandler<Relation<TInput, TOutput>> handler)
//		{
//			if (handler == null)
//				throw new ArgumentNullException(nameof(handler));
//			Queue<int> stack = RedBlackBinaryTreeEnumeratorStackBuffer.Allocate();
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				stack.EnqueueBack(slotIndex);
//				slotIndex = slots[slotIndex]._left;
//			}
//			try
//			{
//				while (stack.Count != 0x0)
//				{
//					if (handler.Handle(new Relation<TInput, TOutput>(slots[slotIndex = stack.Dequeue()]._input, slots[slotIndex]._output)))
//						return;
//					for (slotIndex = slots[slotIndex]._right; slotIndex != -0x1; slotIndex = slots[slotIndex]._left)
//						stack.EnqueueBack(slotIndex);
//				}
//			}
//			finally { RedBlackBinaryTreeEnumeratorStackBuffer.Free(stack); }
//		}
//		/// <summary>
//		/// Determines whether the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> contains a relation.
//		/// </summary>
//		/// <param name="relation">The relation to locate in the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>.</param>
//		/// <returns><see langword="true"/> whether <paramref name="relation"/> is found in the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>; otherwise, <see langword="false"/>.</returns>
//		public bool Contains(Relation<TInput, TOutput> relation)
//		{
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				int compareResult = inputElementComparator.Compare(relation.Input, slots[slotIndex]._input);
//				if (compareResult < 0x0)
//				{
//					slotIndex = slots[slotIndex]._left;
//					continue;
//				}
//				if (compareResult > 0x0)
//				{
//					slotIndex = slots[slotIndex]._right;
//					continue;
//				}
//				return _outputElementComparator.Compare(relation.Output, slots[slotIndex]._output);
//			}
//			return false;
//		}
//		/// <summary>
//		/// Determines the number of relations of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> that contain a specified input element.
//		/// </summary>
//		/// <param name="input">The input element to locate in relations of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>.</param>
//		/// <returns>The number of relations of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> that contain <paramref name="input"/>.</returns>
//		public int GetInputCount(TInput input)
//		{
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				int compareResult = inputElementComparator.Compare(input, slots[slotIndex]._input);
//				if (compareResult < 0x0)
//				{
//					slotIndex = slots[slotIndex]._left;
//					continue;
//				}
//				if (compareResult > 0x0)
//				{
//					slotIndex = slots[slotIndex]._right;
//					continue;
//				}
//				return 0x1;
//			}
//			return 0x0;
//		}
//		/// <summary>
//		/// Determines the number of relations of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> that contain a specified output element.
//		/// </summary>
//		/// <param name="output">The output element to locate in relations of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>.</param>
//		/// <returns>The number of relations of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> that contain <paramref name="output"/>.</returns>
//		public int GetOutputCount(TOutput output)
//		{
//			Queue<int> stack = RedBlackBinaryTreeEnumeratorStackBuffer.Allocate();
//			IEqualityComparator<TOutput> outputElementComparator = _outputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				stack.EnqueueBack(slotIndex);
//				slotIndex = slots[slotIndex]._left;
//			}
//			int count = 0x0;
//			try
//			{
//				while (stack.Count != 0x0)
//				{
//					if (outputElementComparator.Compare(output, slots[slotIndex = stack.Dequeue()]._output))
//						count++;
//					for (slotIndex = slots[slotIndex]._right; slotIndex != -0x1; slotIndex = slots[slotIndex]._left)
//						stack.EnqueueBack(slotIndex);
//				}
//			}
//			finally { RedBlackBinaryTreeEnumeratorStackBuffer.Free(stack); }
//			return count;
//		}
//		/// <summary>
//		/// Determines whether the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> contains a relation with a specified input element.
//		/// </summary>
//		/// <param name="input">The input element to locate in a relation of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>.</param>
//		/// <returns><see langword="true"/> whether <paramref name="input"/> is found in a relation of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>; otherwise, <see langword="false"/>.</returns>
//		public bool Contains(TInput input)
//		{
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				int compareResult = inputElementComparator.Compare(input, slots[slotIndex]._input);
//				if (compareResult < 0x0)
//				{
//					slotIndex = slots[slotIndex]._left;
//					continue;
//				}
//				if (compareResult > 0x0)
//				{
//					slotIndex = slots[slotIndex]._right;
//					continue;
//				}
//				return true;
//			}
//			return false;
//		}
//		/// <summary>
//		/// Determines whether the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> contains a relation with a specified output element.
//		/// </summary>
//		/// <param name="output">The output element to locate in a relation of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>.</param>
//		/// <returns><see langword="true"/> whether <paramref name="output"/> is found in a relation of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>; otherwise, <see langword="false"/>.</returns>
//		public bool ContainsOutput(TOutput output)
//		{
//			Queue<int> stack = RedBlackBinaryTreeEnumeratorStackBuffer.Allocate();
//			IEqualityComparator<TOutput> outputElementComparator = _outputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				stack.EnqueueBack(slotIndex);
//				slotIndex = slots[slotIndex]._left;
//			}
//			try
//			{
//				while (stack.Count != 0x0)
//				{
//					if (outputElementComparator.Compare(output, slots[slotIndex = stack.Dequeue()]._output))
//						return true;
//					for (slotIndex = slots[slotIndex]._right; slotIndex != -0x1; slotIndex = slots[slotIndex]._left)
//						stack.EnqueueBack(slotIndex);
//				}
//			}
//			finally { RedBlackBinaryTreeEnumeratorStackBuffer.Free(stack); }
//			return false;
//		}
//		/// <summary>
//		/// Handles each output element of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> that corresponds a specified input element with a specified <see cref="IElementHandler{T}"/>.
//		/// </summary>
//		/// <param name="input">The input element.</param>
//		/// <param name="handler">The handler of the elements.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
//		public void HandleEachOutput(IElementHandler<TOutput> handler, TInput input)
//		{
//			if (handler == null)
//				throw new ArgumentNullException(nameof(handler));
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				int compareResult = inputElementComparator.Compare(input, slots[slotIndex]._input);
//				if (compareResult < 0x0)
//				{
//					slotIndex = slots[slotIndex]._left;
//					continue;
//				}
//				if (compareResult > 0x0)
//				{
//					slotIndex = slots[slotIndex]._right;
//					continue;
//				}
//				_ = handler.Handle(slots[slotIndex]._output);
//				return;
//			}
//		}
//		/// <summary>
//		/// Handles each input element of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> that corresponds a specified output element with a specified <see cref="IElementHandler{T}"/>.
//		/// </summary>
//		/// <param name="output">The input element.</param>
//		/// <param name="handler">The handler of the elements.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
//		public void HandleEachInput(IElementHandler<TInput> handler, TOutput output)
//		{
//			if (handler == null)
//				throw new ArgumentNullException(nameof(handler));
//			Queue<int> stack = RedBlackBinaryTreeEnumeratorStackBuffer.Allocate();
//			IEqualityComparator<TOutput> outputElementComparator = _outputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				stack.EnqueueBack(slotIndex);
//				slotIndex = slots[slotIndex]._left;
//			}
//			try
//			{
//				while (stack.Count != 0x0)
//				{
//					if (outputElementComparator.Compare(output, slots[slotIndex = stack.Dequeue()]._output))
//						if (handler.Handle(slots[slotIndex]._input))
//							return;
//					for (slotIndex = slots[slotIndex]._right; slotIndex != -0x1; slotIndex = slots[slotIndex]._left)
//						stack.EnqueueBack(slotIndex);
//				}
//			}
//			finally { RedBlackBinaryTreeEnumeratorStackBuffer.Free(stack); }
//		}
//		/// <summary>
//		/// Determines the output element that corresponds a specified input element.
//		/// </summary>
//		/// <param name="input">The input element.</param>
//		/// <returns>The output element that corresponds <paramref name="input"/>.</returns>
//		/// <exception cref="ArgumentException">The <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> does not contain a relation with <paramref name="input"/>.</exception>
//		public TOutput GetOutput(TInput input)
//		{
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				int compareResult = inputElementComparator.Compare(input, slots[slotIndex]._input);
//				if (compareResult < 0x0)
//				{
//					slotIndex = slots[slotIndex]._left;
//					continue;
//				}
//				if (compareResult > 0x0)
//				{
//					slotIndex = slots[slotIndex]._right;
//					continue;
//				}
//				return slots[slotIndex]._output;
//			}
//			throw new ArgumentException(SurjectionHelper.GetSurjectionDoesNotContainRelationWithInputExceptionMessage(this, nameof(input)));
//		}
//		/// <summary>
//		/// Determines the output element that corresponds a specified input element if the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> contains a relation with a specified input element.
//		/// </summary>
//		/// <param name="input">The input element.</param>
//		/// <param name="output">The output element that corresponds <paramref name="input"/> if the relation is found; otherwise, the default value.</param>
//		/// <returns><see langword="true"/> whether the relation is found; otherwise, <see langword="false"/>.</returns>
//		public bool TryGetOutput(TInput input, out TOutput output)
//		{
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				int compareResult = inputElementComparator.Compare(input, slots[slotIndex]._input);
//				if (compareResult < 0x0)
//				{
//					slotIndex = slots[slotIndex]._left;
//					continue;
//				}
//				if (compareResult > 0x0)
//				{
//					slotIndex = slots[slotIndex]._right;
//					continue;
//				}
//				output = slots[slotIndex]._output;
//				return true;
//			}
//			output = default;
//			return false;
//		}
//		/// <summary>
//		/// Determines the input element that corresponds a specified output element.
//		/// </summary>
//		/// <param name="output">The output element.</param>
//		/// <returns>The input element that corresponds <paramref name="output"/>.</returns>
//		/// <exception cref="ArgumentException">The <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> does not contain a relation with <paramref name="output"/>.</exception>
//		public TInput GetInput(TOutput output)
//		{
//			Queue<int> stack = RedBlackBinaryTreeEnumeratorStackBuffer.Allocate();
//			IEqualityComparator<TOutput> outputElementComparator = _outputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				stack.EnqueueBack(slotIndex);
//				slotIndex = slots[slotIndex]._left;
//			}
//			try
//			{
//				while (stack.Count != 0x0)
//				{
//					if (outputElementComparator.Compare(output, slots[slotIndex = stack.Dequeue()]._output))
//						return slots[slotIndex]._input;
//					for (slotIndex = slots[slotIndex]._right; slotIndex != -0x1; slotIndex = slots[slotIndex]._left)
//						stack.EnqueueBack(slotIndex);
//				}
//			}
//			finally { RedBlackBinaryTreeEnumeratorStackBuffer.Free(stack); }
//			throw new ArgumentException(SurjectionHelper.GetSurjectionDoesNotContainRelationWithInputExceptionMessage(this, nameof(output)));
//		}
//		/// <summary>
//		/// Determines the input element that corresponds a specified output element if the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> contains a relation with a specified output element.
//		/// </summary>
//		/// <param name="output">The output element.</param>
//		/// <param name="input">The input element that corresponds <paramref name="output"/> if the relation is found; otherwise, the default value.</param>
//		/// <returns><see langword="true"/> whether the relation is found; otherwise, <see langword="false"/>.</returns>
//		public bool TryGetInput(TOutput output, out TInput input)
//		{
//			Queue<int> stack = RedBlackBinaryTreeEnumeratorStackBuffer.Allocate();
//			IEqualityComparator<TOutput> outputElementComparator = _outputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				stack.EnqueueBack(slotIndex);
//				slotIndex = slots[slotIndex]._left;
//			}
//			try
//			{
//				while (stack.Count != 0x0)
//				{
//					if (outputElementComparator.Compare(output, slots[slotIndex = stack.Dequeue()]._output))
//					{
//						input = slots[slotIndex]._input;
//						return true;
//					}
//					for (slotIndex = slots[slotIndex]._right; slotIndex != -0x1; slotIndex = slots[slotIndex]._left)
//						stack.EnqueueBack(slotIndex);
//				}
//			}
//			finally { RedBlackBinaryTreeEnumeratorStackBuffer.Free(stack); }
//			input = default;
//			return false;
//		}
//		/// <summary>
//		/// Adds a relation to the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>.
//		/// </summary>
//		/// <param name="input">The input element of the relation.</param>
//		/// <param name="output">The output element of the relation.</param>
//		/// <exception cref="InvalidOperationException">The <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> contains the maximum number of elements.</exception>
//		/// <exception cref="ArgumentException">The <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> already contains a relation with <paramref name="input"/>.</exception>
//		public void Add(TInput input, TOutput output)
//		{
//			int slotIndex = _rootSlotIndex;
//			if (slotIndex == -0x1)
//			{
//				slotIndex = TakeNode();
//				_slots[slotIndex]._input = input;
//				_slots[slotIndex]._output = output;
//				_slots[slotIndex]._isRed = false;
//				_slots[slotIndex]._left = -0x1;
//				_slots[slotIndex]._right = -0x1;
//				_rootSlotIndex = slotIndex;
//				return;
//			}
//			int parent = -0x1;
//			int grandparent = -0x1;
//			int greatGrandparent = -0x1;
//			int order = 0x0;
//			while (slotIndex != -0x1)
//			{
//				order = _inputElementComparator.Compare(input, _slots[slotIndex]._input);
//				if (order == 0x0)
//				{
//					_slots[_rootSlotIndex]._isRed = false;
//					throw new ArgumentException(SurjectionHelper.GetSurjectionAlreadyContainsRelationWithInputExceptionMessage(this, nameof(input)));
//				}
//				if (_slots[slotIndex]._left != -0x1 && _slots[_slots[slotIndex]._left]._isRed && _slots[slotIndex]._right != -0x1 && _slots[_slots[slotIndex]._right]._isRed)
//				{
//					_slots[slotIndex]._isRed = true;
//					_slots[_slots[slotIndex]._left]._isRed = false;
//					_slots[_slots[slotIndex]._right]._isRed = false;
//					if (parent != -0x1 && _slots[parent]._isRed)
//						Balance(slotIndex, ref parent, grandparent, greatGrandparent);
//				}
//				greatGrandparent = grandparent;
//				grandparent = parent;
//				parent = slotIndex;
//				slotIndex = order < 0x0 ? _slots[slotIndex]._left : _slots[slotIndex]._right;
//			}
//			Debug.Assert(parent != -0x1);
//			slotIndex = TakeNode();
//			if (order > 0x0)
//				_slots[parent]._right = slotIndex;
//			else
//				_slots[parent]._left = slotIndex;
//			_slots[slotIndex]._input = input;
//			_slots[slotIndex]._output = output;
//			_slots[slotIndex]._isRed = true;
//			_slots[slotIndex]._left = -0x1;
//			_slots[slotIndex]._right = -0x1;
//			if (_slots[parent]._isRed)
//				Balance(slotIndex, ref parent, grandparent, greatGrandparent);
//			_slots[_rootSlotIndex]._isRed = false;
//		}
//		/// <summary>
//		/// Adds a relation to the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> if <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> does not contain a relation with a specified input element.
//		/// </summary>
//		/// <param name="input">The input element of the relation.</param>
//		/// <param name="output">The output element of the relation.</param>
//		/// <returns><see langword="true"/> whether the relation is added; otherwise, <see langword="false"/>.</returns>
//		/// <exception cref="InvalidOperationException">The <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> contains the maximum number of elements.</exception>
//		public bool TryAdd(TInput input, TOutput output)
//		{
//			int slotIndex = _rootSlotIndex;
//			if (slotIndex == -0x1)
//			{
//				slotIndex = TakeNode();
//				_slots[slotIndex]._input = input;
//				_slots[slotIndex]._output = output;
//				_slots[slotIndex]._isRed = false;
//				_slots[slotIndex]._left = -0x1;
//				_slots[slotIndex]._right = -0x1;
//				_rootSlotIndex = slotIndex;
//				return true;
//			}
//			int parent = -0x1;
//			int grandparent = -0x1;
//			int greatGrandparent = -0x1;
//			int order = 0x0;
//			while (slotIndex != -0x1)
//			{
//				order = _inputElementComparator.Compare(input, _slots[slotIndex]._input);
//				if (order == 0x0)
//				{
//					_slots[_rootSlotIndex]._isRed = false;
//					return false;
//				}
//				if (_slots[slotIndex]._left != -0x1 && _slots[_slots[slotIndex]._left]._isRed && _slots[slotIndex]._right != -0x1 && _slots[_slots[slotIndex]._right]._isRed)
//				{
//					_slots[slotIndex]._isRed = true;
//					_slots[_slots[slotIndex]._left]._isRed = false;
//					_slots[_slots[slotIndex]._right]._isRed = false;
//					if (parent != -0x1 && _slots[parent]._isRed)
//						Balance(slotIndex, ref parent, grandparent, greatGrandparent);
//				}
//				greatGrandparent = grandparent;
//				grandparent = parent;
//				parent = slotIndex;
//				slotIndex = order < 0x0 ? _slots[slotIndex]._left : _slots[slotIndex]._right;
//			}
//			Debug.Assert(parent != -0x1);
//			slotIndex = TakeNode();
//			if (order > 0x0)
//				_slots[parent]._right = slotIndex;
//			else
//				_slots[parent]._left = slotIndex;
//			_slots[slotIndex]._input = input;
//			_slots[slotIndex]._output = output;
//			_slots[slotIndex]._isRed = true;
//			_slots[slotIndex]._left = -0x1;
//			_slots[slotIndex]._right = -0x1;
//			if (_slots[parent]._isRed)
//				Balance(slotIndex, ref parent, grandparent, greatGrandparent);
//			_slots[_rootSlotIndex]._isRed = false;
//			return true;
//		}
//		/// <summary>
//		/// Changes the input element of a relation of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>.
//		/// </summary>
//		/// <param name="input">The input element of the relation.</param>
//		/// <param name="newOutput">The new output element of the relation.</param>
//		/// <param name="output">The previous output element of the relation.</param>
//		/// <exception cref="ArgumentException">The <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> does not contain a relation with <paramref name="input"/>.</exception>
//		public void ChangeOutput(TInput input, TOutput newOutput, out TOutput output)
//		{
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				int compareResult = inputElementComparator.Compare(input, slots[slotIndex]._input);
//				if (compareResult < 0x0)
//				{
//					slotIndex = slots[slotIndex]._left;
//					continue;
//				}
//				if (compareResult > 0x0)
//				{
//					slotIndex = slots[slotIndex]._right;
//					continue;
//				}
//				output = slots[slotIndex]._output;
//				slots[slotIndex]._output = newOutput;
//				return;
//			}
//			throw new ArgumentException(SurjectionHelper.GetSurjectionDoesNotContainRelationWithInputExceptionMessage(this, nameof(input)));
//		}
//		/// <summary>
//		/// Changes the input element of a relation of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> if the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> contains a relation with the specified input element.
//		/// </summary>
//		/// <param name="input">The input element of the relation.</param>
//		/// <param name="newOutput">The new output element of the relation.</param>
//		/// <param name="output">The previous output element of the relation if it is changed; otherwise, the default value.</param>
//		public bool TryChangeOutput(TInput input, TOutput newOutput, out TOutput output)
//		{
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				int compareResult = inputElementComparator.Compare(input, slots[slotIndex]._input);
//				if (compareResult < 0x0)
//				{
//					slotIndex = slots[slotIndex]._left;
//					continue;
//				}
//				if (compareResult > 0x0)
//				{
//					slotIndex = slots[slotIndex]._right;
//					continue;
//				}
//				output = slots[slotIndex]._output;
//				slots[slotIndex]._output = newOutput;
//				return true;
//			}
//			output = default;
//			return false;
//		}
//		/// <summary>
//		/// Removes a relation with a specified input element from the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/>.
//		/// </summary>
//		/// <param name="input">The input element of the relation.</param>
//		/// <param name="output">The output element of the relation.</param>
//		/// <exception cref="ArgumentException">The <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> does not contain a relation with <paramref name="input"/>.</exception>
//		public void Remove(TInput input, out TOutput output)
//		{
//			if (_rootSlotIndex == -0x1)
//				throw new ArgumentException(SurjectionHelper.GetSurjectionDoesNotContainRelationWithInputExceptionMessage(this, nameof(input)));
//			int current = _rootSlotIndex;
//			int parent = -0x1;
//			int grandParent = -0x1;
//			int match = -0x1;
//			int parentOfMatch = -0x1;
//			bool foundMatch = false;
//			while (current != -0x1)
//			{
//				if (current != -0x1 && !_slots[current]._isRed && (_slots[current]._left == -0x1 || !_slots[_slots[current]._left]._isRed) && (_slots[current]._right == -0x1 || !_slots[_slots[current]._right]._isRed))
//				{
//					if (parent == -0x1)
//						_slots[current]._isRed = true;
//					else
//					{
//						int sibling = _slots[parent]._left == current ? _slots[parent]._right : _slots[parent]._left;
//						if (_slots[sibling]._isRed)
//						{
//							Debug.Assert(!_slots[parent]._isRed);
//							_ = _slots[parent]._right == sibling ? RotateLeft(_slots, parent) : RotateRight(_slots, parent);
//							_slots[parent]._isRed = true;
//							_slots[sibling]._isRed = false;
//							if (grandParent != -0x1)
//								if (_slots[grandParent]._left == parent)
//									_slots[grandParent]._left = sibling;
//								else
//									_slots[grandParent]._right = sibling;
//							else
//								_rootSlotIndex = sibling;
//							grandParent = sibling;
//							if (parent == match)
//								parentOfMatch = sibling;
//							sibling = (_slots[parent]._left == current) ? _slots[parent]._right : _slots[parent]._left;
//						}
//						Debug.Assert(sibling != -0x1 || _slots[sibling]._isRed == false);
//						if (sibling != -0x1 && !_slots[sibling]._isRed && (_slots[sibling]._left == -0x1 || !_slots[_slots[sibling]._left]._isRed) && (_slots[sibling]._right == -0x1 || !_slots[_slots[sibling]._right]._isRed))
//						{
//							Debug.Assert(parent != -0x1 && _slots[parent]._isRed);
//							_slots[parent]._isRed = false;
//							_slots[current]._isRed = true;
//							_slots[sibling]._isRed = true;
//						}
//						else
//						{
//							Debug.Assert(_slots[sibling]._left != -0x1 && _slots[_slots[sibling]._left]._isRed || _slots[sibling]._right != -0x1 && _slots[_slots[sibling]._right]._isRed, "sibling must have at least one red child!");
//							int newGrandParent;
//							if (_slots[sibling]._left != -0x1 && _slots[_slots[sibling]._left]._isRed)
//							{
//								if (_slots[parent]._left == current)
//								{
//									Debug.Assert(_slots[parent]._right == sibling);
//									Debug.Assert(_slots[_slots[sibling]._left]._isRed);
//									newGrandParent = RotateRightLeft(_slots, parent);
//								}
//								else
//								{
//									Debug.Assert(_slots[parent]._left == sibling);
//									Debug.Assert(_slots[_slots[sibling]._left]._isRed);
//									_slots[_slots[sibling]._left]._isRed = false;
//									newGrandParent = RotateRight(_slots, parent);
//								}
//							}
//							else
//							{
//								if (_slots[parent]._left == current)
//								{
//									Debug.Assert(_slots[parent]._right == sibling);
//									Debug.Assert(_slots[_slots[sibling]._right]._isRed);
//									_slots[_slots[sibling]._right]._isRed = false;
//									newGrandParent = RotateLeft(_slots, parent);
//								}
//								else
//								{
//									Debug.Assert(_slots[parent]._left == sibling);
//									Debug.Assert(_slots[_slots[sibling]._right]._isRed);
//									newGrandParent = RotateLeftRight(_slots, parent);
//								}
//							}
//							_slots[newGrandParent]._isRed = _slots[parent]._isRed;
//							_slots[parent]._isRed = false;
//							_slots[current]._isRed = true;
//							if (grandParent != -0x1)
//								if (_slots[grandParent]._left == parent)
//									_slots[grandParent]._left = newGrandParent;
//								else
//									_slots[grandParent]._right = newGrandParent;
//							else
//								_rootSlotIndex = newGrandParent;
//							if (parent == match)
//								parentOfMatch = newGrandParent;
//						}
//					}
//				}
//				int order = foundMatch ? -0x1 : _inputElementComparator.Compare(input, _slots[current]._input);
//				if (order == 0x0)
//				{
//					foundMatch = true;
//					match = current;
//					parentOfMatch = parent;
//				}
//				grandParent = parent;
//				parent = current;
//				current = order < 0x0 ? _slots[current]._left : _slots[current]._right;
//			}
//			if (foundMatch)
//			{
//				if (parent == match)
//				{
//					Debug.Assert(_slots[match]._right == -0x1);
//					parent = _slots[match]._left;
//				}
//				else
//				{
//					Debug.Assert(grandParent != -0x1);
//					Debug.Assert(_slots[parent]._left == -0x1);
//					Debug.Assert(_slots[parent]._right == -0x1 && _slots[parent]._isRed || _slots[_slots[parent]._right]._isRed && !_slots[parent]._isRed);
//					if (_slots[parent]._right != -0x1)
//						_slots[_slots[parent]._right]._isRed = false;
//					if (grandParent != match)
//					{
//						_slots[grandParent]._left = _slots[parent]._right;
//						_slots[parent]._right = _slots[match]._right;
//					}
//					_slots[parent]._left = _slots[match]._left;
//				}
//				if (parent != -0x1)
//					_slots[parent]._isRed = _slots[match]._isRed;
//				if (parentOfMatch != -0x1)
//					if (_slots[parentOfMatch]._left == match)
//						_slots[parentOfMatch]._left = parent;
//					else
//						_slots[parentOfMatch]._right = parent;
//				else
//					_rootSlotIndex = parent;
//				output = _slots[match]._output;
//				FreeNode(match);
//			}
//			else
//				output = default;
//			if (_rootSlotIndex != -0x1)
//				_slots[_rootSlotIndex]._isRed = false;
//			if (!foundMatch)
//				throw new ArgumentException(SurjectionHelper.GetSurjectionDoesNotContainRelationWithInputExceptionMessage(this, nameof(input)));
//		}
//		/// <summary>
//		/// Removes a relation with a specified input element from the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> if the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> contains a relation with the specified input element.
//		/// </summary>
//		/// <param name="input">The input element of the relation.</param>
//		/// <param name="output">The output element of the relation if the relation is removed; otherwise, the default value.</param>
//		/// <returns><see langword="true"/> whether the relation is removed; otherwise, <see langword="false"/>.</returns>
//		public bool TryRemove(TInput input, out TOutput output)
//		{
//			if (_rootSlotIndex == -0x1)
//			{
//				output = default;
//				return false;
//			}
//			int current = _rootSlotIndex;
//			int parent = -0x1;
//			int grandParent = -0x1;
//			int match = -0x1;
//			int parentOfMatch = -0x1;
//			bool foundMatch = false;
//			while (current != -0x1)
//			{
//				if (current != -0x1 && !_slots[current]._isRed && (_slots[current]._left == -0x1 || !_slots[_slots[current]._left]._isRed) && (_slots[current]._right == -0x1 || !_slots[_slots[current]._right]._isRed))
//				{
//					if (parent == -0x1)
//						_slots[current]._isRed = true;
//					else
//					{
//						int sibling = _slots[parent]._left == current ? _slots[parent]._right : _slots[parent]._left;
//						if (_slots[sibling]._isRed)
//						{
//							Debug.Assert(!_slots[parent]._isRed);
//							_ = _slots[parent]._right == sibling ? RotateLeft(_slots, parent) : RotateRight(_slots, parent);
//							_slots[parent]._isRed = true;
//							_slots[sibling]._isRed = false;
//							if (grandParent != -0x1)
//								if (_slots[grandParent]._left == parent)
//									_slots[grandParent]._left = sibling;
//								else
//									_slots[grandParent]._right = sibling;
//							else
//								_rootSlotIndex = sibling;
//							grandParent = sibling;
//							if (parent == match)
//								parentOfMatch = sibling;
//							sibling = (_slots[parent]._left == current) ? _slots[parent]._right : _slots[parent]._left;
//						}
//						Debug.Assert(sibling != -0x1 || _slots[sibling]._isRed == false);
//						if (sibling != -0x1 && !_slots[sibling]._isRed && (_slots[sibling]._left == -0x1 || !_slots[_slots[sibling]._left]._isRed) && (_slots[sibling]._right == -0x1 || !_slots[_slots[sibling]._right]._isRed))
//						{
//							Debug.Assert(parent != -0x1 && _slots[parent]._isRed);
//							_slots[parent]._isRed = false;
//							_slots[current]._isRed = true;
//							_slots[sibling]._isRed = true;
//						}
//						else
//						{
//							Debug.Assert(_slots[sibling]._left != -0x1 && _slots[_slots[sibling]._left]._isRed || _slots[sibling]._right != -0x1 && _slots[_slots[sibling]._right]._isRed, "sibling must have at least one red child!");
//							int newGrandParent;
//							if (_slots[sibling]._left != -0x1 && _slots[_slots[sibling]._left]._isRed)
//							{
//								if (_slots[parent]._left == current)
//								{
//									Debug.Assert(_slots[parent]._right == sibling);
//									Debug.Assert(_slots[_slots[sibling]._left]._isRed);
//									newGrandParent = RotateRightLeft(_slots, parent);
//								}
//								else
//								{
//									Debug.Assert(_slots[parent]._left == sibling);
//									Debug.Assert(_slots[_slots[sibling]._left]._isRed);
//									_slots[_slots[sibling]._left]._isRed = false;
//									newGrandParent = RotateRight(_slots, parent);
//								}
//							}
//							else
//							{
//								if (_slots[parent]._left == current)
//								{
//									Debug.Assert(_slots[parent]._right == sibling);
//									Debug.Assert(_slots[_slots[sibling]._right]._isRed);
//									_slots[_slots[sibling]._right]._isRed = false;
//									newGrandParent = RotateLeft(_slots, parent);
//								}
//								else
//								{
//									Debug.Assert(_slots[parent]._left == sibling);
//									Debug.Assert(_slots[_slots[sibling]._right]._isRed);
//									newGrandParent = RotateLeftRight(_slots, parent);
//								}
//							}
//							_slots[newGrandParent]._isRed = _slots[parent]._isRed;
//							_slots[parent]._isRed = false;
//							_slots[current]._isRed = true;
//							if (grandParent != -0x1)
//								if (_slots[grandParent]._left == parent)
//									_slots[grandParent]._left = newGrandParent;
//								else
//									_slots[grandParent]._right = newGrandParent;
//							else
//								_rootSlotIndex = newGrandParent;
//							if (parent == match)
//								parentOfMatch = newGrandParent;
//						}
//					}
//				}
//				int order = foundMatch ? -0x1 : _inputElementComparator.Compare(input, _slots[current]._input);
//				if (order == 0x0)
//				{
//					foundMatch = true;
//					match = current;
//					parentOfMatch = parent;
//				}
//				grandParent = parent;
//				parent = current;
//				current = order < 0x0 ? _slots[current]._left : _slots[current]._right;
//			}
//			if (foundMatch)
//			{
//				if (parent == match)
//				{
//					Debug.Assert(_slots[match]._right == -0x1);
//					parent = _slots[match]._left;
//				}
//				else
//				{
//					Debug.Assert(grandParent != -0x1);
//					Debug.Assert(_slots[parent]._left == -0x1);
//					Debug.Assert(_slots[parent]._right == -0x1 && _slots[parent]._isRed || _slots[_slots[parent]._right]._isRed && !_slots[parent]._isRed);
//					if (_slots[parent]._right != -0x1)
//						_slots[_slots[parent]._right]._isRed = false;
//					if (grandParent != match)
//					{
//						_slots[grandParent]._left = _slots[parent]._right;
//						_slots[parent]._right = _slots[match]._right;
//					}
//					_slots[parent]._left = _slots[match]._left;
//				}
//				if (parent != -0x1)
//					_slots[parent]._isRed = _slots[match]._isRed;
//				if (parentOfMatch != -0x1)
//					if (_slots[parentOfMatch]._left == match)
//						_slots[parentOfMatch]._left = parent;
//					else
//						_slots[parentOfMatch]._right = parent;
//				else
//					_rootSlotIndex = parent;
//				output = _slots[match]._output;
//				FreeNode(match);
//			}
//			else
//				output = default;
//			if (_rootSlotIndex != -0x1)
//				_slots[_rootSlotIndex]._isRed = false;
//			return foundMatch;
//		}
//		/// <summary>
//		/// Removes all items from the <see cref="RedBlackBinaryTreeSurjection{TKey, TValue}"/>.
//		/// </summary>
//		public void Clear()
//		{
//			Array.Clear(_slots, 0x0, _usedSlotCount);
//			_count = 0x0;
//			_usedSlotCount = 0x0;
//			_freeSlotIndex = -0x1;
//			_rootSlotIndex = -0x1;
//		}
//		/// <summary>
//		/// Handles each relation of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> from a specified input element with a specified <see cref="IElementHandler{T}"/>.
//		/// </summary>
//		/// <param name="handler">The handler of the relations.</param>
//		/// <param name="start">The starting element.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
//		public void HandleEachFrom(IElementHandler<Relation<TInput, TOutput>> handler, TInput start)
//		{
//			if (handler == null)
//				throw new ArgumentNullException(nameof(handler));
//			Queue<int> stack = RedBlackBinaryTreeEnumeratorStackBuffer.Allocate();
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				if (inputElementComparator.Compare(slots[slotIndex]._input, start) < 0x0)
//				{
//					slotIndex = slots[slotIndex]._right;
//					continue;
//				}
//				stack.EnqueueBack(slotIndex);
//				slotIndex = slots[slotIndex]._left;
//			}
//			try
//			{
//				while (stack.Count != 0x0)
//				{
//					if (handler.Handle(new Relation<TInput, TOutput>(slots[slotIndex = stack.Dequeue()]._input, slots[slotIndex]._output)))
//						return;
//					for (slotIndex = slots[slotIndex]._right; slotIndex != -0x1; slotIndex = slots[slotIndex]._left)
//						stack.EnqueueBack(slotIndex);
//				}
//			}
//			finally { RedBlackBinaryTreeEnumeratorStackBuffer.Free(stack); }
//		}
//		/// <summary>
//		/// Handles each relation of the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> to a specified input element with a specified <see cref="IElementHandler{T}"/>.
//		/// </summary>
//		/// <param name="handler">The handler of the relations.</param>
//		/// <param name="start">The finite element.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="handler"/> is <see langword="null"/>.</exception>
//		public void HandleEachTo(IElementHandler<Relation<TInput, TOutput>> handler, TInput start)
//		{
//			if (handler == null)
//				throw new ArgumentNullException(nameof(handler));
//			Queue<int> stack = RedBlackBinaryTreeEnumeratorStackBuffer.Allocate();
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			while (slotIndex != -0x1)
//			{
//				if (inputElementComparator.Compare(slots[slotIndex]._input, start) > 0x0)
//				{
//					slotIndex = slots[slotIndex]._left;
//					continue;
//				}
//				stack.EnqueueBack(slotIndex);
//				slotIndex = slots[slotIndex]._right;
//			}
//			try
//			{
//				while (stack.Count != 0x0)
//				{
//					if (handler.Handle(new Relation<TInput, TOutput>(slots[slotIndex = stack.Dequeue()]._input, slots[slotIndex]._output)))
//						return;
//					for (slotIndex = slots[slotIndex]._left; slotIndex != -0x1; slotIndex = slots[slotIndex]._right)
//						stack.EnqueueBack(slotIndex);
//				}
//			}
//			finally { RedBlackBinaryTreeEnumeratorStackBuffer.Free(stack); }
//		}
//		/// <summary>
//		/// Determines the relation which input element is maximum and is not greater than a specified threshold.
//		/// </summary>
//		/// <param name="threshold">The threshold.</param>
//		/// <param name="input">The input element of the relation if the relation is found; otherwise, the default value.</param>
//		/// <param name="output">The output element of the relation if the relation is found; otherwise, the default value.</param>
//		/// <returns><see langword="true"/> if the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> contains the relation; otherwise, <see langword="false"/>.</returns>
//		public bool TryGetMax(TInput threshold, out TInput input, out TOutput output)
//		{
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			int lastSlotIndex = -0x1;
//			while (slotIndex != -0x1)
//			{
//				int compareResult = inputElementComparator.Compare(threshold, slots[slotIndex]._input);
//				if (compareResult < 0x0)
//				{
//					lastSlotIndex = slotIndex;
//					slotIndex = slots[slotIndex]._left;
//					continue;
//				}
//				if (compareResult > 0x0)
//				{
//					slotIndex = slots[slotIndex]._right;
//					continue;
//				}
//				input = slots[slotIndex]._input;
//				output = slots[slotIndex]._output;
//				return true;
//			}
//			if (lastSlotIndex == -0x1)
//			{
//				input = default;
//				output = default;
//				return false;
//			}
//			input = slots[lastSlotIndex]._input;
//			output = slots[lastSlotIndex]._output;
//			return true;
//		}
//		/// <summary>
//		/// Determines the relation which input element is minimum and is not less than a specified threshold.
//		/// </summary>
//		/// <param name="threshold">The threshold.</param>
//		/// <param name="input">The input element of the relation if the relation is found; otherwise, the default value.</param>
//		/// <param name="output">The output element of the relation if the relation is found; otherwise, the default value.</param>
//		/// <returns><see langword="true"/> if the <see cref="RedBlackBinaryTreeSurjection{TInput, TOutput}"/> contains the relation; otherwise, <see langword="false"/>.</returns>
//		public bool TryGetMin(TInput threshold, out TInput input, out TOutput output)
//		{
//			IBinaryComparator<TInput> inputElementComparator = _inputElementComparator;
//			Slot[] slots = _slots;
//			int slotIndex = _rootSlotIndex;
//			int lastSlotIndex = -0x1;
//			while (slotIndex != -0x1)
//			{
//				int compareResult = inputElementComparator.Compare(threshold, slots[slotIndex]._input);
//				if (compareResult < 0x0)
//				{
//					slotIndex = slots[slotIndex]._left;
//					continue;
//				}
//				if (compareResult > 0x0)
//				{
//					lastSlotIndex = slotIndex;
//					slotIndex = slots[slotIndex]._right;
//					continue;
//				}
//				input = slots[slotIndex]._input;
//				output = slots[slotIndex]._output;
//				return true;
//			}
//			if (lastSlotIndex == -0x1)
//			{
//				input = default;
//				output = default;
//				return false;
//			}
//			input = slots[lastSlotIndex]._input;
//			output = slots[lastSlotIndex]._output;
//			return true;
//		}
//	}
//}