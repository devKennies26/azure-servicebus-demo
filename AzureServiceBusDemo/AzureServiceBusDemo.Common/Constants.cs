namespace AzureServiceBusDemo.Common;

/// <summary>
/// Static class containing application-wide constants for Service Bus configuration.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Azure Service Bus connection string.
    /// </summary>
    public const string ConnectionString =
        "Endpoint=sb://<your-service-bus-namespace>.servicebus.windows.net/;SharedAccessKeyName=<your-policy-name>;SharedAccessKey=<your-policy-key>";

    /// <summary>
    /// Queue name for order creation events.
    /// </summary>
    public const string OrderCreationQueueName = "OrderCreatedQueue";

    /// <summary>
    /// Queue name for order deletion events.
    /// </summary>
    public const string OrderDeletionQueueName = "OrderDeletedQueue";

    /// <summary>
    /// Topic name for publishing order events.
    /// </summary>
    public const string OrderTopic = "OrderTopic";

    /// <summary>
    /// Subscription name for handling order creation events.
    /// </summary>
    public const string OrderCreationSubName = "OrderCreatedSub";

    /// <summary>
    /// Subscription name for handling order deletion events.
    /// </summary>
    public const string OrderDeletionSubName = "OrderDeletedSub";
}