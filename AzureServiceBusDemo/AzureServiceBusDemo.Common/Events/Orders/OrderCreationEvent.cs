namespace AzureServiceBusDemo.Common.Events.Orders;

/// <summary>
/// Event that is triggered when an order is created.
/// </summary>
public class OrderCreationEvent : BaseEvent
{
    /// <summary>
    /// Name of the product in the created order.
    /// </summary>
    public string ProductName { get; set; }
}