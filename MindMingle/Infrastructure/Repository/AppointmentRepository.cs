using Application.IRepository;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly MMDbContext _mmDbContext;

        public AppointmentRepository(MMDbContext mmDbContext) : base(mmDbContext)
        {
            _mmDbContext = mmDbContext;
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientIdAsync(string patientId, int pageIndex = 1, int pageSize = 10)
        {
            return await _mmDbContext.Appointments
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Therapist)
                .Include(a => a.Session)
                .Include(a => a.CoWorkingSpace)
                .Include(a => a.EmergencyEnd)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByTherapistIdAsync(string therapistId, int pageIndex = 1, int pageSize = 10)
        {
            return await _mmDbContext.Appointments
                .Where(a => a.TherapistId == therapistId)
                .Include(a => a.Patient)
                .Include(a => a.Session)
                .Include(a => a.CoWorkingSpace)
                .Include(a => a.EmergencyEnd)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
