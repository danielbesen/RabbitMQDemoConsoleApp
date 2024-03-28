using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitSender.Models;
using System.Text;

string username = Environment.GetEnvironmentVariable("RABBIT_USERNAME");
string password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD");

ConnectionFactory connection = new ConnectionFactory()
{
    Uri = new Uri($"amqp://{username}:{password}@localhost:5672"),
    ClientProvidedName = "Rabbit Receiver APP One"
};

IConnection cnn = connection.CreateConnection();

using (IModel channel = cnn.CreateModel())
{
    string exchangeName = "DemoExchange";
    string routingKey = "demo-routing-key";
    string queueName = "DemoQueue";

    //set exchange, set queue & bind exchange and queue
    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
    channel.QueueDeclare(queueName, false, false, false, null);
    channel.QueueBind(queueName, exchangeName, routingKey);
    channel.BasicQos(0, 1, false);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (sender, args) =>
    {
        Task.Delay(TimeSpan.FromSeconds(3)).Wait();    //Simulate some work
        var body = args.Body.ToArray();
        string message = Encoding.UTF8.GetString(body);

        var user = JsonConvert.DeserializeObject<User>(message);

        Console.WriteLine($"Message received: {user}");
        user.Processed = true;
        Console.WriteLine($"Message processed: {user.Processed}");
        Console.WriteLine();

        channel.BasicAck(args.DeliveryTag, false);
    };

    string consumerTag = channel.BasicConsume(queueName, false, consumer);

    //Keep the consumer running
    Console.ReadLine();

    channel.BasicCancel(consumerTag);
    channel.Close();
    cnn.Close();
};