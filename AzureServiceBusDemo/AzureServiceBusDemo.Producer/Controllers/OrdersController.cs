using AzureServiceBusDemo.Common;
using AzureServiceBusDemo.Common.DTOs.Orders;
using AzureServiceBusDemo.Common.Events.Orders;
using AzureServiceBusDemo.Producer.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureServiceBusDemo.Producer.Controllers;

/// <summary>
/// RESTful controller for managing order creation and deletion.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly AzureService _azureService;

    /// <summary>
    /// Constructor with AzureService dependency.
    /// </summary>
    public OrdersController(AzureService azureService)
    {
        _azureService = azureService;
    }

    /// <summary>
    /// Creates a new order by publishing messages to both queue and topic.
    /// </summary>
    /// <param name="orderDto">Order details.</param>
    [HttpPost]
    public async Task CreateOrderAsync(OrderDto orderDto)
    {
        OrderCreationEvent orderCreationEvent = new OrderCreationEvent
        {
            Id = orderDto.Id,
            ProductName = orderDto.ProductName
        };

        #region QueueConsumer

        await _azureService.CreateQueueIfNotExistsAsync(Constants.OrderCreationQueueName);
        await _azureService.SendMessageToQueueAsync(Constants.OrderCreationQueueName, orderCreationEvent);

        #endregion

        #region SubscriptionConsumer

        await _azureService.CreateTopicIfNotExistsAsync(Constants.OrderTopic);
        await _azureService.CreateSubscriptionIfNotExistsAsync(Constants.OrderTopic, Constants.OrderCreationSubName,
            "OrderCreation", "OrderCreationRule");
        await _azureService.SendMessageToSubscriptionAsync(Constants.OrderTopic, orderCreationEvent, "OrderCreation");

        #endregion
    }

    /// <summary>
    /// Deletes an existing order by publishing messages to both queue and topic.
    /// </summary>
    /// <param name="id">Order identifier.</param>
    [HttpDelete("{id}")]
    public async Task DeleteOrderAsync(int id)
    {
        OrderDeletionEvent orderDeletionEvent = new OrderDeletionEvent
        {
            Id = id
        };

        #region QueueConsumer

        await _azureService.CreateQueueIfNotExistsAsync(Constants.OrderDeletionQueueName);
        await _azureService.SendMessageToQueueAsync(Constants.OrderDeletionQueueName, orderDeletionEvent);

        #endregion

        #region SubscriptionConsumer

        await _azureService.CreateTopicIfNotExistsAsync(Constants.OrderTopic);
        await _azureService.CreateSubscriptionIfNotExistsAsync(Constants.OrderTopic, Constants.OrderDeletionSubName,
            "OrderDeletion", "OrderDeletionRule");
        await _azureService.SendMessageToSubscriptionAsync(Constants.OrderTopic, orderDeletionEvent, "OrderDeletion");

        #endregion
    }
}