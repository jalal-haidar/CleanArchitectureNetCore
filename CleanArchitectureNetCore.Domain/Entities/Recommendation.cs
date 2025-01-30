using CleanArchitectureNetCore.Common.Entities;
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

        public Recommendation(Recommendation recommendation)
        {
            Id = recommendation.Id;
            Description = recommendation.Description;
            IsCompleted = recommendation.IsCompleted;
            Date = recommendation.Date;
        }
    }
}
