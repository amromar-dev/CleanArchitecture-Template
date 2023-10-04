using Microsoft.Extensions.Options;
using CleanArchitectureTemplate.Application.Common.EmailServices;
using CleanArchitectureTemplate.SharedKernel.Types;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace CleanArchitectureTemplate.Infrastructure.Services.EmailServices
{
	public class SendGridService : IEmailService
	{
		private readonly SendGridClient sendGridClient;
		private readonly SendGridConfiguration configuration;

		public SendGridService(IOptions<SendGridConfiguration> configuration)
		{
			this.configuration = configuration.Value;
			sendGridClient = new SendGridClient(this.configuration.ApiKey);
		}

		public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlContent, bool ensureSuccess = false)
		{
			var from = new EmailAddress(configuration.FromEmail);
			var to = new EmailAddress(toEmail);

			var message = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
			message.ReplyTo = new EmailAddress(configuration.ReplyEmail);

			var response = await sendGridClient.SendEmailAsync(message);
			
			if (response.IsSuccessStatusCode == false && ensureSuccess == true)
				throw new BusinessException(HttpStatusCode.ServiceUnavailable, "Sorry, we were unable to send your email at this time. Please try again later");

			return response.IsSuccessStatusCode;
		}
	}
}
