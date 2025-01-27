using CleanArchitectureNetCore.Common.Entities;
using System.Linq;

namespace CleanArchitectureNetCore.Application.Contracts.Repositories
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity Get(long id);
        IQueryable<TEntity> Get();
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Delete(long id);
    }
}
