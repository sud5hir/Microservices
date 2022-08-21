// See https://aka.ms/new-console-template for more information
using KafkaProducer;

Console.WriteLine("Hello, World!");

    var producer = new Producer<Message>();

    for (int i = 0; i < 25; i++)
    {
        await producer.ProduceAsync(new Message
        {
            Data = $"Pushing Data {i} !!",
        });

        await Task.Delay(1000);
    }

    WriteLine("Publish Success!");
    ReadKey();
