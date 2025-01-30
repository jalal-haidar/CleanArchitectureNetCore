using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Common;
using CleanArchitectureNetCore.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Infrastructure.Persistence.EfMariaDb.Repositories
{
    class AuthTokenRepository : Repository<AuthToken>, IAuthTokenRepository
    {
        public AuthTokenRepository(AppDbContext context, AuthUser authUser, IConfiguration configuration) : base(context, configuration)
        {

        }
    }
}
