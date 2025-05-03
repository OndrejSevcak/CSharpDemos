Queue<string?> requests = new Queue<string?>();

//Thread 1 - Monitor requests -> workers thread
Thread monitoringThread = new Thread(MonitorRequests);
monitoringThread.Start();

//Thread 2 - Simulate web server -> Main thread
Console.WriteLine("Web server is running...");
while (true)
{
    string? input = Console.ReadLine();
    if (input?.ToLower() == "exit")
    {
        break;
    }

    requests.Enqueue(input);
}

void MonitorRequests()
{
    while (true)
    {
        if(requests.Count > 0)
        {
            string? input = requests.Dequeue();
            //Thread 3 - Process request -> workers thread
            Thread processingThread = new Thread(() => ProcessRequest(input));
            processingThread.Start();
        }
        Thread.Sleep(1000);
    }
}

void ProcessRequest(string? input)
{
    if(input == "c")
    {
        Thread.Sleep(2000);
    }
    else 
    {
        Thread.Sleep(1000);
    }
    Console.WriteLine($"Processed request: {input}");
}