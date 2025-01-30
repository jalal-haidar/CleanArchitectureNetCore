using CleanArchitectureNetCore.Common.Entities;
using CleanArchitectureNetCore.Domain.DTOs;


namespace CleanArchitectureNetCore.Domain.Entities
{
    public class User : AuditableBaseEntity
    {

        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public long RoleId { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public PatientInfo PatientInfo { get; set; }
        public long? PatientInfoId { get; set; }

       



        public UserDto ToDto()
        {
            return new UserDto(this);
        }
    }
}
