using CleanArchitectureNetCore.Application.Contracts;
using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Application.RequestModels.Patients;
using CleanArchitectureNetCore.Application.ResponseModels;
using CleanArchitectureNetCore.Common;
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
        private IPatientRepository _patientRepository => unitOfWork.PatientRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly AuthUser authUser;

        public PatientService(IUnitOfWork unitOfWork, AuthUser authUser)
        {
            this.unitOfWork = unitOfWork;
            this.authUser = authUser;
        }

        public PatientDto CreatePatient(PatientRequest request)
        {
            var patient =new Domain.Entities.Patient {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                Description = request.Description,
                Email = request.Email,
                LastVisit = request.LastVisit,
                Age = request.Age,
                DateOfBirth = DateTime.UtcNow.AddYears(-request.Age)
            };
            _patientRepository.Add(patient);
            unitOfWork.SaveChanges();
            return patient.ToDto();
        }
        public PatientDto Update(long id, PatientRequest request)
        {
            var patient = _patientRepository.Get(id);
            if(patient == null)
            {
                throw new Exception("Patient not found");
            }
            patient.FirstName = request.FirstName;
            patient.LastName = request.LastName;
            patient.Email = request.Email;
            patient.Gender = request.Gender;
            patient.Description = request.Description;
            patient.LastVisit = request.LastVisit;
            patient.Age = request.Age;
            patient.DateOfBirth = DateTime.UtcNow.AddYears(-request.Age);


            _patientRepository.Update(patient);
            unitOfWork.SaveChanges();
            return GetPatientById(id);
        }

        public IEnumerable<PatientDto> GetPatients(int pageNumber, int pageSize, string filter)
        {
            var query = _patientRepository.GetAll();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.FirstName.Contains(filter) || p.LastName.Contains(filter) || $"{p.FirstName} {p.LastName}".Contains(filter));
            }

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(p => p.ToDto()).ToList();
        }

        public PatientDto GetPatientById(long id)
        {
            var patient = _patientRepository.GetById(id);
            return patient?.ToDto();
        }

        public IEnumerable<PatientDto> SearchPatients(string searchTerm)
        {
            var query = _patientRepository.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm) || p.Id.ToString().Contains(searchTerm) || $"{p.FirstName} {p.LastName}".Contains(searchTerm));
            }

            return query.Select(p => p.ToDto()).ToList();
        }

        public PatientReportResponse GetReport()
        {
            var report = new PatientReportResponse();
            var patient = _patientRepository.Get(authUser.UserId);
            report.PatientId = patient.Id;
            report.PatientName = $"{patient.FirstName} {patient.LastName}";
            report.PatientEmail = patient.Email;
            report.LastVisit = patient.LastVisit;
            report.NextVisit = patient.NextVisit;
            report.Recommendations = patient.Recommendations?.Select(x=>x.ToDto());
            return report;
        }
    }
}
