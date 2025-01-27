using CleanArchitectureNetCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.Contracts.Repositories
{
    public interface IAuthTokenRepository : IRepository<AuthToken>
    { }
}
