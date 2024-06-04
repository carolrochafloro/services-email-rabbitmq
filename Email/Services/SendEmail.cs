using dotenv.net;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Email.Services;
public class SendEmail
{
    private readonly ILogger<SendEmail> _logger;
    public SendEmail(ILogger<SendEmail> logger)
    {
        DotEnv.Load();
        _logger = logger;
    }


    public async Task<bool> Send(Dictionary<string, string> messageObject)
    {
        if (messageObject == null)
        {
            throw new ArgumentNullException("Message not found");
        }

        string currentDirectory = Directory.GetCurrentDirectory();
        string htmlFilePath = Path.Combine(currentDirectory, "Templates", "Email.html");

        var client = new SendGridClient(Environment.GetEnvironmentVariable("SENDGRID_API_KEY"));
        var from = new EmailAddress(Environment.GetEnvironmentVariable("FROM_EMAIL"), "Carol Rocha");
        var subject = "Check your messages!";
        var to = new EmailAddress(Environment.GetEnvironmentVariable("TO_EMAIL"));
        var plainText = messageObject["Nome"] + messageObject["Email"] + messageObject["Mensagem"];
        var htmlTemplate = await File.ReadAllTextAsync(htmlFilePath);
        var html = string.Format(htmlTemplate, messageObject["Nome"], messageObject["Email"], messageObject["Mensagem"]);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainText, html);

        var response = await client.SendEmailAsync(msg);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("----- Success: Your e-mail was sent.");
            return true;
        }
        else
        {
            _logger.LogInformation($"----- Error: {response.StatusCode}");
            return false;
        }
    }
}
