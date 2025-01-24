using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Domain.DTOs
{
    public class RoleDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
