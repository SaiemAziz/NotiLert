namespace NotiLert.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(IReadOnlyList<string> to, string subject, string body);
    }
}
