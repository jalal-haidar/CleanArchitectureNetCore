using CleanArchitectureNetCore.Common.Entities;
using CleanArchitectureNetCore.Domain.DTOs;
using System;

namespace CleanArchitectureNetCore.Domain.Entities
{
    public class Recommendation : BaseEntity
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Date { get; set; }
        public long PatientInfoId { get; set; }
        public PatientInfo PatientInfo { get; set; }

        public RecommendationDto ToDto() => new(this);

    }
}
