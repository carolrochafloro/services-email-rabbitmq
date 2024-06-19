using dotenv.net;
using FormContato.DTOs;
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


    public async Task<bool> Send(MessageDTO messageObject)
    {
        if (messageObject == null)
        {
            throw new ArgumentNullException("Message not found");
        }

        var recipient = messageObject.Recipient;
        var contact = messageObject.Contact;

        string currentDirectory = Directory.GetCurrentDirectory();
        string htmlFilePath = Path.Combine(currentDirectory, "Templates", "Email.html");

        var client = new SendGridClient(Environment.GetEnvironmentVariable("SENDGRID_API_KEY"));
        var from = new EmailAddress(Environment.GetEnvironmentVariable("FROM_EMAIL"), contact.Name);
        var subject = "Check your messages!";
        var to = new EmailAddress(recipient.RecipientEmail);
        var plainText = contact.Name + contact.Email + contact.Message;
        var htmlTemplate = await File.ReadAllTextAsync(htmlFilePath);
        var html = string.Format(htmlTemplate, contact.Name, contact.Email, contact.Message);
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
