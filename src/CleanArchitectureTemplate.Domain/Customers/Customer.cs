using CleanArchitectureTemplate.Domain.Customers.Rules;
using CleanArchitectureTemplate.Domain.Customers.ValueTypes;
using CleanArchitectureTemplate.SharedKernel.Types;
using CleanArchitectureTemplate.SharedKernel.Utilities;

namespace CleanArchitectureTemplate.Domain.Customers
{
    public class Customer : BaseAuditEntity<int>
    {
        private Customer()
        {

        }

        public Customer(string email, string firstName, string lastName, string referralCodeUsed)
        {
            CheckRule(new EmailValidationRule(email));
            CheckRule(new FirstNameValidationRule(firstName));
            CheckRule(new LastNameValidationRule(lastName));

            Email = email.ToLower();
            FirstName = firstName;
            LastName = lastName;
            ReferralCode = RandomGenerator.GenerateRandomMixed(10);
            ReferralCodeUsed = string.IsNullOrWhiteSpace(referralCodeUsed) ? null : referralCodeUsed?.ToUpper();
            GenerateAuthCode();
        }

        public string Email { get; private set; }

        public string UserName { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

		public string FullName => $"{FirstName} {LastName}";

		public string Phone { get; private set; }
		
		public string OnePassNumber { get; private set; }

        public DateOnly? BirthDate { get; private set; }

        public string Country { get; private set; }

        public string City { get; private set; }

        public string ProfileBanner { get; private set; }

        public string ProfilePicture { get; private set; }

        public string FacebookUserName { get; private set; }

        public string InstagramUserName { get; private set; }

        public bool SubscribeForUpdates { get; private set; }

        public string AuthCode { get; private set; }

        public string ReferralCode { get; private set; }

        public string ReferralCodeUsed { get; private set; }

        public string ReferralCodeQRUrl { get; private set; }

        public DateTime? LastLoginAt { get; private set; }

        public EmailChangeRequest EmailChangeRequest { get; private set; }

        #region Methods

        public void GenerateAuthCode()
        {
            AuthCode = RandomGenerator.GenerateRandomNumber(6).ToString();
            Modified();
        }

        public void ValidateAuthCode(string authCode)
        {
            if (AuthCode != authCode)
                throw new BusinessException("Incorrect code entered. Please try again.");

            AuthCode = $"{AuthCode}_Used";
            Modified();
        }

        public void MarkAsLoggedIn()
        {
            LastLoginAt = DateTime.Now;
            Modified();
        }

        public void UpdateProfile(string userName, string firstName, string lastName, string phone, string onePassNumber, DateOnly? birthDate, string country, string city, string profileBanner, string profilePicture, string facebookUserName, string instagramUserName, bool subscribeForUpdates)
        {
            CheckRule(new FirstNameValidationRule(firstName));
            CheckRule(new LastNameValidationRule(lastName));

            if (birthDate.HasValue)
                CheckRule(new BirthdateValidationRule(birthDate.Value));

            UserName = userName?.ToLower();
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            OnePassNumber = onePassNumber;
            BirthDate = birthDate;
            Country = country;
            City = city;
            ProfileBanner = profileBanner;
            ProfilePicture = profilePicture;
            FacebookUserName = facebookUserName;
            InstagramUserName = instagramUserName;
            SubscribeForUpdates = subscribeForUpdates;
            Modified();
        }

        public void AddEmailChangeRequest(string email)
        {
            CheckRule(new EmailValidationRule(email));

            EmailChangeRequest = new EmailChangeRequest()
            {
                Email = email,
                VerificationCode = RandomGenerator.GenerateRandomNumber(6).ToString(),
            };

            Modified();
        }

        public void ConfirmChangeEmail(string verificationCode)
        {
            if (EmailChangeRequest == null)
                throw new BusinessException("Sorry, No email change request exist");

            if (EmailChangeRequest.VerificationCode != verificationCode)
                throw new BusinessException("Incorrect code entered. Please try again.");

            Email = EmailChangeRequest.Email;
            EmailChangeRequest = null;
            Modified();
        }

        public void UpdateReferralCodeQR(string url)
        {
            if (string.IsNullOrEmpty(this.ReferralCodeQRUrl) == false)
                return;

            this.ReferralCodeQRUrl = url;
            Modified();
        }

        #endregion
    }
}
