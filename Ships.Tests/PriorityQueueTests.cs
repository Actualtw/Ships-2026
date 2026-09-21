using GA.Collections;
using Xunit;

public class PriorityQueueTests
{
    [Fact]
    public void NewQueueTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        Assert.Equal(0, queue.Count);
        Assert.True(queue.IsConsistent());
    }

    [Fact]
    public void EnqueueTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Enqueue(6);
        queue.Enqueue(1);
        queue.Enqueue(3);

        Assert.Equal(3, queue.Count);
        Assert.True(queue.IsConsistent());
    }

    [Fact]
    public void PeekAndOrderTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Enqueue(6);
        queue.Enqueue(1);
        queue.Enqueue(3);

        Assert.Equal(1, queue.Peek());
        Assert.True(queue.IsConsistent());
    }

    [Fact]
    public void DequeueTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Enqueue(6);
        queue.Enqueue(1);
        queue.Enqueue(3);

        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(6, queue.Dequeue());
        Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void ClearQueueTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Enqueue(6);
        queue.Enqueue(1);
        queue.Enqueue(3);

        queue.Clear();

        Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void ClearEmptyQueueTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Clear();

        Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void EnqueueOneElementTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Enqueue(52);

        Assert.Equal(1, queue.Count);
        Assert.Equal(52, queue.Peek());
        Assert.True(queue.IsConsistent());
    }

    [Fact]
    public void DequeueOneElementTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Enqueue(52);

        Assert.Equal(52, queue.Dequeue());
        Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void DequeueEmptyQueueTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
    }

    [Fact]
    public void PeekEmptyQueueTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        Assert.Throws<InvalidOperationException>(() => queue.Peek());
    }

    [Fact]
    public void DuplicateValuesTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Enqueue(3);
        queue.Enqueue(3);
        queue.Enqueue(1);
        queue.Enqueue(3);
        queue.Enqueue(1);

        Assert.Equal(5, queue.Count);
        Assert.True(queue.IsConsistent());

        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void NegativeValuesTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Enqueue(-2);
        queue.Enqueue(6);
        queue.Enqueue(1);
        queue.Enqueue(-3);
        queue.Enqueue(-15);
        queue.Enqueue(0);

        Assert.True(queue.IsConsistent());

        Assert.Equal(-15, queue.Dequeue());
        Assert.Equal(-3, queue.Dequeue());
        Assert.Equal(-2, queue.Dequeue());
        Assert.Equal(0, queue.Dequeue());
        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(6, queue.Dequeue());
    }

    [Fact]
    public void ContainsTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Enqueue(6);
        queue.Enqueue(1);
        queue.Enqueue(3);

        Assert.True(queue.Contains(6));
        Assert.True(queue.Contains(1));
        Assert.True(queue.Contains(3));
    }

    [Fact]
    public void ContainsFalseTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Enqueue(6);
        queue.Enqueue(1);
        queue.Enqueue(3);

        Assert.False(queue.Contains(52));
    }

    [Fact]
    public void ContainsAfterRemovalTest()
    {
        PriorityQueue<int> queue = new PriorityQueue<int>();

        queue.Enqueue(6);
        queue.Enqueue(1);
        queue.Enqueue(3);

        Assert.True(queue.Contains(1));

        queue.Dequeue();

        Assert.False(queue.Contains(1));
        Assert.True(queue.Contains(3));
        Assert.True(queue.Contains(6));
        Assert.Equal(2, queue.Count);
    }
}
