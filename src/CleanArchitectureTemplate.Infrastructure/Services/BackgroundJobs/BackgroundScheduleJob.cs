using Hangfire;
using MediatR;
using Newtonsoft.Json;
using CleanArchitectureTemplate.Application.Common.BackgroundJobs;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Infrastructure.Services.BackgroundJobs
{
	public class BackgroundScheduleJob : IBackgroundScheduleJob
	{
		private readonly IMediator mediator;
		
		public BackgroundScheduleJob(IMediator mediator)
		{
			this.mediator = mediator;
		}

		public string EnqueueJob<TResult>(IRequest<ResponseBase<TResult>> request)
		{
			return BackgroundJob.Enqueue(() => new BackgroundScheduleJob(mediator).EnqueueMyRequest(request.GetType(), JsonConvert.SerializeObject(request)));
		}

		public string ContinueJobWith<TResult>(string parentJobId, IRequest<ResponseBase<TResult>> request)
		{
			return BackgroundJob.ContinueJobWith(parentJobId, () => new BackgroundScheduleJob(mediator).EnqueueMyRequest(request.GetType(), JsonConvert.SerializeObject(request)));
		}

		public string Schedule<TResult>(IRequest<ResponseBase<TResult>> request, DateTimeOffset enqueuAt)
		{
			return BackgroundJob.Schedule(() => new BackgroundScheduleJob(mediator).EnqueueMyRequest(request.GetType(), JsonConvert.SerializeObject(request)), enqueuAt);
		}

		public Task EnqueueMyRequest(Type type, string json)
		{
			var command = JsonConvert.DeserializeObject(json, type);
			return mediator.Send(command);
		}
	}
}
