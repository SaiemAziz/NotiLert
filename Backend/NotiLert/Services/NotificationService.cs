using Microsoft.EntityFrameworkCore;
using NotiLert.Data;

namespace NotiLert.Services;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public NotificationService(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<int> BroadcastAsync(string subject, string message)
    {
        var emails = await _context.EmailRecipients
            .Where(u => u.IsActive)
            .Select(u => u.Email)
            .Distinct()
            .ToListAsync();

        if (emails.Count == 0)
            return 0;

        await _emailService.SendEmailAsync(emails, subject, message);

        return emails.Count;
    }
}
