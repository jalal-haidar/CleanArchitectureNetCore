using CleanArchitectureNetCore.Common.Entities;
using CleanArchitectureNetCore.Application.Contracts.Repositories;  
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Infrastructure.Persistence.Repositories
{
    public class Repository<TEntity> : AdoConnection, IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly AppDbContext _Context;
        public Repository(AppDbContext context, IConfiguration configuration = null) : base(configuration)
        {
            _Context = context;
        }
        public virtual TEntity Add(TEntity entity)
        {
            entity.IsActive = true;
            var entry = _Context.Set<TEntity>().Add(entity);
            return entry.Entity;
        }

        public virtual TEntity Delete(long id)
        {
            if (id > 0)
            {
                var entity = _Context.Set<TEntity>().Find(id);
                if (entity == null) return null;
                entity.IsActive = false;
                return Update(entity);
            }
            return null;
        }

        public virtual IQueryable<TEntity> Get()
        {
            return _Context.Set<TEntity>().Where(x => x.IsActive);
        }


        public virtual TEntity Get(long id)
        {
            return _Context.Set<TEntity>().FirstOrDefault(x => x.Id == id && x.IsActive);
        }

        public virtual TEntity Update(TEntity entity)
        {
            var entry = _Context.Set<TEntity>().Update(entity);
            //_Context.Entry(enr).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            return entry.Entity;
        }
    }
}
