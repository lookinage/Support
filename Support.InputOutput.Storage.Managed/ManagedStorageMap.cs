//using Support.Collections.Generic;
//using System;
//using System.Collections.Generic;

//namespace Support.InputOutput.Storage.Managed
//{
//	/// <summary>
//	/// Represents a map of segments of a managed storage.
//	/// </summary>
//	public class ManagedStorageMap
//	{
//		private readonly RedBlackTreeDictionary<long, long> _allocatedSegmentSizeByStart;
//		private readonly RedBlackTreeCollection<long, long> _freedSegmentStartBySize;

//		/// <summary>
//		/// Initializes the <see cref="ManagedStorageMap"/>.
//		/// </summary>
//		public ManagedStorageMap()
//		{
//			_allocatedSegmentSizeByStart = new RedBlackTreeDictionary<long, long>();
//			_freedSegmentStartBySize = new RedBlackTreeCollection<long, long>();
//		}

//		/// <summary>
//		/// Gets the capacity of the storage.
//		/// </summary>
//		public long Capacity => _allocatedSegmentSizeByStart.TryGetFirstOnRay(true, long.MaxValue, out long start, out long size) ? start + size : 0x0;
//		/// <summary>
//		/// Gets the amount of allocated segments.
//		/// </summary>
//		public long AllocatedAmount
//		{
//			get
//			{
//				long allocatedAmount = 0x0;
//				foreach (KeyValuePair<long, long> pair in _allocatedSegmentSizeByStart)
//					allocatedAmount += pair.Value;
//				return allocatedAmount;
//			}
//		}
//		/// <summary>
//		/// Gets the amount of freed segments.
//		/// </summary>
//		public long FreedAmount
//		{
//			get
//			{
//				long freedAmount = 0x0;
//				foreach (KeyValuePair<long, long> pair in _freedSegmentStartBySize)
//					freedAmount += pair.Key;
//				return freedAmount;
//			}
//		}
//		/// <summary>
//		/// Gets the number of allocated segments of the storage.
//		/// </summary>
//		public int AllocatedCount => _allocatedSegmentSizeByStart.Count;
//		/// <summary>
//		/// Gets the number of freed segments of the storage.
//		/// </summary>
//		public int FreedCount => _freedSegmentStartBySize.Count;

//		/// <summary>
//		/// Occurs when a segment is moved.
//		/// </summary>
//		public event EventTracker<ManagedStorageMap, SegmentMovedEventArgs> Moved;

