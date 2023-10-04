namespace CleanArchitectureTemplate.SharedKernel.Types
{
    public class BaseAuditEntity<TKey> : BaseEntity<TKey>
	{
        public BaseAuditEntity()
        {
            CreatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { get; private set; }
        
        public DateTime? ModifiedAt { get; private set; }
        
        public DateTime? DeletedAt { get; private set; }
        
        public bool IsDeleted { get; private set; }

        public void Modified()
        {
            ModifiedAt = DateTime.Now;
        }

        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }
}
