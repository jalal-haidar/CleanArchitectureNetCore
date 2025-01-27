using CleanArchitectureNetCore.Application.ResponseModels;
using CleanArchitectureNetCore.Domain.DTOs;
using CleanArchitectureNetCore.Domain.Entities;

namespace CleanArchitectureNetCore.Application.Contracts
{
    public interface IAuthService
    {
        LoginResponse Authenticate(string username, string password, string device, string deviceId);
        UserDto Get(long id);
        bool HasDefaultPassword(long userId);
        void Logout(long userId);
        LoginResponse RefreshToken(string token, string deviceId);
        User GeneratePassword(User user);
        string GetAuthToken(long userId);
        LoginResponse Authenticate(string token, string device, string deviceId);
    }
}
