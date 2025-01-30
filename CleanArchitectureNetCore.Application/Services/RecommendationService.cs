using CleanArchitectureNetCore.Application.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.Services
{
    public class RecommendationService
    {
        private readonly IRecommendationRepository _recommendationRepository;

        public RecommendationService(IRecommendationRepository recommendationRepository)
        {
            _recommendationRepository = recommendationRepository;
        }

        public bool MarkRecommendationAsCompleted(long id)
        {
            return _recommendationRepository.MarkRecommendationAsCompleted(id);
        }
    }
}
