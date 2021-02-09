using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ReadOnlyList<T> : IEnumerable<T>
{
    private T[] _data;

    public ReadOnlyList(IEnumerable<T> enumerable)
    {
        _data = enumerable.ToArray();
    }

    public T this[int index]
    {
        get
        {
            return _data[index];
        }
    }

    public int Count
    {
        get
        {
            return _data.Length;
        }
    }

    public bool Contains(T value)
    {
        return _data.Contains(value);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _data.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _data.GetEnumerator();
    }
}
