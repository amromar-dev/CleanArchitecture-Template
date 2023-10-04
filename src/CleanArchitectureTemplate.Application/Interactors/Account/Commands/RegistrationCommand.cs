using AutoMapper;
using MediatR;
using CleanArchitectureTemplate.Application.Common.BackgroundJobs;
using CleanArchitectureTemplate.Application.Interactors.Account.Outputs;
using CleanArchitectureTemplate.Domain.Customers;
using CleanArchitectureTemplate.Domain.Customers.Interfaces;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Application.Interactors.Account.Commands
{
    public class RegistrationCommand : IInteractorBase<RegistrationOutput>
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ReferralCode { get; set; }
    }

    public class RegistrationHandler : IInteractorHandlerBase<RegistrationCommand, RegistrationOutput>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly IBackgroundScheduleJob backgroundScheduleJob;

        public RegistrationHandler(ICustomerRepository customerRepository, IMediator mediator, IMapper mapper, IBackgroundScheduleJob backgroundScheduleJob)
        {
            this.customerRepository = customerRepository;
            this.mediator = mediator;
            this.mapper = mapper;
            this.backgroundScheduleJob = backgroundScheduleJob;
        }

        public async Task<ResponseBase<RegistrationOutput>> Handle(RegistrationCommand request, CancellationToken cancellationToken)
        {
            var existCustomer = await customerRepository.GetByEmail(request.Email);
            if (existCustomer != null)
                throw new BusinessException("This email is already exist.");

            await GuardAgainstInvalidReferralCode(request.ReferralCode);

            var customer = new Customer(request.Email, request.FirstName, request.LastName, request.ReferralCode);
            customerRepository.Add(customer);

            await customerRepository.CommitAsync(cancellationToken);

            await SendAuthenticationCode(customer);
            
            var response = mapper.Map<RegistrationOutput>(customer);
            return new ResponseBase<RegistrationOutput>(response);
        }

        #region Private Methods

        private async Task GuardAgainstInvalidReferralCode(string referralCode)
        {
            if (string.IsNullOrWhiteSpace(referralCode))
                return;

            var referralCodeExist = await customerRepository.ReferralCodeExist(referralCode);
            if (referralCodeExist)
                return;

            throw new BusinessException("Referral code is not valid");
        }

        private Task SendAuthenticationCode(Customer customer)
        {
            return mediator.Send(new SendAuthenticationCodeByEmailCommand(customer.FirstName, customer.LastName, customer.Email, customer.AuthCode, AuthenticationType.Registration));
        }

        #endregion
    }
}
