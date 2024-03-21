using RabbitMQ.Client;
using System.Text;

ConnectionFactory connectionFactory = new ConnectionFactory();

string username = Environment.GetEnvironmentVariable("RABBIT_USERNAME");
string password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD");

connectionFactory.Uri = new Uri($"amqp://{username}:{password}");