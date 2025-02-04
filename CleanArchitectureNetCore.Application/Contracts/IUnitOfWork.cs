using CleanArchitectureNetCore.Application.Contracts.Repositories;

namespace CleanArchitectureNetCore.Application.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IPatientRepository PatientRepository { get; }
        IRoleRepository RoleRepository { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IRecommendationRepository RecommendationRepository { get; }


        void SaveChanges();
        void RevertChanges();
    }
}
