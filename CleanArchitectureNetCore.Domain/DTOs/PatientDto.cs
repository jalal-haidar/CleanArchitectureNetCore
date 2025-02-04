using CleanArchitectureNetCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitectureNetCore.Domain.DTOs
{
    public class PatientDto:BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public char Gender { get; set; }
        public int Age { get; set; }
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastVisit { get; set; }
        public DateTime NextVisit { get; set; } 
        public ICollection<RecommendationDto> Recommendations { get; set; }

        public PatientDto(Patient patient)
        {
            Id = patient.Id;
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            Gender = patient.Gender;
            Email = patient.Email;
            LastVisit = patient.LastVisit;
            NextVisit = patient.NextVisit;
            Description = patient.Description;
            Age =   DateTime.UtcNow.Year - patient.DateOfBirth.Year;
            Recommendations = patient.Recommendations?.Where(x=>x.IsActive).Select(x=>x.ToDto()).ToList();
        }
    }
}
