using Application.IRepository;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class EmergencyEndRepository : GenericRepository<EmergencyEnd>, IEmergencyEndRepository
    {
        public EmergencyEndRepository(MMDbContext mMDbContext) : base(mMDbContext) { }
    }
}
