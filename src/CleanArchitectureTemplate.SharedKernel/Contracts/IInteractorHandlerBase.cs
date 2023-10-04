using MediatR;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.SharedKernel.Contracts
{
    public interface IInteractorHandlerBase<in TQuery, TResult> : IRequestHandler<TQuery, ResponseBase<TResult>>
        where TQuery : IInteractorBase<TResult>
    {
    }
}