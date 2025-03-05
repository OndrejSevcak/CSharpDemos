// IEnumerable demo

using System.Collections;

//creating basic IEnumerable collection
IEnumerable<int> collection = [1, 2, 3, 4, 5];  

//iterating it using foreach
foreach (int item in collection)
{
    Console.WriteLine(item);
}

//iterating it using enumerator
IEnumerator<int> enumerator = collection.GetEnumerator();
while (enumerator.MoveNext())
{
    Console.WriteLine(enumerator.Current);
}
//Note: the first call to MoveNext() method moves the cursor to the first element of the collection

//creating custom IEnumerable collection
MyCustomEvenEnumerable customCollection = new MyCustomEvenEnumerable(new int[] { 1, 2, 3, 4, 5 });

//iterating custom collection using foreach and enumerator
IEnumerator<int> customEnumerator = customCollection.GetEnumerator();
while (customEnumerator.MoveNext())
{
    Console.WriteLine(customEnumerator.Current);
}

//iterating custom collection using foreach
foreach (int item in customCollection)
{
    Console.WriteLine(item);
}

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
