using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// iterace hodnot metody vracející IAsyncEnumerable<T> pomocí await foreach
await foreach (var number in GetNumbersRangeAsync(10, 5))
{
    Console.Write(number + " ");
}

//iterace hodnot s použitím CancellationToken
var cts = new CancellationTokenSource();
cts.CancelAfter(1000);
await foreach (int item in GetNumbersRangeCancellationAsync(10, 5).WithCancellation(cts.Token))
{
  Console.Write(item + " ");
}

//Jak napsat metodu, která vrací IAsyncEnumerable<T>?
async IAsyncEnumerable<int> GetNumbersRangeAsync(int start, int count)
{
    for(int i = start; i < start + count; i++)
    {
        await Task.Delay(1000);
        yield return start + i;
    }
}

//Přidání CancellationTokenu - atribut [EnumeratorCancellation] zajistí jeho předání do metody GetAsyncEnumerator
async IAsyncEnumerable<int> GetNumbersRangeCancellationAsync(
    int start, int count, [EnumeratorCancellation] CancellationToken cancellationToken = default)
{
    for(int i = start; i < start + count; i++)
    {
        await Task.Delay(1000, cancellationToken);
        yield return start + i;
    }
}

