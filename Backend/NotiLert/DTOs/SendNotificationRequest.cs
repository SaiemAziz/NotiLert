namespace NotiLert.DTOs
{
    public class SendNotificationRequest
    {
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
