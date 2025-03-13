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
    public class PatientResponseRepository : GenericRepository<PatientResponse>, IPatientResponseRepository
    {
        private readonly MMDbContext _mmDbContext;

        public PatientResponseRepository(MMDbContext mmDbContext) : base(mmDbContext)
        {
            _mmDbContext = mmDbContext;
        }

        public async Task<List<PatientResponse>> GetResponsesBySurveyIdAsync(string surveyId, int pageIndex = 1, int pageSize = 10)
        {
            return await _mmDbContext.PatientResponses
                .Where(pr => pr.PatientSurveyId == surveyId)
                .Include(pr => pr.Question)
                .Include(pr => pr.Answer)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
