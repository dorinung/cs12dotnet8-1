HttpClient client = new();

HttpResponseMessage response =
  await client.GetAsync("http://www.apple.com/");

WriteLine("Apple's home page has {0:N0} bytes.",
  response.Content.Headers.ContentLength);

// Start the HTTP GET request
Task<HttpResponseMessage> getResponseTask = client.GetAsync("http://www.apple.com/");

// Poll for task completion
while (!getResponseTask.IsCompleted)
{
    // Perform other work while waiting for the task to complete
    WriteLine("Some text");

   // Optionally, wait for a short time before checking again
   await Task.Delay(10);  // Wait for 10 milliseconds
}

// Once the task completes, you can get the result
if (getResponseTask.IsCompletedSuccessfully)
{
    response = getResponseTask.Result;
    WriteLine("Apple's home page has {0:N0} bytes.",
      response.Content.Headers.ContentLength);
}
else if (getResponseTask.IsFaulted)
{
    // Handle any errors that occurred during the task
    Console.WriteLine("The request failed: " + getResponseTask.Exception);
}
else if (getResponseTask.IsCanceled)
{
    // Handle the case where the task was canceled
    Console.WriteLine("The request was canceled.");
}

