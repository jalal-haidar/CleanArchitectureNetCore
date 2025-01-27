using CleanArchitectureNetCore.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureNetCore.Application.RequestModels
{
    public class UserRequest
    {

        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public int RoleId { get; set; }


        public User ToUser()
        {
            return new User
            {
                Email = Email,
                Username = Username,
                RoleId = RoleId,
      
            };
        }
    }
}
