namespace NotiLert.Application.Notifications.DTOs;

using NotiLert.Domain.Enums;

public record CreateNotificationRequest(
    string Recipient,
    string? Subject,
    string Body,
    NotificationChannel Channel);
