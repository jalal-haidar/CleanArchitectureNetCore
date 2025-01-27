using CleanArchitectureNetCore.Application.Contracts.Repositories;

namespace CleanArchitectureNetCore.Application.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRoleRepository RoleRepository { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        void SaveChanges();
        void RevertChanges();
    }
}
