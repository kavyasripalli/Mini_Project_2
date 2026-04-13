using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using HealthcareApp.Entities.Models;
using HealthcareApp.DAL.Repositories.Interfaces;

namespace HealthcareApp.DAL.Repositories.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
            _connectionString = _context.Database.GetDbConnection().ConnectionString;
        }

        public async Task<bool> HasAppointments(int patientId)
        {
            return await _context.Appointments.AnyAsync(a => a.PatientId == patientId);
        }

        public async Task DeleteAsync(Patient patient)
        {
            _context.Patients.Remove(patient);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        // 🔹 Dapper (FAST READ)
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Patients";
            return await db.QueryAsync<Patient>(query);
        }

        public async Task<Patient> GetByIdAsync(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            string query = "SELECT * FROM Patients WHERE PatientId = @Id";
            return await db.QueryFirstOrDefaultAsync<Patient>(query, new { Id = id });
        }

        // 🔹 EF Core (WRITE OPERATIONS)
        public async Task<int> AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
    }
}