using System.Collections.Concurrent;

namespace ConsoleApp3
{
    internal class ConsoleApp3;

    class Program
    {
        static void Main()
        {
            var channel = new ConcurrentQueue<double>();
            var random = new Random();
            var stopSignal = new ManualResetEvent(false);
            var totalWeight = 0.0;
            var weightLock = new object();
            var producerDone = new CountdownEvent(3);

            int[] itemCounts = new int[3];

            Thread server = new Thread(() =>
            {
                while (true)
                {
                    if (channel.TryDequeue(out double item))
                    {
                        lock (weightLock)
                        {
                            if (totalWeight + item > 20.0)
                            {
                                stopSignal.Set(); 
                                break;
                            }
                            else
                            {
                                totalWeight += item;
                                Console.WriteLine($"[Server] Added item {item:F2} kg. Total: {totalWeight:F2} kg");
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(10); 
                    }
                }
            });

            server.Start();

            for (int i = 0; i < 3; i++)
            {
                int personId = i;
                new Thread(() =>
                {
                    while (!stopSignal.WaitOne(0))
                    {
                        double item = Math.Round(0.1 + random.NextDouble() * (4.5 - 0.1), 2);
                        channel.Enqueue(item);
                        itemCounts[personId]++;
                        Console.WriteLine($"[Person {personId + 1}] Packed item {item:F2} kg");
                        Thread.Sleep(100);
                    }

                    Console.WriteLine($"[Person {personId + 1}] Stopped. Total items packed: {itemCounts[personId]}");
                    producerDone.Signal();
                }).Start();
            }

            producerDone.Wait();
            server.Join();

            Console.WriteLine($"Total suitcase weight: {totalWeight:F2} kg");
        }
    }

}
