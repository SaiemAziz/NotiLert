using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NotiLert.Data;
using NotiLert.DTOs;

namespace NotiLert.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public NotificationController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("send-broadcast")]
    public async Task<IActionResult> SendBroadcast([FromBody] SendNotificationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message content cannot be empty.");

        var emails = await _context.EmailRecipients
            .Where(u => u.IsActive)
            .Select(u => u.Email)
            .Distinct()
            .ToListAsync();

        if (!emails.Any())
            return NotFound("No active recipients found.");

        foreach (var email in emails)
        {
            await SendEmailAsync(email, request.Subject, request.Message);
        }

        return Ok(new { Count = emails.Count, Status = "Sent successfully." });
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_config["Smtp:FromAddress"]));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]));

        if (!string.IsNullOrEmpty(_config["Smtp:Username"]))
            await client.AuthenticateAsync(_config["Smtp:Username"], _config["Smtp:Password"]);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}