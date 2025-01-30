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
    internal class RecommendationRepository: Repository<Recommendation>, IRecommendationRepository
    {
        private readonly AppDbContext _Context;
        public RecommendationRepository(AppDbContext context, AuthUser authUser, IConfiguration configuration) : base(context, configuration)
        {
            _Context = context;
        }

        public bool MarkRecommendationAsCompleted(long id)
        {
            var recommendation = _Context.Recommendations.Find(id);
            if (recommendation == null)
            {
                return false;
            }
            recommendation.IsCompleted = true;
            _Context.Recommendations.Update(recommendation);
            return true;
        }
    }
}
