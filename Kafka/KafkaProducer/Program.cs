// See https://aka.ms/new-console-template for more information

using Kafka;

Console.WriteLine("Hello, World!");
var consumer = new Consumer<Message>();
await consumer.ConsumeAsync();