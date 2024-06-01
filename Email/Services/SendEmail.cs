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

namespace Email.Services;
internal class SendEmail
{
    public SendEmail()
    {
        DotEnv.Load();
    }
    public async Task Send(string message)
    {
        if (message == null)
        {
            throw new ArgumentNullException("message not found");
        }

        var client = new SendGridClient(Environment.GetEnvironmentVariable("SENDGRID_API_KEY"));
        var from = new EmailAddress(Environment.GetEnvironmentVariable("FROM_EMAIL"), "Carol Rocha");
        var subject = "You have a new message";
        var to = new EmailAddress(Environment.GetEnvironmentVariable("TO_EMAIL"));
        var plainText = message;
        var html = "<p>Teste</p>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainText, html);
        var response = await client.SendEmailAsync(msg);

    }
}
