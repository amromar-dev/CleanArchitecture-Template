using CleanArchitectureTemplate.SharedKernel.Interfaces;

namespace CleanArchitectureTemplate.Domain.Customers.Rules
{
    public class LastNameValidationRule : IBusinessRule
    {
		const int MaxLength = 50;
		private readonly string lastName;

        public LastNameValidationRule(string lastName)
        {
            this.lastName = lastName;
        }

        public bool IsBroken() => string.IsNullOrWhiteSpace(lastName) || lastName.Length > MaxLength;

		public string Message => $"LastName is required with max length of {MaxLength} chars";
	}
}
