namespace NotiLert.Services
{
    public interface INotificationService
    {
        Task<int> BroadcastAsync(string subject, string message);
    }
}
