using CleanArchitectureTemplate.Domain.Customers;
using CleanArchitectureTemplate.Infrastructure.Persistence.Repositories.Base;
using CleanArchitectureTemplate.Domain.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories
{
	public class CustomerRepository : BaseRepository<Customer, int>, ICustomerRepository
	{
		public CustomerRepository(EFDbContext context) : base(context)
		{
		}

		public Task<Customer> GetByEmail(string email)
		{
			return dbSet.Where(s => s.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
		}

        public Task<Customer> GetByReferralCode(string referralCode)
        {
            return dbSet.Where(s => s.ReferralCode.ToUpper() == referralCode.ToUpper()).FirstOrDefaultAsync();
        }

        public Task<bool> ReferralCodeExist(string referralCode)
		{
			return dbSet.Where(s => s.ReferralCode.ToUpper() == referralCode.ToUpper()).AnyAsync();
		}
		
		public Task<bool> UserNameExist(string userName)
		{
			return dbSet.Where(s => s.UserName != null && s.UserName.ToLower() == userName.ToLower()).AnyAsync();
		}
	}
}
