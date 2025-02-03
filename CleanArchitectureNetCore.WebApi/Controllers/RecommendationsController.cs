using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Application.RequestModels;
using CleanArchitectureNetCore.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CleanArchitectureNetCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : BaseController
    {
        private readonly IConfiguration _Configuration;
        private readonly AuthService _AuthService;
        private readonly RecommendationService _recommendationsService;

        public RecommendationsController(IConfiguration configuration, AuthService authService, RecommendationService recommendationsService)
        {
            _Configuration = configuration;
            _AuthService = authService;
            this._recommendationsService = recommendationsService;
        }

        //create recommendation
        [HttpPost, Route("")]
        public IActionResult Create(RecommendationRequest request)
        {
            var result = _recommendationsService.Create(request);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }
        
        
        //Get All Recommendations
        public IActionResult Get()
        {
            var result = _recommendationsService.Get();
            if( result == null )
            {
                return BadRequest();
            }
            return Ok(result);
        }

        //Mark Recommendation As Completed
        [HttpPost, Route("{id}")]
        public IActionResult MarkRecommendationAsCompleted(long id)
        {
            var result = _recommendationsService.MarkRecommendationAsCompleted(id);
            if (!result)
            {
                return BadRequest();
            }
            return NoContent();
        }

        //Delete Recommendation
        [HttpDelete, Route("{id}")]
        public IActionResult Delete(long id)
        {
            var result = _recommendationsService.Delete(id);
            if (result == null)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }

}
