using System;
using System.Collections.Generic;
using System.Text;
using HealthcareApp.BLL.DTOs;

namespace HealthcareApp.BLL.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<bool> BookAppointmentAsync(AppointmentDTO dto);
        Task<IEnumerable<AppointmentDTO>> GetAllAsync();
        Task<AppointmentDTO> GetByIdAsync(int id);
        Task<bool> UpdateAsync(AppointmentDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}