using MediatR;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.SharedKernel.Contracts
{
    public class InteractorExecution : IInteractorExecution
    {
        private readonly IMediator mediator;

        public InteractorExecution(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public Task<ResponseBase<TResult>> ExecuteAsync<TResult>(IInteractorBase<TResult> interactor)
        {
            return mediator.Send(interactor);
        }
    }
}