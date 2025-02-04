using CleanArchitectureNetCore.Domain.DTOs;
using System;
using System.Collections.Generic;

namespace CleanArchitectureNetCore.Domain.Entities
{
    public class Patient: UserBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Description { get; set; }
        public Char Gender { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }

        public DateTime LastVisit { get; set; }
        public DateTime NextVisit { get; set; }

        public ICollection<Recommendation> Recommendations { get; set; } = new List<Recommendation>();

        public PatientDto ToDto() => new(this);
    }
}
