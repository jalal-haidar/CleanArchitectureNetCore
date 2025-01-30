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
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public ICollection<Recommendation> Recommendations { get; set; }

        public PatientInfoDto(PatientInfo patient)
        {
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            DateOfBirth = patient.DateOfBirth;
            Gender = patient.Gender;
            Address = patient.Address;
            PhoneNumber = patient.PhoneNumber;
            Email = patient.Email;
            Recommendations = patient.Recommendations;
        }
    }
}
