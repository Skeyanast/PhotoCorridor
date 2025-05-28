using System.Collections;
using System.Collections.Generic;

public class CircularDoublyLinkedList<T> : IEnumerable<T>
{
    private DoublyNode<T> _head;
    private int _count;

    public int Count => _count;
    public bool IsEmpty => _count == 0;

    public void Add(T data)
    {
        DoublyNode<T> node = new DoublyNode<T>(data);

        if (_head == null)
        {
            _head = node;
            _head.Next = node;
            _head.Previous = node;
        }
        else
        {
            node.Previous = _head.Previous;
            node.Next = _head;
            _head.Previous.Next = node;
            _head.Previous = node;
        }

        _count++;
    }

    public bool Remove(T data)
    {
        DoublyNode<T> current = _head;

        DoublyNode<T> removedItem = null;
        if (_count == 0)
        {
            return false;
        }

        do
        {
            if (current.Data.Equals(data))
            {
                removedItem = current;
                break;
            }
            current = current.Next;
        }
        while (current != _head);

        if (removedItem != null)
        {
            if (_count == 1)
            {
                _head = null;
            }
            else
            {
                if (removedItem == _head)
                {
                    _head = _head.Next;
                }
                removedItem.Previous.Next = removedItem.Next;
                removedItem.Next.Previous = removedItem.Previous;
            }

            _count--;
            return true;
        }
        return false;
    }

    public void Clear()
    {
        _head = null;
        _count = 0;
    }

    public bool Contains(T data)
    {
        DoublyNode<T> current = _head;

        if (current == null)
        {
            return false;
        }

        do
        {
            if (current.Data.Equals(data))
            {
                return true;
            }
            current = current.Next;
        }
        while(current != _head);

        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)this).GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        DoublyNode<T> current = _head;

        do
        {
            if (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        while (current != _head);
    }
}
