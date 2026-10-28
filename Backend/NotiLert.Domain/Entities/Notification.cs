namespace NotiLert.Domain.Entities;

using NotiLert.Domain.Enums;

public class Notification
{
    public Guid Id { get; private set; }
    public string Recipient { get; private set; } = string.Empty;
    public string? Subject { get; private set; }
    public string Body { get; private set; } = string.Empty;
    public NotificationChannel Channel { get; private set; }
    public NotificationStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? SentAtUtc { get; private set; }
    public string? ErrorMessage { get; private set; }

    // Required for EF Core materialization
    private Notification()
    {
    }

    public Notification(
        Guid id,
        string recipient,
        string? subject,
        string body,
        NotificationChannel channel)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Recipient = !string.IsNullOrWhiteSpace(recipient)
            ? recipient
            : throw new ArgumentException("Recipient cannot be empty.", nameof(recipient));
        Subject = subject;
        Body = !string.IsNullOrWhiteSpace(body)
            ? body
            : throw new ArgumentException("Body cannot be empty.", nameof(body));
        Channel = channel;
        Status = NotificationStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public static Notification Create(
        string recipient,
        string? subject,
        string body,
        NotificationChannel channel)
    {
        return new Notification(Guid.NewGuid(), recipient, subject, body, channel);
    }

    public void MarkAsSent(DateTime sentAtUtc)
    {
        Status = NotificationStatus.Sent;
        SentAtUtc = sentAtUtc;
        ErrorMessage = null;
    }

    public void MarkAsFailed(string errorMessage)
    {
        Status = NotificationStatus.Failed;
        ErrorMessage = errorMessage;
    }
}
