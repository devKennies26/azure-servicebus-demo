# Azure Service Bus Demo

This is a minimalistic ASP.NET Core demo project designed to showcase the basic usage of **Azure Service Bus** with queues, topics, and subscriptions. The solution follows a simplified event-driven approach using message-based communication patterns.

## Purpose

The primary goal of this project is to demonstrate how to integrate Azure Service Bus into a .NET-based application for asynchronous communication. It simulates the creation and deletion of orders through event publishing, with messages delivered to both queues and topic-based subscriptions.

## Key Concepts

- **Azure Service Bus Queues**: Point-to-point communication for single consumer scenarios.
- **Azure Service Bus Topics & Subscriptions**: Publish-subscribe messaging model for multiple consumers with optional filtering.
- **Message Routing**: Use of correlation filters to selectively route messages based on custom metadata.
- **Event-Driven Design**: Order creation and deletion events are used to decouple producers from consumers.

## Technologies Used

- .NET 8 / ASP.NET Core
- Azure.Messaging.ServiceBus
- Newtonsoft.Json
- Azure Service Bus (Queue, Topic, Subscription)

## Notes

This project is intended for learning, prototyping, or as a foundational reference for building larger, message-driven systems on Azure. It avoids production complexities and focuses on clarity and simplicity.
