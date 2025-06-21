using System.Text;
using AzureServiceBusDemo.Common;
using AzureServiceBusDemo.Common.Events.Orders;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace AzureServiceBusDemo.Consumer;

/// <summary>
/// Main entry point of the application. Handles queue and topic subscription consumers.
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        #region QueueConsumer

        ConsumeQueue<OrderCreationEvent>(Constants.OrderCreationQueueName,
            ra => { Console.WriteLine($"Order Created: {ra.Id}, Product: {ra.ProductName}"); }).Wait();

        ConsumeQueue<OrderDeletionEvent>(Constants.OrderDeletionQueueName,
            ra => { Console.WriteLine($"Order Deleted: {ra.Id}"); }).Wait();

        Console.ReadLine();

        #endregion

        #region SubscriptionConsumer

        ConsumeSubscription<OrderCreationEvent>(Constants.OrderTopic, Constants.OrderCreationSubName,
            ra => { Console.WriteLine($"Order Created: {ra.Id}, Product: {ra.ProductName}"); }).Wait();

        ConsumeSubscription<OrderDeletionEvent>(Constants.OrderTopic, Constants.OrderDeletionSubName,
            ra => { Console.WriteLine($"Order Deleted: {ra.Id}"); }).Wait();

        Console.ReadLine();

        #endregion
    }

    /// <summary>
    /// Subscribes to a specific Azure Service Bus queue and processes incoming messages.
    /// </summary>
    /// <typeparam name="T">Type of message to deserialize.</typeparam>
    /// <param name="queueName">Name of the queue to consume.</param>
    /// <param name="receiveAction">Action to be executed with the deserialized message.</param>
    private static async Task ConsumeQueue<T>(string queueName, Action<T> receiveAction)
    {
        IQueueClient queueClient = new QueueClient(Constants.ConnectionString, queueName);

        queueClient.RegisterMessageHandler(async (message, cancellationToken) =>
        {
            T? model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
            receiveAction(model);
            await Task.CompletedTask;
        }, new MessageHandlerOptions((e) => Task.CompletedTask));

        Console.WriteLine($"{typeof(T).Name} is being consumed from {queueName} queue.");
    }

    /// <summary>
    /// Subscribes to a specific Azure Service Bus topic subscription and processes incoming messages.
    /// </summary>
    /// <typeparam name="T">Type of message to deserialize.</typeparam>
    /// <param name="topicName">Name of the topic.</param>
    /// <param name="subscriptionName">Name of the subscription.</param>
    /// <param name="receiveAction">Action to be executed with the deserialized message.</param>
    private static async Task ConsumeSubscription<T>(string topicName, string subscriptionName, Action<T> receiveAction)
    {
        ISubscriptionClient subscriptionClient =
            new SubscriptionClient(Constants.ConnectionString, topicName, subscriptionName);

        subscriptionClient.RegisterMessageHandler(async (message, cancellationToken) =>
        {
            T? model = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(message.Body));
            receiveAction(model);
            await Task.CompletedTask;
        }, new MessageHandlerOptions((e) => Task.CompletedTask));

        Console.WriteLine(
            $"{typeof(T).Name} is being consumed from {topicName} topic with {subscriptionName} subscription.");
    }
}