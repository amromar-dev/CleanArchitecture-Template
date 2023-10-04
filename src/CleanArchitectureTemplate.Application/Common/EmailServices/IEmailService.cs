namespace CleanArchitectureTemplate.Application.Common.EmailServices;

public interface IEmailService
{
	Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent, bool ensureSuccess = false);
}
