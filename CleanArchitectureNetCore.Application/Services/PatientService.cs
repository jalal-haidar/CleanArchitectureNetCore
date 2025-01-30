using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.Services
{
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public IEnumerable<PatientInfoDto> GetPatients(int pageNumber, int pageSize, string filter)
        {
            var query = _patientRepository.GetAll();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.FirstName.Contains(filter) || p.LastName.Contains(filter));
            }

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(p => p.ToDto()).ToList();
        }

        public PatientInfoDto GetPatientById(long id)
        {
            var patient = _patientRepository.GetById(id);
            return patient?.ToDto();
        }

        public IEnumerable<PatientInfoDto> SearchPatients(string searchTerm)
        {
            var query = _patientRepository.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm) || p.Id.ToString().Contains(searchTerm));
            }

            return query.Select(p => p.ToDto()).ToList();
        }
    }
}
