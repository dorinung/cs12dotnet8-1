using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWorkQueue
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    class WorkQueue<T>
    {
        private readonly BlockingCollection<T> _queue = new();
        private readonly CancellationTokenSource _cts = new();
        private Task _workerTask;

        public WorkQueue()
        {
            // Start the worker task that waits for and processes items
            _workerTask = Task.Run(() => ProcessQueue(_cts.Token));
        }

        // Enqueue work into the queue
        public void EnqueueWork(T workItem)
        {
            _queue.Add(workItem);
            Console.WriteLine($"Work item enqueued: {workItem}");
        }

        // The worker method that processes items from the queue
        private void ProcessQueue(CancellationToken token)
        {
            try
            {
                foreach (var workItem in _queue.GetConsumingEnumerable(token))
                {
                    // Process the item
                    Console.WriteLine($"Processing work item: {workItem}");
                    // Simulate some processing delay
                    Task.Delay(1000).Wait();
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Work queue processing was canceled.");
            }
        }

        // Signal to stop processing and wait for the worker task to finish
        public void StopProcessing()
        {
            _cts.Cancel();
            _queue.CompleteAdding(); // Signal that no more items will be added
            _workerTask.Wait(); // Wait for the worker task to complete
            Console.WriteLine("Work queue processing stopped.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var workQueue = new WorkQueue<string>();

            // Enqueue some work items
            workQueue.EnqueueWork("Task 1");
            workQueue.EnqueueWork("Task 2");
            workQueue.EnqueueWork("Task 3");

            // Simulate some delay before stopping the queue
            Task.Delay(5000).Wait();

            // Stop processing
            workQueue.StopProcessing();
        }
    }

}
