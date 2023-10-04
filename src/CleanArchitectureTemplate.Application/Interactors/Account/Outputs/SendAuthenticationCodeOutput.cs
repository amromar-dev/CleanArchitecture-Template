namespace CleanArchitectureTemplate.Application.Interactors.Account.Outputs
{
	public class SendAuthenticationCodeOutput
	{
		public SendAuthenticationCodeOutput()
		{

		}

		public SendAuthenticationCodeOutput(bool userExist, string email)
		{
			UserExist = userExist;
			Email = email;
		}

		/// <summary>
		/// 
		/// </summary>
		public bool UserExist { get; set; }

		public string Email { get; set; }
	}
}
