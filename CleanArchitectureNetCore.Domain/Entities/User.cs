using System;
using CleanArchitectureNetCore.Common.Entities;
using CleanArchitectureNetCore.Domain.DTOs;


namespace CleanArchitectureNetCore.Domain.Entities
{
    public class User : UserBase
    {

        public string Name { get; set; }

        public Role Role { get; set; }
        public long RoleId { get; set; }


        public UserDto ToDto()
        {
            return new UserDto(this);
        }
    }
}
