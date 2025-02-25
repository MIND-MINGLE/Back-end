using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Application.IRepository;
namespace Infrastructure.Repository
{
	public class TherapistRepository : GenericRepository<Therapist>, ITherapistRepository
    {
        public TherapistRepository(MMDbContext mMDbContext) : base(mMDbContext)
        {
        }
    }
}
