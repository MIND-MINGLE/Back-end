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
    public class PatientSurveyRepository : GenericRepository<PatientSurvey>, IPatientSurveyRepository
    {
        private readonly MMDbContext _mmDbContext;

        public PatientSurveyRepository(MMDbContext mmDbContext) : base(mmDbContext)
        {
            _mmDbContext = mmDbContext;
        }

        public async Task<List<PatientSurvey>> GetSurveysByPatientIdAsync(string patientId, int pageIndex = 1, int pageSize = 10)
        {
            return await _mmDbContext.PatientSurveys
                .Where(ps => ps.PatientId == patientId)
                .Include(ps => ps.PatientResponses)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
