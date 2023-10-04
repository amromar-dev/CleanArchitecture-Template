using AutoMapper;
using Microsoft.AspNetCore.Http;
using CleanArchitectureTemplate.Application.Interactors.Account.Outputs;
using CleanArchitectureTemplate.Domain.Customers.Interfaces;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Application.Interactors.Account.Queries
{
    public class GetUserProfileQuery : IInteractorBase<ProfileOutput>
    {

    }

    public class GetUserProfileHandler : IInteractorHandlerBase<GetUserProfileQuery, ProfileOutput>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IHttpContextAccessor context;
        private readonly IMapper mapper;

        public GetUserProfileHandler(ICustomerRepository customerRepository, IHttpContextAccessor context, IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ResponseBase<ProfileOutput>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = context.GetUserId();
            var cusomter = await customerRepository.FindAsync(userId);
            if (cusomter == null)
                throw new BusinessException("User not found");

            var response = mapper.Map<ProfileOutput>(cusomter);

            return new ResponseBase<ProfileOutput>(response);
        }
    }
}
