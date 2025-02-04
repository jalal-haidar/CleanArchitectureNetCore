using CleanArchitectureNetCore.Common.Entities;

namespace CleanArchitectureNetCore.Domain.Entities
{
    public abstract class UserBase:AuditableBaseEntity
    {

        public string Email { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
