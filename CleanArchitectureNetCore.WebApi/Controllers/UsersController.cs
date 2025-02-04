using CleanArchitectureNetCore.Application.RequestModels;
using CleanArchitectureNetCore.Application.Services;
using CleanArchitectureNetCore.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CleanArchitectureNetCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IConfiguration _Configuration;
        private readonly AuthService _AuthService;
        private readonly UserService userService;
        private readonly PatientService _patientService;
        private readonly RecommendationService recommendationService;

        public UsersController(IConfiguration configuration, AuthService authService, UserService userService, 
            RecommendationService recommendationService)
        {
            _Configuration = configuration;
            _AuthService = authService;
            this.userService = userService;
            this.recommendationService = recommendationService;
        }



        [HttpPost]
        public ActionResult<UserDto> Create(UserRequest request)
        {
            return Ok(userService.Create(request));
        }

        [HttpPatch("{id}")]
        public ActionResult<PatientDto> Create(long id, UserRequest request)
        {
            return Ok(userService.Update(id, request));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PatientDto>> GetPatients(int pageNumber = 1, int pageSize = 10, string filter = null)
        {
            var patients = userService.Get(pageNumber, pageSize, filter);
            return Ok(patients);
        }


        [HttpGet("{id}")]
        public ActionResult<PatientDto> GetPatientById(long id)
        {
            var patient = userService.Get(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<PatientDto>> SearchPatients(string searchTerm)
        {
            var patients = userService.Search(searchTerm);
            return Ok(patients);
        }

       
    }


}
