using GA.Collections;
using Xunit;

public class LinkedListTests
{
	[Fact]
	public void TestBasicAdd()
	{
		GA.Collections.LinkedList<int> list = new GA.Collections.LinkedList<int>();
		list.Add(1);
		list.Add(6);
		list.Add(-1);

		Assert.Equal(3, list.Count);
	}

	[Fact]
	public void TestRemoveHead()
	{
		GA.Collections.LinkedList<int> list = new GA.Collections.LinkedList<int>();
		list.Add(1);
		list.Add(6);
		list.Add(-1);

		bool removed = list.Remove(1);

		Assert.True(removed);
		Assert.True(list.First() == 6);
		Assert.Equal(2, list.Count);
	}

	[Fact]
	public void TestRemoveTail()
	{
		GA.Collections.LinkedList<int> list = new GA.Collections.LinkedList<int>();
		list.Add(1);
		list.Add(6);
		list.Add(-1);

		bool removed = list.Remove(-1);

		Assert.True(removed);
		Assert.True(list.Last() == 6);
		Assert.Equal(2, list.Count);
	}

	[Fact]
	public void TestRemoveAnyOther()
	{
		GA.Collections.LinkedList<int> list = new GA.Collections.LinkedList<int>();
		list.Add(1);
		list.Add(6);
		list.Add(-1);

		bool removed = list.Remove(6);

		Assert.True(removed);
		Assert.True(list.First() == 1);
		Assert.True(list.Last() == -1);
		Assert.Equal(2, list.Count);
	}

	[Fact]
	public void TestRemoveOnlyElement()
	{
		GA.Collections.LinkedList<int> list = new GA.Collections.LinkedList<int>();
		list.Add(1);

		bool removed = list.Remove(1);

		Assert.True(removed);
		Assert.Empty(list);
	}

	[Fact]
	public void TestRemoveFalseElement()
	{
		GA.Collections.LinkedList<int> list = new GA.Collections.LinkedList<int>();
		list.Add(1);
		list.Add(6);
		list.Add(-1);

		bool removed = list.Remove(7);

		Assert.False(removed);
		Assert.Equal(3, list.Count);
	}

	[Fact]
	public void TestBasicClear()
	{
		GA.Collections.LinkedList<int> list = new GA.Collections.LinkedList<int>();
		list.Add(1);
		list.Add(6);
		list.Add(-1);

		list.Clear();

		Assert.Empty(list);
	}
}
