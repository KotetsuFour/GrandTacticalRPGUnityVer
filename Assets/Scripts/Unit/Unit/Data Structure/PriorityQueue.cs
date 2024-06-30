using System;
public class PriorityQueue<E> where E : IComparable
{

	private QueueNode start;
	private QueueNode end;
	private int listSize;

	public PriorityQueue()
	{
		start = new QueueNode();
		end = new QueueNode();
		start.setNext(end);
		end.setPrev(start);
		listSize = 0;
	}

	public void add(E element)
	{
		if (element == null)
		{
			throw new Exception("Cannot add a null pointer to a priority queue");
		}
		QueueNode idx = start.getNext();
		while (idx.getElement() != null && idx.getElement().CompareTo(element) > 0)
		{
			idx = idx.getNext();
		}
		new QueueNode(element, idx.getPrev(), idx);
		listSize++;
	}
	public E pop()
	{
		if (isEmpty())
		{
			throw new Exception("Queue is empty and cannot pop first item");
		}
		E ret = start.getNext().getElement();
		start.setNext(start.getNext().getNext());
		start.getNext().setPrev(start);
		listSize--;
		return ret;
	}
	public E get(int idx)
	{
		if (isEmpty())
		{
			throw new Exception("Queue is empty and cannot return an element at an index");
		}
		QueueNode check = start.getNext();
		while (idx > 0)
		{
			if (check.getElement() == null)
			{
				throw new Exception("Attempted to access an index that doesn't exist");
			}
			check = check.getNext();
			idx--;
		}
		return check.getElement();
	}

	public int size()
	{
		return listSize;
	}
	public bool isEmpty()
	{
		return size() == 0;
	}

	private class QueueNode
	{

		private QueueNode prev;
		private QueueNode next;
		private E element;

		public QueueNode()
        {
		}
		public QueueNode(E element, QueueNode prev, QueueNode next)
		{
			setElement(element);
			setPrev(prev);
			setNext(next);
			if (prev != null)
			{
				prev.setNext(this);
			}
			if (next != null)
			{
				next.setPrev(this);
			}
		}

		public QueueNode getPrev()
		{
			return prev;
		}
		public void setPrev(QueueNode prev)
		{
			this.prev = prev;
		}
		public QueueNode getNext()
		{
			return next;
		}
		public void setNext(QueueNode next)
		{
			this.next = next;
		}
		public E getElement()
		{
			return element;
		}
		public void setElement(E element)
		{
			this.element = element;
		}
	}

}
