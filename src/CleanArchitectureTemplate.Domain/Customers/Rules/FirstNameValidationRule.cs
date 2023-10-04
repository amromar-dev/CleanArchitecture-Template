using CleanArchitectureTemplate.SharedKernel.Interfaces;

namespace CleanArchitectureTemplate.Domain.Customers.Rules
{
    public class FirstNameValidationRule : IBusinessRule
    {
        const int MaxLength = 50;
        private readonly string firstName;

        public FirstNameValidationRule(string firstName)
        {
            this.firstName = firstName;
        }

        public bool IsBroken() => string.IsNullOrWhiteSpace(firstName) || firstName.Length > MaxLength;

        public string Message => $"Firstname is required with max length {MaxLength} chars";
    }
}
