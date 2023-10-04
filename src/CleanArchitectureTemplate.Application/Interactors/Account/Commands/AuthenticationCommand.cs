using AutoMapper;
using CleanArchitectureTemplate.Application.Common.EmailServices;
using CleanArchitectureTemplate.Application.Common.EmailTemplates;
using CleanArchitectureTemplate.Application.Common.IdentityServices;
using CleanArchitectureTemplate.Application.Interactors.Account.Outputs;
using CleanArchitectureTemplate.Domain.Customers;
using CleanArchitectureTemplate.Domain.Customers.Interfaces;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Application.Interactors.Account.Commands
{
    public class AuthenticationCommand : IInteractorBase<AuthenticationOutput>
	{
		public string Email { get; set; }

		public string AuthCode { get; set; }
	}

	public class AuthenticationHandler : IInteractorHandlerBase<AuthenticationCommand, AuthenticationOutput>
	{
		private readonly ICustomerRepository customerRepository;
		private readonly ITokenService tokenService;
		private readonly IEmailService emailService;
		private readonly IEmailTemplateHelper emailTemplateHelper;
		private readonly IMapper mapper;

		public AuthenticationHandler(ICustomerRepository customerRepository, ITokenService tokenService, IMapper mapper, IEmailService emailService, IEmailTemplateHelper emailTemplateHelper)
		{
			this.customerRepository = customerRepository;
			this.tokenService = tokenService;
			this.mapper = mapper;
			this.emailService = emailService;
			this.emailTemplateHelper = emailTemplateHelper;
		}

		public async Task<ResponseBase<AuthenticationOutput>> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
		{
			var customer = await customerRepository.GetByEmail(request.Email);
			if (customer == null)
				throw new BusinessException("This email is not exist.");

			customer.ValidateAuthCode(request.AuthCode);
			customerRepository.Update(customer);

			var token = await tokenService.GenerateToken(customer.Id, customer.Email);

			var response = mapper.Map<AuthenticationOutput>(customer);
			response.AccessToken = token.AccessToken;
			response.RefreshToken = token.RefreshToken;
			response.ExpiresIn = token.ExpiresIn;

			if (customer.LastLoginAt.HasValue == false)
				await SendRegistrationConfirmation(customer);

			customer.MarkAsLoggedIn();
			await customerRepository.CommitAsync(cancellationToken);

			return new ResponseBase<AuthenticationOutput>(response);
		}

		#region Private Methods

		private Task SendRegistrationConfirmation(Customer customer)
		{
            emailTemplateHelper.PrepareTemplate("WelcomeEmail.html");
            emailTemplateHelper.AddTemplateParameter("firstname", customer.FirstName);

            _ = emailService.SendEmailAsync(customer.Email, "Welcome", emailTemplateHelper.GetEmailContent());

            return Task.CompletedTask;
        }

        #endregion
    }
}
