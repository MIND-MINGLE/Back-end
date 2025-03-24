using Application.IRepository;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class TherapistSpecializationRepository : GenericRepository<Therapist_Specialization>, ITherapistSpecializationRepository
    {
        public TherapistSpecializationRepository(MMDbContext mMDbContext) : base(mMDbContext)
        {
        }
    }
}
