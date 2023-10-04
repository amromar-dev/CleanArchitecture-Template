namespace CleanArchitectureTemplate.SharedKernel.Types
{
	public class ExceptionResponseBase
	{
		public ExceptionResponseBase(string message)
		{
			Message = message;
		}

		public string Message { get; set; }

	}
}
