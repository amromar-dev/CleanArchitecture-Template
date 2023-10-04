using CleanArchitectureTemplate.SharedKernel.Interfaces;
using CleanArchitectureTemplate.SharedKernel.Utilities;

namespace CleanArchitectureTemplate.Domain.Customers.Rules
{
    public class EmailValidationRule : IBusinessRule
    {
        private readonly string email;

        public EmailValidationRule(string email)
        {
            this.email = email;
        }

        public bool IsBroken() => string.IsNullOrWhiteSpace(email) || Validator.IsValidEmail(email) == false;

        public string Message => "Email is invalid";
    }
}
