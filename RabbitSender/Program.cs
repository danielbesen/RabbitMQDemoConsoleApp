﻿using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitSender.Models;
using System.Text;
using System.Text.Json.Serialization;


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

    //set exchange, set queue & bind exchange and queue
    channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
    channel.QueueDeclare(queueName, false, false, false, null);
    channel.QueueBind(queueName, exchangeName, routingKey);

    User user = new User()
    {
        Age = 25,
        Email = "nielb@gmail.com",
        FirstName = "Daniel",
        LastName = "de Sá Besen",
        Processed = false
    };

    string jsonMessage = JsonConvert.SerializeObject(user);
    var encondedMessage = Encoding.UTF8.GetBytes(jsonMessage);

    channel.BasicPublish(exchangeName, routingKey, null, encondedMessage);
    channel.Close();
    cnn.Close();
};