//		private void TryReallocate(long start, long size)
//		{
//			_allocatedAmount += size;
//			if (_allocatedSegmentSizeByStart.TryGetFirstOnRay(true, start, out long segmentStart, out long segmentSize) && segmentStart + segmentSize == start)
//			{
//				_allocatedSegmentSizeByStart[segmentStart] = segmentSize + size;
//				return;
//			}
//			_allocatedSegmentSizeByStart.Add(start, size);
//		}
//		/// <summary>
//		/// Allocates a new segment in the storage.
//		/// </summary>
//		/// <param name="size">The size of the segment.</param>
//		/// <returns>The start of the allocated segment.</returns>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is not positive.</exception>
//		/// <exception cref="ArgumentException">There is not enough space to allocate.</exception>
//		public long AllocateSegment(long size)
//		{
//			if (size <= 0x0)
//				throw new ArgumentOutOfRangeException(nameof(size));
//			if (_freedSegmentStartBySize.TryGetValue(size, out long start))
//			{
//				TryReallocate(start, size);
//				_ = _freedSegmentStartBySize.Remove(size, start);
//				return start;
//			}
//			if (_freedSegmentStartBySize.TryGetFirstOnRay(true, long.MaxValue, out long freeSegmentSize, out start) && freeSegmentSize > size)
//			{
//				TryReallocate(start, size);
//				try { return start; }
//				finally
//				{
//					_ = _freedSegmentStartBySize.Remove(freeSegmentSize, start);
//					_freedSegmentStartBySize.Add(freeSegmentSize - size, start + size);
//				}
//			}
//			start = Capacity;
//			try { checked { _capacity += size; } }
//			catch (OverflowException exception) { throw new ArgumentException("There is not enough space to allocate.", exception); }
//			TryReallocate(start, size);
//			return start;
//		}
//		/// <summary>
//		/// Frees a specified segment of the storage.
//		/// </summary>
//		/// <param name="start">The start of the segment.</param>
//		/// <param name="size">The size of the segment.</param>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/> is less than 0.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is not positive.</exception>
//		/// <exception cref="ArgumentException"><paramref name="size"/> is greater than the number of space from <paramref name="start"/> to the end of the capacity of the storage.</exception>
//		/// <exception cref="ArgumentException">The specified segment includes one or more freed segments.</exception>
//		public void FreeSegment(long start, long size)
//		{
//			if (start < 0x0)
//				throw new ArgumentOutOfRangeException(nameof(start));
//			if (size <= 0x0)
//				throw new ArgumentOutOfRangeException(nameof(size));
//			if (size > _capacity - start)
//				throw new ArgumentException(string.Format("{0} is greater than the number of space from {1} to the end of the capacity of the storage.", nameof(size), nameof(start)));
//			long end;
//			long segmentEnd;
//			if (!_allocatedSegmentSizeByStart.TryGetFirstOnRay(true, start, out long segmentStart, out long segmentSize) || (end = start + size) > (segmentEnd = segmentStart + segmentSize))
//				throw new ArgumentException("The specified segment includes one or more freed segments.");
//			if (end == _capacity)
//				_capacity -= size;
//			_allocatedAmount -= size;
//			long neighbourSegmentStart;
//			long freeSegmentSize;
//			if (segmentStart == start)
//			{
//				if (_allocatedSegmentSizeByStart.TryGetFirstOnRay(true, segmentStart - 0x1, out neighbourSegmentStart, out long neighbourSegmentSize))
//				{
//					start = neighbourSegmentStart + neighbourSegmentSize;
//					freeSegmentSize = segmentStart - start;
//					size += freeSegmentSize;
//					_ = _freedSegmentStartBySize.Remove(freeSegmentSize, start);
//				}
//				_ = _allocatedSegmentSizeByStart.Remove(segmentStart);
//			}
//			else
//				_allocatedSegmentSizeByStart[segmentStart] = start - segmentStart;
//			if (segmentEnd == end)
//			{
//				if (_allocatedSegmentSizeByStart.TryGetFirstOnRay(false, segmentStart + 0x1, out neighbourSegmentStart, out _))
//				{
//					freeSegmentSize = neighbourSegmentStart - end;
//					size += freeSegmentSize;
//					_ = _freedSegmentStartBySize.Remove(freeSegmentSize, segmentEnd);
//				}
//			}
//			else
//				_allocatedSegmentSizeByStart.Add(end, segmentEnd - end);
//			_freedSegmentStartBySize.Add(size, start);
//		}
//		/// <summary>
//		/// Marks a specified segment of the storage as allocated.
//		/// </summary>
//		/// <param name="start">The start of the segment.</param>
//		/// <param name="size">The size of the segment.</param>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/> is less than 0.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is not positive.</exception>
//		/// <exception cref="ArgumentException"><paramref name="size"/> is greater than the number of space from <paramref name="start"/> to the end of the storage.</exception>
//		public void Allocate(long start, long size)
//		{
//			if (start < 0x0)
//				throw new ArgumentOutOfRangeException(nameof(start));
//			if (size < 0x0)
//				throw new ArgumentOutOfRangeException(nameof(size));
//			if (size > long.MaxValue - start)
//				throw new ArgumentException(string.Format("{0} is greater than the number of space from {1} to the end of the storage.", nameof(size), nameof(start)));
//			if (size == 0x0)
//				return;
//			long end = start + size;
//			if (end > _capacity)
//				_capacity = end;
//			long segmentEnd = 0x0;
//			long intersection;
//			if (_allocatedSegmentSizeByStart.TryGetFirstOnRay(true, start, out long segmentStart, out long segmentSize) && (segmentEnd = segmentStart + segmentSize) >= start)
//			{
//				if (segmentEnd >= end)
//					return;
//				intersection = segmentEnd - start;
//				segmentSize += size - intersection;
//				_allocatedAmount += size - intersection;
//			}
//			else
//			{
//				segmentStart = start;
//				segmentSize = size;
//				_allocatedAmount += size;
//				_allocatedSegmentSizeByStart.Add(segmentStart, segmentSize);
//			}
//			for (; ; )
//			{
//				if (!_allocatedSegmentSizeByStart.TryGetFirstOnRay(false, start + 0x1, out long nextSegmentStart, out long nextSegmentSize) || nextSegmentStart > end)
//					break;
//				_ = _allocatedSegmentSizeByStart.Remove(nextSegmentStart);
//				long nextSegmentEnd;
//				if ((nextSegmentEnd = nextSegmentStart + nextSegmentSize) > end)
//				{
//					intersection = end - nextSegmentStart;
//					segmentSize += nextSegmentSize - intersection;
//					_allocatedAmount -= intersection;
//					break;
//				}
//				else
//				{
//					_ = _freedSegmentStartBySize.Remove(nextSegmentStart - segmentEnd, segmentEnd);
//					segmentEnd = nextSegmentEnd;
//				}
//				_allocatedAmount -= nextSegmentSize;
//			}
//			_allocatedSegmentSizeByStart[segmentStart] = segmentSize;
//		}
//		/// <summary>
//		/// Tests whether a specified segment is inside a segment of the storage.
//		/// </summary>
//		/// <param name="start">The start of the segment.</param>
//		/// <param name="end">The end of the segment.</param>
//		/// <returns><see langword="true"/> whether the specified segment is inside a segment of the storage; otherwise, <see langword="false"/>.</returns>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="start"/> is less than 0.</exception>
//		/// <exception cref="ArgumentOutOfRangeException"><paramref name="end"/> is less than 0.</exception>
//		/// <exception cref="ArgumentException"><paramref name="end"/> is less than <paramref name="start"/>.</exception>
//		public bool Full(long start, long end)
//		{
//			if (start < 0)
//				throw new ArgumentOutOfRangeException(nameof(start));
//			if (end < 0)
//				throw new ArgumentOutOfRangeException(nameof(end));
//			if (end < start)
//				throw new ArgumentException(string.Format("{0} is less than {1}.", nameof(end), nameof(start)));
//			foreach (KeyValuePair<long, long> pair in _allocatedSegmentSizeByStart.GetFromRay(true, end))
//			{
//				long segmentStart = pair.Key;
//				long segmentEnd = segmentStart + pair.Value;
//				if (segmentEnd <= start)
//					break;
//				if (segmentEnd < end)
//					return false;
//				end = segmentStart;
//			}
//			return end <= start;
//		}
//		/// <summary>
//		/// Removes freed segments by the moving of the allocated segments to the start of the storage.
//		/// </summary>
//		public void Defragment()
//		{
//			long allocatedAmount = 0x0;
//			long end = 0x0;
//			long offset = 0x0;
//			foreach (KeyValuePair<long, long> pair in _allocatedSegmentSizeByStart)
//			{
//				long start = pair.Key;
//				long size = pair.Value;
//				allocatedAmount += size;
//				Moved?.Invoke(this, new SegmentMovedEventArgs(start, size, offset += start - end));
//				end = start + size;
//			}
//			_allocatedSegmentSizeByStart.Clear();
//			_freedSegmentStartBySize.Clear();
//			_allocatedSegmentSizeByStart.Add(0x0, allocatedAmount);
//		}
//	}
//}