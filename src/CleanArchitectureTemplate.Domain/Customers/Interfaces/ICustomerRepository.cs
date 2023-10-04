using CleanArchitectureTemplate.SharedKernel.Interfaces;

namespace CleanArchitectureTemplate.Domain.Customers.Interfaces
{
	public interface ICustomerRepository : IBaseRepository<Customer, int>
	{
		Task<Customer> GetByEmail(string email);
		Task<Customer> GetByReferralCode(string referralCode);
        Task<bool> ReferralCodeExist(string referralCode);
		Task<bool> UserNameExist(string userName);
	}
}
