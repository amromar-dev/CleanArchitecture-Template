using MediatR;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Application.Common.BackgroundJobs
{
	public interface IBackgroundScheduleJob
	{
		string EnqueueJob<TResult>(IRequest<ResponseBase<TResult>> request);
		string ContinueJobWith<TResult>(string parentJobId, IRequest<ResponseBase<TResult>> request);
		string Schedule<TResult>(IRequest<ResponseBase<TResult>> request, DateTimeOffset enqueuAt);
	}
}