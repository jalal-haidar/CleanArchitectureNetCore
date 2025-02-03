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

        public string Description { get; set; }
        public Char Gender { get; set; }

        //public DateTime DateOfBirth { get; set; }
        public DateTime LastVisit { get; set; }
        public DateTime NextVisit { get; set; }

        public ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

        public long UserId { get; set; }
        public User User { get; set; }

        public PatientInfoDto ToDto() => new(this);
    }
}
