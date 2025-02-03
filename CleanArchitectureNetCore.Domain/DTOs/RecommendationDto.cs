using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitectureNetCore.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CleanArchitectureNetCore.Domain.DTOs
{
    public class RecommendationDto
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Date { get; set; }
        public long PatientInfoId { get; set; }
        public PatientInfo PatientInfo { get; set; }
        public RecommendationDto(Recommendation recommendation)
        {
            Description = recommendation.Description;
            IsCompleted = recommendation.IsCompleted;
            Date = recommendation.Date;
        }
    }
}
