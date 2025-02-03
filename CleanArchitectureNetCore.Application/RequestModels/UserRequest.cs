using CleanArchitectureNetCore.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureNetCore.Application.RequestModels
{
    public class UserRequest
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public char Gender { get; set; }
        [Required]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        [Required]
        public int RoleId { get; set; }




        public User ToUser()
        {
            return new User
            {
                FirstName = FirstName,
                LastName = LastName,
                DateOfBirth = Dob,
                Email = Email,
                Username = Username,
                RoleId = RoleId,
                //PatientInfo = new PatientInfo
                //{
                //    FirstName = FirstName,
                //    LastName = LastName,

                //}
      
            };
        }
    }
}
