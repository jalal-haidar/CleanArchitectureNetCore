using System;

namespace CleanArchitectureNetCore.Application.RequestModels.Patients
{
    public class PatientRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public char Gender { get; set; }
        public int Age { get; set; }
        public DateTime LastVisit { get; set; }
        public DateTime NextVisit { get; set; }
        //public List<RecommendationRequest> Recommendations { get; set; }
    }
}
