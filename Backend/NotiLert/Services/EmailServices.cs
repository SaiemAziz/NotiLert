using MailKit.Net.Smtp;
using MimeKit;

namespace NotiLert.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(IReadOnlyList<string> to, string subject, string body)
    {
        if (to.Count == 0)
            return;

        using var client = new SmtpClient();
        await client.ConnectAsync(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"] ?? "587"));

        if (!string.IsNullOrEmpty(_config["Smtp:Username"]))
        {
            await client.AuthenticateAsync(_config["Smtp:Username"], _config["Smtp:Password"]);
        }
        for (int i = 0; i < to.Count; i++)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_config["Smtp:FromAddress"]));
            message.To.Add(MailboxAddress.Parse(to[i]));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };
            await client.SendAsync(message);
            if (i < to.Count - 1)
            {
                await Task.Delay(int.Parse(_config["Smtp:Delay"])); // Wait between emails
            }
        }
        await client.DisconnectAsync(true);
    }
}
