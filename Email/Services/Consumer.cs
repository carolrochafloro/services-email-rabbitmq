using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using dotenv.net;
using Email.Context;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace Email.Services;
public class Consumer
{
    protected readonly string? hostName = Environment.GetEnvironmentVariable("HOST_NAME");
    protected ConnectionFactory factory;
    protected IConnection connection;
    protected IModel channel;
    private readonly SendEmail _sendEmail;
    public ConcurrentQueue<string> Messages { get; } = new ConcurrentQueue<string>();
    private readonly EmailContext _context;
    private readonly UpdateDB _updateDB;
    private readonly ILogger<UpdateDB> _loggerDb;
    private readonly ILogger<SendEmail> _loggerSendEmail;
    private readonly ILogger<Consumer> _logger;
    // logger


    public Consumer(EmailContext context, ILogger<Consumer> logger, ILogger<UpdateDB> loggerDb, ILogger<SendEmail> loggerSendEmail)
    {
        factory = new ConnectionFactory { HostName = "localhost" };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        _loggerDb = loggerDb;
        _loggerSendEmail = loggerSendEmail;
        _logger = logger;
        _sendEmail = new SendEmail(_loggerSendEmail);
        _context = context;
        _updateDB = new UpdateDB(_context, _loggerDb);
        _logger = logger;
    }

    public Task Consume()
    {
        string queueName = "form_contact";

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
            );

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Messages.Enqueue(message);
            _logger.LogInformation($"----- Message consumed at {DateTime.Now}. -----");

        };

        channel.BasicConsume(queue: queueName,
                            autoAck: true,
                            consumer: consumer);

        return Task.CompletedTask;
    }

    public async Task HandleMessages()
    {
     
        if (Messages.TryDequeue(out var message))
        {
            var messageObject = JsonSerializer.Deserialize<Dictionary<string, string>>(message);
            bool isEmailSent = await _sendEmail.Send(messageObject);
            Guid id = Guid.Parse(messageObject["Id"]);
            await _updateDB.UpdateIsSent(isEmailSent, id);
        }


    }
}
