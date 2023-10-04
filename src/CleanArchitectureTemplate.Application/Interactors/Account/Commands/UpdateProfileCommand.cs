using AutoMapper;
using Microsoft.AspNetCore.Http;
using CleanArchitectureTemplate.Application.Interactors.Account.Outputs;
using CleanArchitectureTemplate.Domain.Customers;
using CleanArchitectureTemplate.Domain.Customers.Interfaces;
using CleanArchitectureTemplate.SharedKernel.Contracts;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Application.Interactors.Account.Commands
{
	public class UpdateProfileCommand : IInteractorBase<ProfileOutput>
	{
		public string UserName { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Phone { get; set; }

		public string OnePassNumber { get; set; }

		public DateOnly? BirthDate { get; set; }

		public string Country { get; set; }

		public string City { get; set; }

		public string ProfileBanner { get; set; }

		public string ProfilePicture { get; set; }

		public string FacebookUserName { get; set; }

		public string InstagramUserName { get; set; }

		public bool SubscribeForUpdates { get; set; }
	}

    public class UpdateProfileHandler : IInteractorHandlerBase<UpdateProfileCommand, ProfileOutput>
	{
		private readonly ICustomerRepository customerRepository;
		private readonly IHttpContextAccessor context;
		private readonly IMapper mapper;

		public UpdateProfileHandler(ICustomerRepository customerRepository, IHttpContextAccessor context, IMapper mapper)
		{
			this.customerRepository = customerRepository;
			this.context = context;
			this.mapper = mapper;
		}

		public async Task<ResponseBase<ProfileOutput>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
		{
			var userId = context.GetUserId();
			var customer = await customerRepository.FindAsync(userId);
			if (customer == null)
				throw new BusinessException("User not found");

			await GuardAgainstDuplicatedUserName(customer, request.UserName);

			customer.UpdateProfile(request.UserName,
				request.FirstName,
				request.LastName,
				request.Phone,
				request.OnePassNumber,
				request.BirthDate,
				request.Country,
				request.City,
				request.ProfileBanner,
				request.ProfilePicture,
				request.FacebookUserName,
				request.InstagramUserName,
				request.SubscribeForUpdates);

			customerRepository.Update(customer);

			await customerRepository.CommitAsync(cancellationToken);

			var response = mapper.Map<ProfileOutput>(customer);
			return new ResponseBase<ProfileOutput>(response);
		}


		#region Private Methods

		private async Task GuardAgainstDuplicatedUserName(Customer customer, string userName)
		{
			if (string.IsNullOrEmpty(userName))
				return;

			if (customer.UserName?.ToLower() == userName?.ToLower())
				return;

			var usernameExist = await customerRepository.UserNameExist(userName);
			if (usernameExist == false)
				return;

			throw new BusinessException("Username is already exist");
		}

		#endregion
	}
}
