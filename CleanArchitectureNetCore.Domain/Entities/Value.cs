
using CleanArchitectureNetCore.Common.Entities;

namespace CleanArchitectureNetCore.Domain.Entities
{
    public class Value : AuditableBaseEntity
    {
        public string Val { get; set; }
    }
}
