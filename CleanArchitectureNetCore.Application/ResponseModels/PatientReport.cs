using CleanArchitectureNetCore.Domain.DTOs;
using System;
using System.Collections.Generic;

namespace CleanArchitectureNetCore.Application.ResponseModels
{
    public class PatientReportResponse
    {
        public long PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientEmail { get; set; }

        public DateTime? LastVisit { get; set; }
        public DateTime? NextVisit { get; set; }
        public IEnumerable<RecommendationDto> Recommendations { get; set; }
    }
}
