using System;
using System.Collections.Generic;
using System.Text;
using HealthcareApp.Entities.Models;

namespace HealthcareApp.DAL.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync();
        Task<Doctor> GetByIdAsync(int id);
        Task AddAsync(Doctor doctor);
        void Update(Doctor doctor);
        void Delete(Doctor doctor);
        Task SaveAsync();
        Task<bool> HasAppointments(int doctorId);
    }
}
