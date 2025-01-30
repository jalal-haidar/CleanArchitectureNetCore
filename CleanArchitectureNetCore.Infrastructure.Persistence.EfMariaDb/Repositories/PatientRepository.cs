using CleanArchitectureNetCore.Application.Common.Contracts.Repositories;
using CleanArchitectureNetCore.Common;
using CleanArchitectureNetCore.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitectureNetCore.Application.Contracts.Repositories;

namespace CleanArchitectureNetCore.Infrastructure.Persistence.EfMariaDb.Repositories
{
    public class PatientRepository : Repository<PatientInfo> , IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context, AuthUser authUser, IConfiguration configuration) : base(context, configuration)
        {
            _context = context;
        }

        public IQueryable<PatientInfo> GetAll()
        {
            return _context.PatientInfos.Include(p => p.Recommendations);
        }

        public PatientInfo GetById(long id)
        {
            return _context.PatientInfos.Include(p => p.Recommendations).FirstOrDefault(p => p.Id == id);
        }

    }
}
