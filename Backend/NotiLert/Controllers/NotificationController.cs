using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NotiLert.Data;
using NotiLert.DTOs;
using NotiLert.Services;

namespace NotiLert.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _context;

    public NotificationController(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
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

        await _emailService.SendEmailAsync(emails, request.Subject, request.Message);

        return Ok(new { Count = emails.Count, Status = "Sent successfully." });
    }
}