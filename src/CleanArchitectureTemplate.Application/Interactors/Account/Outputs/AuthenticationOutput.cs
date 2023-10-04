namespace CleanArchitectureTemplate.Application.Interactors.Account.Outputs
{
	public class AuthenticationOutput 
	{
		public string AccessToken { get; set; }

		public double ExpiresIn { get; set; }

		public string RefreshToken { get; set; }

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}
