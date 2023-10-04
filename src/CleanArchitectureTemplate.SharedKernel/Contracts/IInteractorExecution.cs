using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.SharedKernel.Contracts
{
    public interface IInteractorExecution
    {
        Task<ResponseBase<TResult>> ExecuteAsync<TResult>(IInteractorBase<TResult> interactor);
    }
}