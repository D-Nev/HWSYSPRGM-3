namespace ConsoleApp2
{
    internal class Program
    {
        class ConsoleApp2
        {
            static void Main()
            {
                int totalJobs = 10;          
                int printerCount = 3;         
                Semaphore semaphore = new Semaphore(printerCount, printerCount);
                Printer printer = new Printer();

                for (int i = 1; i <= totalJobs; i++)
                {
                    int jobNumber = i; 
                    new Thread(() =>
                    {
                        semaphore.WaitOne(); 
                        printer.PrintJob(jobNumber);
                        semaphore.Release(); 
                    }).Start();
                }

                Console.ReadLine();
            }
        }
        class Printer
        {
            public void PrintJob(int jobNumber)
            {
                Console.WriteLine("Printing job {0} on printer {1}.", jobNumber,
                    Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(1000);
                Console.WriteLine("Job {0} completed on printer {1}.", jobNumber,
                    Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
    
}
