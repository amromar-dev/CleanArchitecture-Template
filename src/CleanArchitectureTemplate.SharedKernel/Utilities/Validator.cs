using System.Text.RegularExpressions;

namespace CleanArchitectureTemplate.SharedKernel.Utilities
{
	public static class Validator
	{
		public static bool IsValidEmail(string email)
		{
			string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

			return Regex.IsMatch(email, pattern);
		}
	}
}
