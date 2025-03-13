using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository
{
    public interface IPatientSurveyRepository : IGenericRepository<PatientSurvey>
    {
        Task<List<PatientSurvey>> GetSurveysByPatientIdAsync(string patientId, int pageIndex = 1, int pageSize = 10);
    }
}
