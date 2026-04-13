using System;
using System.Collections.Generic;
using System.Text;
using HealthcareApp.BLL.DTOs;
using HealthcareApp.BLL.Services.Interfaces;
using HealthcareApp.DAL.Repositories.Interfaces;
using HealthcareApp.Entities.Models;

namespace HealthcareApp.BLL.Services.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;

        public PatientService(IPatientRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PatientDTO>> GetAllAsync()
        {
            var patients = await _repo.GetAllAsync();

            return patients.Select(p => new PatientDTO
            {
                PatientId = p.PatientId,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                MedicalNotes = p.MedicalNotes
            });
        }

        public async Task<IEnumerable<PatientDTO>> SearchAsync(string searchTerm)
        {
            var patients = await _repo.GetAllAsync();

            if (string.IsNullOrEmpty(searchTerm))
                return patients.Select(p => new PatientDTO
                {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    PhoneNumber = p.PhoneNumber,
                    Email = p.Email
                });

            return patients
                .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                         || p.PhoneNumber.Contains(searchTerm))
                .Select(p => new PatientDTO
                {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    PhoneNumber = p.PhoneNumber,
                    Email = p.Email
                });
        }

        public async Task<PatientDTO> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;

            return new PatientDTO
            {
                PatientId = p.PatientId,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                MedicalNotes = p.MedicalNotes
            };
        }

        public async Task<bool> AddAsync(PatientDTO dto)
        {
            try
            {
                var patient = new Patient
                {
                    Name = dto.Name,
                    Age = dto.Age,
                    Gender = dto.Gender,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    MedicalNotes = dto.MedicalNotes
                };

                var result = await _repo.AddAsync(patient);
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding patient", ex);
            }
        }

        public async Task<bool> UpdateAsync(PatientDTO dto)
        {
            var patient = new Patient
            {
                PatientId = dto.PatientId,
                Name = dto.Name,
                Age = dto.Age,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                MedicalNotes = dto.MedicalNotes
            };

            var result = await _repo.UpdateAsync(patient);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            //  VALIDATION
            if (await _repo.HasAppointments(id))
            {
                throw new Exception("Cannot delete patient with appointments");
            }

            var patient = await _repo.GetByIdAsync(id);

            if (patient == null)
                return false;

            await _repo.DeleteAsync(patient);   
            await _repo.SaveAsync();            

            return true;
        }
    }
}