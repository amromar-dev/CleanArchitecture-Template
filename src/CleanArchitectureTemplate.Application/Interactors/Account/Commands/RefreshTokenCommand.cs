using AutoMapper;
using CleanArchitectureTemplate.Application.Common.IdentityServices;
using CleanArchitectureTemplate.Application.Interactors.Account.Outputs;
using CleanArchitectureTemplate.Domain.Customers.Interfaces;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Application.Interactors.Account.Commands
{
	public class RefreshTokenCommand : IInteractorBase<AuthenticationOutput>
	{
		public string RefreshToken { get; set; }
	}

	public class RefreshTokenHandler : IInteractorHandlerBase<RefreshTokenCommand, AuthenticationOutput>
	{
        private readonly ICustomerRepository customerRepository;
		private readonly ITokenService tokenService;
		private readonly IMapper mapper;

		public RefreshTokenHandler(ICustomerRepository customerRepository, ITokenService tokenService, IMapper mapper)
		{
			this.customerRepository = customerRepository;
			this.tokenService = tokenService;
			this.mapper = mapper;
		}

		public async Task<ResponseBase<AuthenticationOutput>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
		{
			var token = await tokenService.GenerateToken(request.RefreshToken);

			var customer = await customerRepository.FindAsync(token.UserId);
			if (customer == null)
				throw new BusinessException(System.Net.HttpStatusCode.Unauthorized, "Not Valid Refresh Token");

			var response = mapper.Map<AuthenticationOutput>(customer);

			response.AccessToken = token.AccessToken;
			response.RefreshToken = token.RefreshToken;
			response.ExpiresIn = token.ExpiresIn;

			return new ResponseBase<AuthenticationOutput>(response);
		}
	}
}
