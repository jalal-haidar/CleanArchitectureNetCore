using CleanArchitectureNetCore.Application.Contracts.Repositories;
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
    }

}
