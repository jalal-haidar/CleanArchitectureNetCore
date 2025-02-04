using CleanArchitectureNetCore.Application.Contracts;
using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Application.RequestModels;
using CleanArchitectureNetCore.Application.RequestModels.Recommendations;
using CleanArchitectureNetCore.Domain.DTOs;
using CleanArchitectureNetCore.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CleanArchitectureNetCore.Application.Services
{
    public class RecommendationService
    {
        private readonly IUnitOfWork unitOfWork;
        private IRecommendationRepository _recommendationRepository=>unitOfWork.RecommendationRepository;

        public RecommendationService( IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //Create a Recommendation
        public RecommendationDto Create(RecommendationRequest request)
        {
            var recommendation = new Recommendation
            {
                PatientId = request.PatientId,
                Description = request.Description,
                IsCompleted = request.IsCompleted??false,
                DateCompleted = request.IsCompleted ?? false ? DateTime.UtcNow : (DateTime?)null,
                Date = DateTime.UtcNow
            };

            var result = _recommendationRepository.Add(recommendation);
            unitOfWork.SaveChanges();

            return result.ToDto();
        }
        public RecommendationDto Create(long patientId, NewRecommendationRequest request)
        {
            var recommendation = new Recommendation
            {
                PatientId = patientId,
                Description = request.Description,
                Date = DateTime.UtcNow
            };

            var result = _recommendationRepository.Add(recommendation);
            unitOfWork.SaveChanges();

            return result.ToDto();
        }
        public RecommendationDto Update(long patientId, long id, UpdateRecommendationRequest request)
        {
            var result = _recommendationRepository.Get(id);
            if(result == null)
            {
                throw new Exception("Recommendation not found");
            }
            if(result.PatientId != patientId)
            {
                throw new Exception("Recommendation not found");
            }
            result.IsCompleted = request.IsCompleted;
            result.DateCompleted = request.IsCompleted ? DateTime.UtcNow : (DateTime?)null;
            _recommendationRepository.Update(result);
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
