using CleanArchitectureNetCore.Application.Contracts;
using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Application.RequestModels;
using CleanArchitectureNetCore.Domain.DTOs;
using CleanArchitectureNetCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.Services
{
    public class RecommendationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRecommendationRepository _recommendationRepository;

        public RecommendationService(IRecommendationRepository recommendationRepository, IUnitOfWork unitOfWork)
        {
            _recommendationRepository = recommendationRepository;
            this.unitOfWork = unitOfWork;
        }

        //Create a Recommendation
        public RecommendationDto Create(RecommendationRequest request)
        {
            var recommendation = new Recommendation
            {
                PatientInfoId = request.PatientInfoId,
                Description = request.Description,
                IsCompleted = false,
                Date = DateTime.UtcNow
            };

            var result = _recommendationRepository.Add(recommendation);
            unitOfWork.SaveChanges();

            return result.ToDto();
        }

        //Get All Recommendations
        public List<RecommendationDto> Get()
        {
            var result = _recommendationRepository.Get();
            unitOfWork.SaveChanges();

            return (List<RecommendationDto>)result;
        }
        
        //Mark As Completed Action
        public bool MarkRecommendationAsCompleted(long id)
        {
            var result = _recommendationRepository.MarkRecommendationAsCompleted(id);
            unitOfWork.SaveChanges();

            return result;
        }

        //Delete Recommendation
        public RecommendationDto Delete(long id)
        {
            var result = _recommendationRepository.Delete(id);
            unitOfWork.SaveChanges();

            return result.ToDto();
        }
    }
}
