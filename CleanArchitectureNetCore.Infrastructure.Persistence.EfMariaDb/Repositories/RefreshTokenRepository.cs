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
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly AppDbContext _Context;

        public RefreshTokenRepository(AppDbContext context, AuthUser authUser, IConfiguration configuration) : base(context, configuration)
        {
            _Context = context;
        }

        public RefreshToken GetByToken(string token)
        {
            return _Context.RefreshTokens.FirstOrDefault(x => x.Token == token);
        }

        public IEnumerable<RefreshToken> GetByUserId(long userId)
        {
            return base.Get().Where(x => x.UserId == userId);
        }

        public IEnumerable<RefreshToken> Update(IEnumerable<RefreshToken> tokens)
        {
            for (int i = 0; i < tokens.Count(); i++)
            {
                base.Update(tokens.ElementAt(i));
            }
            return tokens;
        }
    }
}
