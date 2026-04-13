using HealthcareApp.BLL.DTOs;
using HealthcareApp.BLL.Services.Interfaces;
using HealthcareApp.DAL.Repositories.Interfaces;
using HealthcareApp.Entities.Models;

namespace HealthcareApp.BLL.Services.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IDoctorRepository _doctorRepo;

        public AppointmentService(IAppointmentRepository repo, IDoctorRepository doctorRepo)
        {
            _repo = repo;
            _doctorRepo = doctorRepo;
        }

        public async Task<bool> BookAppointmentAsync(AppointmentDTO dto)
        {
            var now = DateTime.Now;

            if (dto.Date.Date < now.Date)
            {
                throw new Exception("Appointment date cannot be in the past");
            }
            var doctor = await _doctorRepo.GetByIdAsync(dto.DoctorId);

            if (doctor == null)
                throw new Exception("Doctor not found");

            // Check time inside doctor availability
            var from = new TimeSpan(doctor.AvailableFrom.Hours, doctor.AvailableFrom.Minutes, 0);
            var to = new TimeSpan(doctor.AvailableTo.Hours, doctor.AvailableTo.Minutes, 0);
            var slot = new TimeSpan(dto.TimeSlot.Hours, dto.TimeSlot.Minutes, 0);

            if (slot < from || slot > to)
            {
                throw new Exception("Selected time is outside doctor's available time");
            }
            try
            {
                var appointments = await _repo.GetAllAsync();

                bool isClash = appointments.Any(a =>
                    a.DoctorId == dto.DoctorId &&
                    a.Date.Date == dto.Date.Date &&
                    Math.Abs((a.TimeSlot - slot).TotalMinutes) < 15
                );

                if (isClash)
                {
                    throw new Exception("Doctor already has appointment within 15 minutes");
                }

                bool patientClash = appointments.Any(a =>
                    a.PatientId == dto.PatientId &&
                    a.Date.Date == dto.Date.Date &&
                    Math.Abs((a.TimeSlot - slot).TotalMinutes) < 15
                );

                if (patientClash)
                {
                    throw new Exception("Patient already has appointment within 15 minutes");
                }

                var appointment = new Appointment
                {
                    DoctorId = dto.DoctorId,
                    PatientId = dto.PatientId,
                    Date = dto.Date,
                    TimeSlot = slot,
                    Status = AppointmentStatus.Booked
                };

                await _repo.AddAsync(appointment);
                await _repo.SaveAsync();

                return true;
            }
            catch (Exception ex)
            {
                // log later if needed
                throw new Exception("Error while booking appointment", ex);
            }
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();

            var now = DateTime.Now;

            return data.Select(a =>
            {
                var status = a.Status;

                // auto-complete logic
                if (a.Date.Date < now.Date ||
                   (a.Date.Date == now.Date && a.TimeSlot < now.TimeOfDay))
                {
                    status = AppointmentStatus.Completed;
                }

                return new AppointmentDTO
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    DoctorId = a.DoctorId,
                    Date = a.Date,
                    TimeSlot = a.TimeSlot,
                    Status = status,
                    PatientName = a.Patient.Name,
                    DoctorName = a.Doctor.Name
                };
            });
        }

        public async Task<AppointmentDTO> GetByIdAsync(int id)
        {
            var a = (await _repo.GetAllAsync()).FirstOrDefault(x => x.AppointmentId == id);

            if (a == null) return null;

            return new AppointmentDTO
            {
                AppointmentId = a.AppointmentId,
                PatientId = a.PatientId,
                DoctorId = a.DoctorId,
                Date = a.Date,
                TimeSlot = a.TimeSlot,
                Status = a.Status
            };
        }

        public async Task<bool> UpdateAsync(AppointmentDTO dto)
        {
            var now = DateTime.Now;

            if (dto.Date.Date < now.Date)
                throw new Exception("Appointment date cannot be in the past");

            var doctor = await _doctorRepo.GetByIdAsync(dto.DoctorId);

            if (doctor == null)
                throw new Exception("Doctor not found");

            var from = new TimeSpan(doctor.AvailableFrom.Hours, doctor.AvailableFrom.Minutes, 0);
            var to = new TimeSpan(doctor.AvailableTo.Hours, doctor.AvailableTo.Minutes, 0);
            var slot = new TimeSpan(dto.TimeSlot.Hours, dto.TimeSlot.Minutes, 0);

            if (slot < from || slot > to)
            {
                throw new Exception("Selected time is outside doctor's available time");
            }

            var existing = (await _repo.GetAllAsync())
                .FirstOrDefault(x => x.AppointmentId == dto.AppointmentId);

            if (existing == null)
                return false;

            if (existing.Date.Date < now.Date ||
               (existing.Date.Date == now.Date && existing.TimeSlot < now.TimeOfDay))
            {
                return false; // real-time completed check
            }

            // duplicate check (ONLY ONCE)
            var appointments = await _repo.GetAllAsync();

            bool isClash = appointments.Any(a =>
                a.DoctorId == dto.DoctorId &&
                a.Date.Date == dto.Date.Date &&
                Math.Abs((a.TimeSlot - slot).TotalMinutes) < 15 &&
                a.AppointmentId != dto.AppointmentId
            );

            if (isClash)
            {
                throw new Exception("Doctor already has appointment within 15 minutes");
            }

            bool patientClash = appointments.Any(a =>
                a.PatientId == dto.PatientId &&
                a.Date.Date == dto.Date.Date &&
                Math.Abs((a.TimeSlot - slot).TotalMinutes) < 15 &&
                a.AppointmentId != dto.AppointmentId
            );

            if (patientClash)
            {
                throw new Exception("Patient already has appointment within 15 minutes");
            }

            if (dto.Date.Date < now.Date ||
                (dto.Date.Date == now.Date && dto.TimeSlot < now.TimeOfDay))
            {
                dto.Status = AppointmentStatus.Completed;
            }

            // update tracked entity
            existing.PatientId = dto.PatientId;
            existing.DoctorId = dto.DoctorId;
            existing.Date = dto.Date;
            existing.TimeSlot = slot;
            existing.Status = dto.Status;

            await _repo.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var a = (await _repo.GetAllAsync()).FirstOrDefault(x => x.AppointmentId == id);

            if (a == null) return false;

            var now = DateTime.Now;

            if (a.Date.Date < now.Date ||
               (a.Date.Date == now.Date && a.TimeSlot < now.TimeOfDay))
            {
                return false;
            }

            await _repo.DeleteAsync(a);
            await _repo.SaveAsync();

            return true;
        }
    }
}