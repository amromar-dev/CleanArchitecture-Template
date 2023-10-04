using MediatR;
using CleanArchitectureTemplate.Application.Interactors.Account.Outputs;
using CleanArchitectureTemplate.Domain.Customers.Interfaces;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Application.Interactors.Account.Commands
{
	public class SendAuthenticationCodeCommand : IInteractorBase<SendAuthenticationCodeOutput>
	{
		public string Email { get; set; }
	}

    public class SendAuthenticationCodeHandler : IInteractorHandlerBase<SendAuthenticationCodeCommand, SendAuthenticationCodeOutput>
	{
        private readonly ICustomerRepository customerRepository;
        private readonly IMediator mediator;

		public SendAuthenticationCodeHandler(ICustomerRepository customerRepository, IMediator mediator)
		{
			this.customerRepository = customerRepository;
			this.mediator = mediator;
		}

		public async Task<ResponseBase<SendAuthenticationCodeOutput>> Handle(SendAuthenticationCodeCommand request, CancellationToken cancellationToken)
		{
			var customer = await customerRepository.GetByEmail(request.Email);
			if (customer == null)
				return new ResponseBase<SendAuthenticationCodeOutput>(new SendAuthenticationCodeOutput(false, request.Email));

			customer.GenerateAuthCode();
			customerRepository.Update(customer);
			await customerRepository.CommitAsync(cancellationToken);

			await mediator.Send(new SendAuthenticationCodeByEmailCommand(customer.FirstName, customer.LastName, customer.Email, customer.AuthCode, AuthenticationType.Login), cancellationToken);

			return new ResponseBase<SendAuthenticationCodeOutput>(new SendAuthenticationCodeOutput(true, request.Email));
		}
	}
}
