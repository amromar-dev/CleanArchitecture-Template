using System.Net;

namespace CleanArchitectureTemplate.SharedKernel.Types
{
	public class ResponseBase<T>
	{
		public ResponseBase(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
		{
			StatusCode = statusCode;
			Success = statusCode == HttpStatusCode.OK;
			Data = data;
		}

		public HttpStatusCode StatusCode { get; private set; } = HttpStatusCode.OK;

		public bool Success { get; private set; }

		public T Data { get; private set; }

	}
}
