using HealthcareApp.BLL.DTOs;

namespace HealthcareApp.BLL.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDTO>> GetAllAsync();
        Task<DoctorDTO> GetByIdAsync(int id);
        Task AddAsync(DoctorDTO dto);
        Task UpdateAsync(DoctorDTO dto);
        Task DeleteAsync(int id);
    }
}