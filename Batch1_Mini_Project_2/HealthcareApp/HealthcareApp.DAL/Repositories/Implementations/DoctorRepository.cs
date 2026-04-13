using System;
using System.Collections.Generic;
using System.Text;
using HealthcareApp.Entities.Models;
using HealthcareApp.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.DAL.Repositories.Implementations
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasAppointments(int doctorId)
        {
            return await _context.Appointments.AnyAsync(a => a.DoctorId == doctorId);
        }
        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors.ToListAsync();
        }

        public async Task<Doctor> GetByIdAsync(int id)
        {
            return await _context.Doctors.FindAsync(id);
        }

        public async Task AddAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
        }

        public void Update(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
        }

        public void Delete(Doctor doctor)
        {
            _context.Doctors.Remove(doctor);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}