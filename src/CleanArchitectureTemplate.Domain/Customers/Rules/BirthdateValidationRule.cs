using CleanArchitectureTemplate.SharedKernel.Interfaces;

namespace CleanArchitectureTemplate.Domain.Customers.Rules
{
    public class BirthdateValidationRule : IBusinessRule
    {
		private readonly DateOnly birthDate;

        public BirthdateValidationRule(DateOnly birthDate)
        {
            this.birthDate = birthDate;
        }

        public bool IsBroken() => birthDate >= DateOnly.FromDateTime(DateTime.Now.Date);

		public string Message => $"Invalid birth date entered. Please enter a valid date in the past.";
	}
}
