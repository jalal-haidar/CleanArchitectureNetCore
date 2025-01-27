using CleanArchitectureNetCore.Application.Common.Contracts.Repositories;
using CleanArchitectureNetCore.Application.Contracts.Repositories;
using System;

namespace CleanArchitectureNetCore.Application.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRoleRepository RoleRepository { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IAuthTokenRepository AuthTokens { get; }
        void SaveChanges();
        void RevertChanges();
    }
}
