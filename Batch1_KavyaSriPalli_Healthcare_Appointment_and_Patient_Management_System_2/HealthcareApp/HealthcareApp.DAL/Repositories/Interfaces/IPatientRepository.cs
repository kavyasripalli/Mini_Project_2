using System;
using System.Collections.Generic;
using System.Text;
using HealthcareApp.Entities.Models;

namespace HealthcareApp.DAL.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient> GetByIdAsync(int id);
        Task<int> AddAsync(Patient patient);
        Task<int> UpdateAsync(Patient patient);
        Task<int> DeleteAsync(int id);
        Task<bool> HasAppointments(int patientId);
        Task DeleteAsync(Patient patient);
        Task SaveAsync();
    }
}