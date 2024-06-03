using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotenv.net;
using SendGrid;
using SendGrid.Helpers.Errors.Model;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
using System.Text.Json;

namespace Email.Services;
internal class SendEmail
{
    public SendEmail()
    {
        DotEnv.Load();
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
            await Console.Out.WriteLineAsync($"E-mail successfully sent.");
            return true;
        }
        else
        {
            Console.WriteLine($"Error: {response.StatusCode}");
            return false;
        }
    }
}
