using CleanArchitectureTemplate.Application.Common.EmailServices;
using CleanArchitectureTemplate.Application.Interactors.Account.Outputs;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using CleanArchitectureTemplate.SharedKernel.Types;
using System.Text;

namespace CleanArchitectureTemplate.Application.Interactors.Account.Commands
{
	public class SendAuthenticationCodeByEmailCommand : IInteractorBase<bool>
	{
		public SendAuthenticationCodeByEmailCommand(string firstName, string lastName, string email, string code, AuthenticationType authenticationType)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Code = code;
			AuthenticationType = authenticationType;
		}

		public string FirstName { get; }

		public string LastName { get; }

		public string Email { get; }

		public string Code { get; }

		public AuthenticationType AuthenticationType { get; }
	}

	public class SendAuthenticationCodeByEmailHandler : IInteractorHandlerBase<SendAuthenticationCodeByEmailCommand, bool>
	{
		private readonly IEmailService emailService;

		public SendAuthenticationCodeByEmailHandler(IEmailService emailService)
		{
			this.emailService = emailService;
		}

		public async Task<ResponseBase<bool>> Handle(SendAuthenticationCodeByEmailCommand request, CancellationToken cancellationToken)
		{
			var title = PrepareEmailTitle(request.AuthenticationType);
			var emailBody = PrepareEmailBody(request.FirstName, request.LastName, request.Code, request.AuthenticationType);

			await emailService.SendEmailAsync(request.Email, $"{title}", emailBody, true);
			return new ResponseBase<bool>(true);
		}

		#region Private Methods

		private static string PrepareEmailBody(string firstName, string lastName, string code, AuthenticationType authenticationType)
		{
			var content = PrepareEmailBodyContent(authenticationType);

			var builder = new StringBuilder();
			builder.Append("<section style='font-family: Open Sans,Trebuchet MS,sans-serif'>");
			builder.Append("<img src='https://www.assets/logo.png' alt='' style='background-color: white;margin: 10px 0 30px;width: 100px;height: 100px;'>");
			builder.Append($"<h4 style='margin-top: 0;color: black;'>Hi {firstName} {lastName},</h4>");
			builder.Append($"<p style='margin-bottom: 30px;'>{content}</p>");
			builder.Append($"<p style='background-color: #ff265d;padding: 20px;margin-bottom: 30px;width: max-content;color: white;letter-spacing: 3px;border-radius: 10px;font-weight: bold;font-size: 1rem;color: white'>{code}</p>");
			builder.Append("<p style='margin-bottom: 30px;'>If you did not request this code, please ignore this email and contact us immediately.</p>");
			builder.Append("<p style='color: black;'>Cheers,<br>Dev Team</p>");
			builder.Append("</section>");

			return builder.ToString();
		}

		private static string PrepareEmailBodyContent(AuthenticationType authenticationType)
		{
			var title = "Please enter this code in the form.";

			if (authenticationType == AuthenticationType.Registration)
				title = "Use the following one-time password (OTP) to complete your registration.";
			else if (authenticationType == AuthenticationType.Login)
                title = "Use the following one-time password (OTP) to complete your login.";
            else if (authenticationType == AuthenticationType.SecurityKeys)
				title = "Use the following one-time password (OTP) to complete your security validation.";
			else if (authenticationType == AuthenticationType.EmailVerification)
				title = "Use the following one-time password (OTP) to complete the email change process";
		
			return title;
		}

		private static string PrepareEmailTitle(AuthenticationType authenticationType)
		{
			var title = "Your One-Time Password";

			if (authenticationType == AuthenticationType.Registration)
				title = "Your One-Time Password for Registration";
            else if (authenticationType == AuthenticationType.Login)
                title = "Your One-Time Password for Login";
            else if (authenticationType == AuthenticationType.SecurityKeys)
				title = "Your One-Time Password for Security Validation";
			else if (authenticationType == AuthenticationType.EmailVerification)
				title = " Your One-Time Password for Changing Email";

			return title;
		}

		#endregion
	}
}
