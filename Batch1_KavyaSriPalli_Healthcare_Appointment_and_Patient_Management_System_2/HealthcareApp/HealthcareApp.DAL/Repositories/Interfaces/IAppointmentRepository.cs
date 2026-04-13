using System;
using System.Collections.Generic;
using System.Text;
using HealthcareApp.Entities.Models;

namespace HealthcareApp.DAL.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task AddAsync(Appointment appointment);
        Task<bool> ExistsAsync(int doctorId, DateTime date, TimeSpan timeSlot);
        Task SaveAsync();
        Task<Doctor> GetDoctorById(int doctorId);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(Appointment appointment);
    }
}
