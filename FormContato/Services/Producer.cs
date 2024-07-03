using dotenv.net;
using FormContato.DTOs;
using FormContato.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace FormContato.Services;

public class Producer : IDisposable
{
    protected readonly string? hostName = Environment.GetEnvironmentVariable("HOST_NAME");
    protected readonly string? queueName = Environment.GetEnvironmentVariable("QUEUE_NAME");
    protected ConnectionFactory factory;
    protected IConnection connection;
    protected IModel channel;
    public Producer()
    {
        DotEnv.Load();
        factory = new ConnectionFactory { HostName = hostName };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
    }

    public void Produce(ContactModel contact, RecipientModel recipient)
    {

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
            );

        var newMessage = new MessageDTO
        {
            Contact = contact,
            Recipient = recipient,
        };

        string message = JsonConvert.SerializeObject(newMessage);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
                     routingKey: queueName,
                     basicProperties: null,
                     body: body);
    }
    public void Dispose()
    {
        channel.Close();
        connection.Close();
    }

}
