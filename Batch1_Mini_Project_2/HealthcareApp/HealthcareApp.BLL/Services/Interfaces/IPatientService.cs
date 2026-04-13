using System;
using System.Collections.Generic;
using System.Text;
using HealthcareApp.BLL.DTOs;

namespace HealthcareApp.BLL.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientDTO>> GetAllAsync();
        Task<PatientDTO> GetByIdAsync(int id);
        Task<bool> AddAsync(PatientDTO dto);
        Task<bool> UpdateAsync(PatientDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<PatientDTO>> SearchAsync(string searchTerm);
    }
}