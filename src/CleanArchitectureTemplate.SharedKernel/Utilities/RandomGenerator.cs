namespace CleanArchitectureTemplate.SharedKernel.Utilities
{
	public static class RandomGenerator
	{
		public static int GenerateRandomNumber(int length)
		{
			Random random = new();

			var minValue = (int)Math.Pow(10, length - 1);
			var maxValue = (int)Math.Pow(10, length) - 1;

			return random.Next(minValue, maxValue);
		}

		public static string GenerateRandomMixed(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

			Random random = new();

			return new(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public static string GenerateRandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

			Random random = new();

			return new(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
