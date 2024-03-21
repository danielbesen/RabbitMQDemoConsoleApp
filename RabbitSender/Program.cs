using RabbitMQ.Client;
using System.Text;


string username = Environment.GetEnvironmentVariable("RABBIT_USERNAME");
string password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD");

//set connection
ConnectionFactory connectionFactory = new ConnectionFactory()
{
    Uri = new Uri($"amqp://{username}:{password}@localhost:5672"),
    ClientProvidedName = "Rabbit Sender APP Zero"
};

//open connection
IConnection cnn = connectionFactory.CreateConnection();

//set channel
using (IModel channel = cnn.CreateModel()){

    string exchangeName = "DemoExchange";
    string routingKey = "demo-routing-key";
    string queueName = "DemoQueue";

    //set exchange
    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
    channel.QueueDeclare(queueName, false, false, false, null);
    channel.QueueBind(queueName, exchangeName, routingKey);

};




