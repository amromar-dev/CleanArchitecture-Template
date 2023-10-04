namespace CleanArchitectureTemplate.Domain.Customers.ValueTypes
{
	public record EmailChangeRequest
	{
		public string Email { get; set; }

		public string VerificationCode { get; set; }
	}
}
