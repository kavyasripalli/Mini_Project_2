using System;
using System.Collections.Generic;
using System.Text;
using HealthcareApp.Entities.Models;
using HealthcareApp.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.DAL.Repositories.Implementations
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor> GetDoctorById(int doctorId)
        {
            return await _context.Doctors.FindAsync(doctorId);
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();
        }
        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        public async Task<bool> ExistsAsync(int doctorId, DateTime date, TimeSpan timeSlot)
        {
            return await _context.Appointments.AnyAsync(a =>
                a.DoctorId == doctorId &&
                a.Date.Date == date.Date &&
                a.TimeSlot == timeSlot);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
