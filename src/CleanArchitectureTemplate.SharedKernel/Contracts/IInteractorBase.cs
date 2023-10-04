using MediatR;
using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.SharedKernel.Contracts
{
    public interface IInteractorBase<TResult> : IRequest<ResponseBase<TResult>>
    {
    }
}
