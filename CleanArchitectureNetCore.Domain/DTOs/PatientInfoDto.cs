using CleanArchitectureNetCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Domain.DTOs
{
    public class PatientInfoDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public char Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastVisit { get; set; }
        public DateTime NextVisit { get; set; } 
        public ICollection<Recommendation> Recommendations { get; set; }

        public PatientInfoDto(PatientInfo patient)
        {
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            Gender = patient.Gender;
            //DateOfBirth = patient.DateOfBirth;
            LastVisit = patient.LastVisit;
            NextVisit = patient.NextVisit;
            Recommendations = patient.Recommendations;
        }
    }
}
