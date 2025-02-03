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
    public class PatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;

        public PatientService(IPatientRepository patientRepository, IUserRepository userRepository)
        {
            _patientRepository = patientRepository;
            _userRepository = userRepository;
        }

        //create patient Info
        public PatientInfoDto Create(PatientInfoRequest request)
        {
            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.Dob,
                Email = request.Email,
                Username = request.Username
            };
            _userRepository.Add(user);


            var patientInfo = new PatientInfo
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Description = request.Description,
                Gender = request.Gender,
                LastVisit = request.LastVisit,
                NextVisit = request.NextVisit,
                User = user
            };

            _patientRepository.Add(patientInfo);
            return patientInfo.ToDto();
        }

        //Get All Patients with Filters
        public IEnumerable<PatientInfoDto> GetPatients(int pageNumber, int pageSize, string filter)
        {
            var query = _patientRepository.GetAll();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.FirstName.Contains(filter) || p.LastName.Contains(filter));
            }

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(p => p.ToDto()).ToList();
        }

        //Get Patient by Id
        public PatientInfoDto GetPatientById(long id)
        {
            var patient = _patientRepository.GetById(id);
            return patient?.ToDto();
        }

        //search paient
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
