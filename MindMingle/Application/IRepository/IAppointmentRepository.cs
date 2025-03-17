using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<List<Appointment>> GetAppointmentsByPatientIdAsync(string patientId, int pageIndex = 1, int pageSize = 10);
        Task<List<Appointment>> GetAppointmentsByTherapistIdAsync(string therapistId, int pageIndex = 1, int pageSize = 10);
    }
}
