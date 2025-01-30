using CleanArchitectureNetCore.Application.Contracts;
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
        
        public PatientsController(IConfiguration configuration, AuthService authService, PatientService patientsService)
        {
            _Configuration = configuration;
            _AuthService = authService;
            this._patientService = patientsService;
        }



        [HttpGet, Route("")]
        public ActionResult<IEnumerable<PatientInfoDto>> GetPatients(int pageNumber = 1, int pageSize = 10, string filter = null)
        {
            var patients = _patientService.GetPatients(pageNumber, pageSize, filter);
            return Ok(patients);
        }


        [HttpGet, Route("{id}")]
        public ActionResult<PatientInfoDto> GetPatientById(long id)
        {
            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<PatientInfoDto>> SearchPatients(string searchTerm)
        {
            var patients = _patientService.SearchPatients(searchTerm);
            return Ok(patients);
        }


    }


}
