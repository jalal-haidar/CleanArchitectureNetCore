namespace CleanArchitectureNetCore.Common.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public BaseEntity()
        {
            IsActive = true;
        }
    }
}
