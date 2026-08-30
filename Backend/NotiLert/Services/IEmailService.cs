namespace NotiLert.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(List<string> emails, string subject, string body);
    }
}
