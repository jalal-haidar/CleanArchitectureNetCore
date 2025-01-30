using CleanArchitectureNetCore.Common.Entities;
using CleanArchitectureNetCore.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Domain.Entities
{
    public class PatientInfo : AuditableBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

       public PatientInfoDto ToDto() => new(this);
    }
}
