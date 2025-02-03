using CleanArchitectureNetCore.Application.RequestModels;
using CleanArchitectureNetCore.Domain.DTOs;
using CleanArchitectureNetCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.Contracts.Repositories
{
    public interface IRecommendationRepository: IRepository<Recommendation>
    {
        bool MarkRecommendationAsCompleted(long id);

    }
}
