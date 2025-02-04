using CleanArchitectureNetCore.Application.Contracts;
using CleanArchitectureNetCore.Application.RequestModels.Patients;
using CleanArchitectureNetCore.Application.RequestModels.Recommendations;
using CleanArchitectureNetCore.Application.Services;
using CleanArchitectureNetCore.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CleanArchitectureNetCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : BaseController
    {
        private readonly IConfiguration _Configuration;
        private readonly AuthService _AuthService;
        private readonly PatientService _patientService;
        private readonly RecommendationService recommendationService;

        public PatientsController(IConfiguration configuration, AuthService authService, PatientService patientsService, 
            RecommendationService recommendationService)
        {
            _Configuration = configuration;
            _AuthService = authService;
            this._patientService = patientsService;
            this.recommendationService = recommendationService;
        }



        [HttpPost]
        public ActionResult<PatientDto> Create(PatientRequest request)
        {
            return Ok(_patientService.CreatePatient(request));
        }

        [HttpPatch("{id}")]
        public ActionResult<PatientDto> Create(long id,PatientRequest request)
        {
            return Ok(_patientService.Update(id, request));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PatientDto>> GetPatients(int pageNumber = 1, int pageSize = 10, string filter = null)
        {
            var patients = _patientService.GetPatients(pageNumber, pageSize, filter);
            return Ok(patients);
        }


        [HttpGet("{id}")]
        public ActionResult<PatientDto> GetPatientById(long id)
        {
            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<PatientDto>> SearchPatients(string searchTerm)
        {
            var patients = _patientService.SearchPatients(searchTerm);
            return Ok(patients);
        }

        [HttpGet("report")]
        public ActionResult<IEnumerable<PatientDto>> GetPatientReport()
        {
            var patients = _patientService.GetReport();
            return Ok(patients);
        }

        #region RECOMMENDATIONS
        [HttpPost("{id}/Recommendations")]
        public ActionResult<RecommendationDto> CreateRecommendation(long id, NewRecommendationRequest request)
        {
            return Ok(recommendationService.Create(id, request));
        }

        [HttpPatch("{id}/Recommendations/{recommendationId}")]
        public ActionResult<RecommendationDto> UpdateRecommendation(long id,long recommendationId,  UpdateRecommendationRequest request)
        {
            return Ok(recommendationService.Update(id, recommendationId, request));
        }
        #endregion
    }


}
