using CleanArchitectureNetCore.Application.RequestModels;
using CleanArchitectureNetCore.Domain.DTOs;
using System.Collections.Generic;

namespace CleanArchitectureNetCore.Application.Contracts
{
    public interface IUserService
    {
        UserDto Create(UserRequest request);
        UserDto Update(long id, UserRequest request);
        IEnumerable<UserDto> Get();
        UserDto Get(long id);
        UserDto Delete(long id);
    }
}
