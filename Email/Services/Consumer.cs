using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Email.Models;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Email.Services;
internal class Consumer
{
    protected readonly string? hostName = Environment.GetEnvironmentVariable("HOST_NAME");
    protected ConnectionFactory factory;
    protected IConnection connection;
    protected IModel channel;
    public string message;
    public Message Message { get; private set; }
    // logger


    public Consumer()
    {
        factory = new ConnectionFactory { HostName = "localhost" };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
    }

    public string Consume(string queueName)
    {
        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
            );

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
            //logger
        };

        
        channel.BasicConsume(queue: queueName,
                            autoAck: true,
                            consumer: consumer);

        return message;
        //logger - mensagem
    }
}
