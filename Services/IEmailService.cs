namespace JobPortalSystem.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string htmlMessage);
        Task<bool> SendApplicationStatusEmailAsync(string toEmail, string applicantName, string jobTitle, string status);
        Task<bool> SendWelcomeEmailAsync(string toEmail, string userName);
    }
}
