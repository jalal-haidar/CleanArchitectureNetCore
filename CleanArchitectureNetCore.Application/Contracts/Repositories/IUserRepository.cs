using CleanArchitectureNetCore.Domain.Entities;

namespace CleanArchitectureNetCore.Application.Contracts.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User? Authenticate(string username, string password);
    }
}
