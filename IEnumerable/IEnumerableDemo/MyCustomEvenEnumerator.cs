using System;
using System.Collections;

namespace IEnumerableDemo;

//custom implementation of Enumerator
public class MyCustomEvenEnumerator : IEnumerator<int>
{
    private int[] _data;
    private int _currentIndex = -1;

    public MyCustomEvenEnumerator(int[] data)
    {
        _data = data;
    }

    public int Current => _data[_currentIndex];
    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        _currentIndex++;
        if(_currentIndex >= _data.Length)
        {
            return false;
        }

        if(_data[_currentIndex] % 2 != 0)
        {
            return MoveNext();
        }

        return _currentIndex < _data.Length;
    }

    public void Reset()
    {
        _currentIndex = -1;
    }

    public void Dispose(){}
}