namespace AzureServiceBusDemo.Common.DTOs.Orders;

/// <summary>
/// Data Transfer Object (DTO) representing an order.
/// </summary>
public class OrderDto
{
    /// <summary>
    /// Unique identifier of the order.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the product associated with the order.
    /// </summary>
    public string ProductName { get; set; }
}