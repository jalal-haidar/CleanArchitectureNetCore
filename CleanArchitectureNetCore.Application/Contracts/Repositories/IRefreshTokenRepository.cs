using CleanArchitectureNetCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.Contracts.Repositories
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        public IEnumerable<RefreshToken> GetByUserId(long userId);
        public IEnumerable<RefreshToken> Update(IEnumerable<RefreshToken> token);
        public RefreshToken GetByToken(string token);
    }
}
