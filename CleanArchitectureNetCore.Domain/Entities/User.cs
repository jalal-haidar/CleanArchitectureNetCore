using System.Collections.Generic;
using System.Data;
using CleanArchitectureNetCore.Common.Entities;


namespace CleanArchitectureNetCore.Domain.Entities
{
    public class User : AuditableBaseEntity
    {

        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public long RoleId { get; set; }

    }
}
