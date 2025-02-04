using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Common;
using CleanArchitectureNetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Infrastructure.Persistence.Repositories
{
    public class UserRespository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _Context;

        public UserRespository(AppDbContext context, AuthUser authUser, IConfiguration configuration) : base(context, configuration)
        {
            _Context = context;
        }

        public IEnumerable<User> GetByRole(long roleId) => base.Get().Where(x => x.RoleId == roleId).ToList();

       
        public async Task<User> GetById(long id)
        {
            return await base.Get()
                      .FirstOrDefaultAsync(x => x.Id == id);
        }



        public async Task<User> GetByIdForced(long id)
        {
            return await _Context.Set<User>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public User? Authenticate(string email, string password)
        {
            var user = base.Get().FirstOrDefault(x => x.Email == email);
            if (user == null) return null;
            // TODO: remove the line below to avoid plain passwords
            if(user.Salt==null) return user.Password == password ? user : null;
            if (Utilities.HashPassword(password, user.Salt)!= user.Password) return null;
            return user;
        }
    }
}
