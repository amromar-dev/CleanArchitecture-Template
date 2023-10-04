namespace CleanArchitectureTemplate.Infrastructure.Services.EmailServices
{
	public class SendGridConfiguration
	{
		public static string ConfigurationName = "SendGrid";

		public string ApiKey { get; set; }

		public string FromEmail { get; set; }
		
		public string ReplyEmail { get; set; }
	}
}
