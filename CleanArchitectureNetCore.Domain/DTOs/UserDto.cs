using CleanArchitectureNetCore.Domain.Entities;
using System;


namespace CleanArchitectureNetCore.Domain.DTOs
{
    public class UserDto : BaseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
        public long RoleId { get; set; }

        public UserDto() { }
        public UserDto(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            this.Id = user.Id;

            Name = user.Name;
            Email = user.Email;
            RoleName = user.Role?.Name;
            RoleId = user.Role?.Id ?? 0;
        }
    }
}
