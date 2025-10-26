using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JobPortalSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            try
            {
                var apiKey = _configuration["SendGrid:ApiKey"];
                var client = new SendGridClient(apiKey);

                var from = new EmailAddress(
                    _configuration["SendGrid:FromEmail"],
                    _configuration["SendGrid:FromName"]
                );

                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlMessage);

                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    _logger.LogInformation($"Email sent successfully to {toEmail}");
                    return true;
                }
                else
                {
                    _logger.LogError($"Failed to send email to {toEmail}. Status: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendApplicationStatusEmailAsync(string toEmail, string applicantName, string jobTitle, string status)
        {
            var subject = $"Application Status Update - {jobTitle}";
            var htmlMessage = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>Application Status Update</h2>
                    <p>Dear {applicantName},</p>
                    <p>Your application for the position of <strong>{jobTitle}</strong> has been updated.</p>
                    <p>Current Status: <strong>{status}</strong></p>
                    <p>Please log in to your dashboard for more details.</p>
                    <br/>
                    <p>Best regards,<br/>Job Portal Team</p>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, htmlMessage);
        }

        public async Task<bool> SendWelcomeEmailAsync(string toEmail, string userName)
        {
            var subject = "Welcome to Job Portal!";
            var htmlMessage = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>Welcome to Job Portal!</h2>
                    <p>Dear {userName},</p>
                    <p>Thank you for registering with Job Portal. We're excited to have you on board!</p>
                    <br/>
                    <p>Best regards,<br/>Job Portal Team</p>
                </body>
                </html>
            ";

            return await SendEmailAsync(toEmail, subject, htmlMessage);
        }
    }
}
