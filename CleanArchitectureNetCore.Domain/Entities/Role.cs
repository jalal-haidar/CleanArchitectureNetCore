using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitectureNetCore.Common.Entities;

using CleanArchitectureNetCore.Domain.DTOs;

namespace CleanArchitectureNetCore.Domain.Entities
{
    public class Role : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RoleDto ToDto()
        {
            return new RoleDto
            {
                Name = Name,
                Description = Description,
                Id = Id,
            };
        }
    }
}
