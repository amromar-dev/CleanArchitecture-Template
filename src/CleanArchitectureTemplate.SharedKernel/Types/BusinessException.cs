using System.Net;

namespace CleanArchitectureTemplate.SharedKernel.Types
{
    public class BusinessException : Exception
    {
        public BusinessException(string message)
            : base(message)
        {
            Status = HttpStatusCode.BadRequest;
        }

        public BusinessException(HttpStatusCode status, string message) : base(message)
        {
            Status = status;
        }

        public HttpStatusCode Status { get; }
    }
}
