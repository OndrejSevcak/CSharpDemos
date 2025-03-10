// IEnumerable demo

using System.Collections;
using IEnumerableDemo;

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


//yield return - deffered execution demo
IEnumerable<int> GetNumbersOneByOne()
{
    yield return 1;
    yield return 3;
    yield return 2;
    yield return 4;
}

var numbers = GetNumbersOneByOne();

foreach (var number in numbers) 
{
  Console.WriteLine(number);
  // Output:
  // 1
  // 3
  // 2
  //4
}

//infinte sequence using yield return
IEnumerable<int> GenerateNumbers()
{
    int i = 1;
    while(true){
        yield return i++;
    }
}

var infiniteNumbers = GenerateNumbers();
foreach (var number in infiniteNumbers) 
{
  if(number > 10) break;
  Console.WriteLine(number);
}

//filtering data
IEnumerable<int> GetEvenNumbers(IEnumerable<int> numbers)
{
    foreach (var num in numbers)
    {
        if (num % 2 == 0)
            yield return num;
    }
}

var evenNumbers = GetEvenNumbers(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
foreach (var number in evenNumbers) 
{
  Console.WriteLine(number);
  // Output:
  // 2
  // 4
  // 6
  // 8
  // 10
}

//processing large text file line by line
IEnumerable<string> ReadLargeFile(string filePath)
{
    using (StreamReader reader = new StreamReader(filePath))
    {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }
}

