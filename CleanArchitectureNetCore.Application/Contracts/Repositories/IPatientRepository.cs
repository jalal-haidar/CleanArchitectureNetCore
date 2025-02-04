using CleanArchitectureNetCore.Domain.Entities;
using System.Linq;

namespace CleanArchitectureNetCore.Application.Contracts.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        IQueryable<Patient> GetAll();
        Patient GetById(long id);
    }
}
