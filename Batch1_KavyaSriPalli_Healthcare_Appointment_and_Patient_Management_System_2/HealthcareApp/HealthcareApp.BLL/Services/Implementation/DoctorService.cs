using HealthcareApp.DAL.Repositories.Interfaces;
using HealthcareApp.BLL.DTOs;
using HealthcareApp.Entities.Models;
using HealthcareApp.BLL.Services.Interfaces;

namespace HealthcareApp.BLL.Services.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepo;

        public DoctorService(IDoctorRepository doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }

        public async Task<IEnumerable<DoctorDTO>> GetAllAsync()
        {
            var doctors = await _doctorRepo.GetAllAsync();

            return doctors.Select(d => new DoctorDTO
            {
                DoctorId = d.DoctorId,
                Name = d.Name,
                Specialization = d.Specialization,
                AvailableFrom = d.AvailableFrom,
                AvailableTo = d.AvailableTo
            });
        }

        public async Task<DoctorDTO> GetByIdAsync(int id)
        {
            var d = await _doctorRepo.GetByIdAsync(id);

            if (d == null) return null;

            return new DoctorDTO
            {
                DoctorId = d.DoctorId,
                Name = d.Name,
                Specialization = d.Specialization,
                AvailableFrom = d.AvailableFrom,
                AvailableTo = d.AvailableTo
            };
        }

        public async Task AddAsync(DoctorDTO dto)
        {
            if (dto.AvailableTo <= dto.AvailableFrom)
            {
                throw new Exception("Available To must be greater than Available From");
            }
            try
            {
                var doctor = new Doctor
                {
                    Name = dto.Name,
                    Specialization = dto.Specialization,
                    AvailableFrom = dto.AvailableFrom,
                    AvailableTo = dto.AvailableTo
                };

                await _doctorRepo.AddAsync(doctor);
                await _doctorRepo.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while adding doctor", ex);
            }
        }

        public async Task UpdateAsync(DoctorDTO dto)
        {
            if (dto.AvailableTo <= dto.AvailableFrom)
            {
                throw new Exception("Available To must be greater than Available From");
            }
            var doctor = await _doctorRepo.GetByIdAsync(dto.DoctorId);

            if (doctor == null) return;

            doctor.Name = dto.Name;
            doctor.Specialization = dto.Specialization;
            doctor.AvailableFrom = dto.AvailableFrom;
            doctor.AvailableTo = dto.AvailableTo;

            _doctorRepo.Update(doctor);
            await _doctorRepo.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (await _doctorRepo.HasAppointments(id))
            {
                throw new Exception("Cannot delete doctor with appointments");
            }
            var doctor = await _doctorRepo.GetByIdAsync(id);

            if (doctor == null) return;

            _doctorRepo.Delete(doctor);
            await _doctorRepo.SaveAsync();
        }
    }
}