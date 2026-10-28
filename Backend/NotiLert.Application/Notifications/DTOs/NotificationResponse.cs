namespace NotiLert.Application.Notifications.DTOs;

using NotiLert.Domain.Entities;
using NotiLert.Domain.Enums;

public record NotificationResponse(
    Guid Id,
    string Recipient,
    string? Subject,
    string Body,
    NotificationChannel Channel,
    NotificationStatus Status,
    DateTime CreatedAtUtc,
    DateTime? SentAtUtc,
    string? ErrorMessage)
{
    public static NotificationResponse FromEntity(Notification notification) =>
        new(
            notification.Id,
            notification.Recipient,
            notification.Subject,
            notification.Body,
            notification.Channel,
            notification.Status,
            notification.CreatedAtUtc,
            notification.SentAtUtc,
            notification.ErrorMessage);
}
