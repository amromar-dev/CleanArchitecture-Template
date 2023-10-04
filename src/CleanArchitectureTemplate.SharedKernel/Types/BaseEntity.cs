using CleanArchitectureTemplate.SharedKernel.Interfaces;

namespace CleanArchitectureTemplate.SharedKernel.Types
{
    public abstract class BaseEntity<TKey>
    {
        public BaseEntity()
        {

        }

        public TKey Id { get; protected set; }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
                throw new BusinessException(rule.Message);
        }
    }
}
