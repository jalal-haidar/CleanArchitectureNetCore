using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Infrastructure.Persistence.EfMariaDb.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {

        }
    }
}
