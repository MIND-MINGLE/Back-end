using Application.IRepository;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
	public class PatientRepository : GenericRepository<Patient>, IPatientRepository
	{
		public PatientRepository(MMDbContext context) : base(context) { }
	}
}
