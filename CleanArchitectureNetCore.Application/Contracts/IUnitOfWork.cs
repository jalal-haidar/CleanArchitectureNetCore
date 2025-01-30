using CleanArchitectureNetCore.Application.Contracts.Repositories;

namespace CleanArchitectureNetCore.Application.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IPatientRepository PatientRepository { get; }
        IRoleRepository RoleRepository { get; }
        IAuthTokenRepository AuthTokens { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IRecommendationRepository RecommendationRepository { get; }


        void SaveChanges();
        void RevertChanges();
    }
}
