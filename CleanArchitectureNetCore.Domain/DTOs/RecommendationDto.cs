using CleanArchitectureNetCore.Domain.Entities;
using System;

namespace CleanArchitectureNetCore.Domain.DTOs
{
    public class RecommendationDto:BaseDto
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Date { get; set; }
        public long PatientId { get; set; }
        public RecommendationDto(Recommendation recommendation)
        {
            Id = recommendation.Id;
            Description = recommendation.Description;
            IsCompleted = recommendation.IsCompleted;
            Date = recommendation.Date;
            PatientId = recommendation.PatientId;
        }
    }
}
