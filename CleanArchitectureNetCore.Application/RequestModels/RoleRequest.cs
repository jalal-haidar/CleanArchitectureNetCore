using CleanArchitectureNetCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.RequestModels
{
    public class RoleRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Role ToRole()
        {
            return new Role
            {
                Name = Name,
                Description = Description
            };
        }
    }
}
