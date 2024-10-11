using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Hello, World!");
        HttpClient client = new();

        // TaskCompletionSource allows manual signaling of a task's completion.
        var taskCompletionSource = new TaskCompletionSource();

        // Start the HTTP GET request
        Task<HttpResponseMessage> getResponseTask = client.GetAsync("http://www.apple.com/");

        // A separate task that waits for task completion or external signaling
        var waitForCompletionTask = WaitForCompletionOrSignal(getResponseTask, taskCompletionSource.Task);

        // Simulate other work that may signal task completion
        DoOtherWork(taskCompletionSource);

        // Wait for either the request to finish or for the signal
        await waitForCompletionTask;

        if (getResponseTask.IsCompletedSuccessfully)
        {
            HttpResponseMessage response = getResponseTask.Result;
            ProcessResponse(response);
        }
        else if (getResponseTask.IsFaulted)
        {
            Console.WriteLine("The request failed: " + getResponseTask.Exception);
        }
        else if (getResponseTask.IsCanceled)
        {
            Console.WriteLine("The request was canceled.");
        }
    }

    static async Task WaitForCompletionOrSignal(Task<HttpResponseMessage> getResponseTask, Task signalTask)
    {
        // Wait for either the HTTP request to finish or for the external signal
        Task completedTask = await Task.WhenAny(getResponseTask, signalTask);

        if (completedTask == signalTask)
        {
            Console.WriteLine("Processing was signaled to complete early.");
        }
        else
        {
            Console.WriteLine("HTTP request completed.");
        }
    }

    static void DoOtherWork(TaskCompletionSource taskCompletionSource)
    {
        // Simulate some other work being done, then signal completion
        Task.Run(async () =>
        {
            Console.WriteLine("Doing some other work...");
            await Task.Delay(3000);  // Simulate some delay
            taskCompletionSource.SetResult();  // Signal that other work is done
            Console.WriteLine("Signaled task completion.");
        });
    }

    static void ProcessResponse(HttpResponseMessage response)
    {
        Console.WriteLine($"Response received with status code: {response.StatusCode}");
    }
}
