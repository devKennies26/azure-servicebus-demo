namespace AzureServiceBusDemo.Common.Events;

/// <summary>
/// Base class for events used in the messaging infrastructure.
/// </summary>
public class BaseEvent
{
    /// <summary>
    /// Unique identifier for the event.
    /// </summary>
    public int Id { get; set; }
}