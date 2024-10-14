// Declare a delegate
public delegate void Notify(string message);
public delegate void NotifyEventHandler(string message);

// Publisher class
class Publisher
{
    // Declare an event using the delegate type
    public event NotifyEventHandler? NotifyEvent;

    public void RaiseEvent()
    {
        // Raise the event (invoke the delegate)
        NotifyEvent?.Invoke("Event raised!");
    }
}

// Subscriber class
class Subscriber
{
    public void OnEventNotified(string message)
    {
        Console.WriteLine($"Subscriber received message: {message}");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Assign a method to the delegate
        Notify notifier = ShowMessage;

        // Invoke the method via the delegate
        notifier("Hello, World!");

        // Add another method to the delegate (multicast)
        notifier += LogMessage;

        // Invoke both methods
        notifier("This is a multicast delegate.");

        Publisher publisher = new Publisher();
        Subscriber subscriber = new Subscriber();

        // Subscribe to the event
        publisher.NotifyEvent += subscriber.OnEventNotified;

        // Raise the event (all subscribers are notified)
        publisher.RaiseEvent();

    }

    static void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    static void LogMessage(string message)
    {
        Console.WriteLine($"Logging: {message}");
    }
}

