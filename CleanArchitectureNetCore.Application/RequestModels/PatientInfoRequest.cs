using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.RequestModels
{
    public class PatientInfoRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string Description {get; set; }
        public DateTime LastVisit { get; set; }
        public DateTime NextVisit { get; set; }
        public char Gender { get; set; }
        [Required]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Contact { get; set; }


    }
}
