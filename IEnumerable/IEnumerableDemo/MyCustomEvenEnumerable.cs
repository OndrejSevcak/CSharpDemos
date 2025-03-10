using System.Collections;
using System.Collections.Generic;

namespace IEnumerableDemo;

//Custom implementation of IEnumerable
public class MyCustomEvenEnumerable : IEnumerable<int>
{
    private MyCustomEvenEnumerator _enumerator;

    public MyCustomEvenEnumerable(int[] data)
    {
        _enumerator = new MyCustomEvenEnumerator(data);
    }

    public IEnumerator<int> GetEnumerator()
    {
        return _enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
