namespace CleanArchitectureTemplate.Application.Interactors.Account.Outputs
{
	public class ProfileOutput 
	{
		public string Email { get; set; }

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

		public string ReferralCode { get; set; }

		public bool SubscribeForUpdates { get; set; }
    }
}
