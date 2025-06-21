using System.Text;
using AzureServiceBusDemo.Common;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;

namespace AzureServiceBusDemo.Producer.Services;

/// <summary>
/// Service layer responsible for managing Azure Service Bus operations (queues, topics, subscriptions).
/// </summary>
public class AzureService
{
    private readonly ManagementClient _managementClient;

    /// <summary>
    /// Constructor with dependency injection for ManagementClient.
    /// </summary>
    /// <param name="managementClient">Azure Service Bus management client.</param>
    public AzureService(ManagementClient managementClient)
    {
        _managementClient = managementClient;
    }

    /// <summary>
    /// Sends a message to the specified queue.
    /// </summary>
    public async Task SendMessageToQueueAsync(string queueName, object messageContent, string messageType = null!)
    {
        IQueueClient queueClient = new QueueClient(Constants.ConnectionString, queueName);
        await SendMessageAsync(queueClient, messageContent, messageType);
    }

    /// <summary>
    /// Creates the queue if it does not already exist.
    /// </summary>
    public async Task CreateQueueIfNotExistsAsync(string queueName)
    {
        if (!await _managementClient.QueueExistsAsync(queueName))
            await _managementClient.CreateQueueAsync(queueName);
    }

    /// <summary>
    /// Sends a message to the specified topic.
    /// </summary>
    public async Task SendMessageToSubscriptionAsync(string topicName, object messageContent,
        string messageType = null!)
    {
        ITopicClient topicClient = new TopicClient(Constants.ConnectionString, topicName);
        await SendMessageAsync(topicClient, messageContent, messageType);
    }

    /// <summary>
    /// Creates the topic if it does not already exist.
    /// </summary>
    public async Task CreateTopicIfNotExistsAsync(string topicName)
    {
        if (!await _managementClient.TopicExistsAsync(topicName))
            await _managementClient.CreateTopicAsync(topicName);
    }

    /// <summary>
    /// Creates a subscription with an optional correlation rule if it does not already exist.
    /// </summary>
    public async Task CreateSubscriptionIfNotExistsAsync(string topicName, string subscriptionName,
        string messageType = null!, string ruleName = null!)
    {
        if (await _managementClient.SubscriptionExistsAsync(topicName, subscriptionName))
            return;

        if (messageType is not null)
        {
            SubscriptionDescription subscriptionClient = new SubscriptionDescription(topicName, subscriptionName);
            CorrelationFilter correlationFilter = new CorrelationFilter();
            correlationFilter.Properties["MessageType"] = messageType;
            RuleDescription ruleDescription = new RuleDescription(ruleName ?? messageType + "Rule", correlationFilter);
            await _managementClient.CreateSubscriptionAsync(subscriptionClient, ruleDescription);
        }
        else
        {
            await _managementClient.CreateSubscriptionAsync(topicName, subscriptionName);
        }
    }

    /// <summary>
    /// Internal method for serializing and sending a message using a Service Bus client.
    /// </summary>
    private async Task SendMessageAsync(ISenderClient client, object messageContent, string messageType)
    {
        byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageContent));
        Message message = new Message(byteArray);
        message.UserProperties["MessageType"] = messageType;
        await client.SendAsync(message);
    }
}