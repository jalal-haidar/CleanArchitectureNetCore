using CleanArchitectureNetCore.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureNetCore.Application.RequestModels
{
    public class UserRequest
    {

        public string Name { get; set; }
        //public char Gender { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int RoleId { get; set; }




        public User ToUser()
        {
            return new User
            {
                Name = Name,
                Email = Email,
                RoleId = RoleId,
            };
        }
    }
}
