using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitectureNetCore.Domain.Entities;


namespace CleanArchitectureNetCore.Domain.DTOs
{
    public class UserDto : BaseDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public long RoleId { get; set; }
        public PatientInfoDto PatientInfo { get; set; }

        public UserDto() { }
        public UserDto(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            this.Id = user.Id;

            Email = user.Email;
            Username = user.Username;
            RoleName = user.Role?.Name;
            RoleId = user.Role?.Id ?? 0;
            PatientInfo = user.PatientInfo.ToDto();
            if (PatientInfo != null)
            {
                PatientInfo.Email = user.Email;
            }

        }
    }
}
