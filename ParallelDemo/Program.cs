using System;
using ParallelDemo;
using System.Diagnostics;

Console.WriteLine("Parallel class library demo");

// 10x 1s tasks
Stopwatch sw = new();
sw.Start();
ParallelClass.RunFor();
sw.Stop();
Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");